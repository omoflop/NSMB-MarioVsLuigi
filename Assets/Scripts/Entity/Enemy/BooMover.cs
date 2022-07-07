using System;
using Photon.Pun;
using UnityEngine;

namespace Entity.Enemy {
	public class BooMover : KillableEntity {
		public float speed = 1, despawnDistance = 8;
		public bool lookingLeft;
		private new void FixedUpdate() {
			if (GameManager.Instance && GameManager.Instance.gameover) {
				body.velocity = Vector2.zero;
				body.angularVelocity = 0;
				animator.enabled = false;
				body.isKinematic = true;
				return;
			}
			
			if (Frozen) {
				body.velocity = Vector2.zero;
			}
			
			if (!animator.GetCurrentAnimatorStateInfo(0).IsName("turn")) {
				if (IsLookedAt()) {
					animator.Play("shy");
					body.velocity = Vector2.zero;
				}
				else {
					animator.Play("wiggle");
					FaceClosestPlayer();
					body.velocity = new(speed * (lookingLeft ? -1 : 1), body.velocity.y);
				}
				
				transform.localScale = new Vector3(3 * (lookingLeft ? -1 : 1), transform.localScale.y, transform.localScale.z);
			}

			if (!Frozen && photonView.IsMine )
				DespawnCheck();

			
		}

		public override void InteractWithPlayer(PlayerController player) {
	        if (Frozen || player.Frozen)
	            return;
	        
	        if (player.invincible > 0 || player.inShell || player.state == Enums.PowerupState.MegaMushroom) {
		        photonView.RPC("Kill", RpcTarget.All);
	            return;
	        }
	        

	        player.photonView.RPC("Powerdown", RpcTarget.All, false);
	    }

		private void DespawnCheck() {
			foreach (PlayerController player in GameManager.Instance.allPlayers) {
	            if (!player)
	                continue;

	            if (Utils.WrappedDistance(player.body.position, body.position) < despawnDistance)
	                return;
	        }

	        PhotonNetwork.Destroy(photonView);
	    }

		private bool IsLookedAt() {
			foreach (PlayerController player in GameManager.Instance.allPlayers) {
				if (!player || Utils.WrappedDistance(player.body.position, body.position) > despawnDistance)
					continue;
				
				//player.body.position, body.position
				bool side = player.body.position.x - body.position.x < 0;
				if ((side && player.facingRight) || (!side && !player.facingRight)) {
					return true;
				}
			}

			return false;
		}

		private bool FaceClosestPlayer() {
			PlayerController closestPlayer = null;
			float dist = float.MaxValue;
			foreach (PlayerController player in GameManager.Instance.allPlayers) {
				float curDist = Utils.WrappedDistance(player.body.position, body.position);
				if (!player || curDist > despawnDistance)
					continue;
				if (dist > curDist) {
					closestPlayer = player;
					dist = curDist;
				}
			}

			if (!closestPlayer) return false;
			bool side = closestPlayer.body.position.x - body.position.x > 0;

			if (lookingLeft != side) {
				animator.Play("turn");
			}

			lookingLeft = side;
			return true;
		}

	    [PunRPC]
	    public override void Kill() {
	        SpecialKill(!left, false, 0);
	    }

	    [PunRPC]
	    public override void SpecialKill(bool right, bool groundpound, int combo) {
	        body.velocity = new Vector2(0, 2.5f);
	        body.constraints = RigidbodyConstraints2D.None;
	        body.angularVelocity = 400f * (right ? 1 : -1);
	        body.gravityScale = 1.5f;
	        body.isKinematic = false;
	        hitbox.enabled = false;
	        animator.speed = 0;
	        gameObject.layer = LayerMask.NameToLayer("HitsNothing");
	        if (groundpound)
	            Instantiate(Resources.Load("Prefabs/Particle/EnemySpecialKill"), body.position + new Vector2(0, 0.5f), Quaternion.identity);

	        dead = true;
	        photonView.RPC("PlaySound", RpcTarget.All, Enums.Sounds.Enemy_Shell_Kick);
	    }
	}
}
