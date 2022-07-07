using System;
using UnityEngine;

namespace EditorOnly {
	[ExecuteAlways]
	public class GridSnap : MonoBehaviour {
		#if UNITY_EDITOR

		private void Update() {
			Vector3 p = transform.localPosition;
			transform.localPosition = new Vector3(Mathf.Round(p.x / 0.25f) * 0.25f, Mathf.Round(p.y / 0.25f) * 0.25f, p.z);
		}

		#endif
	}
}