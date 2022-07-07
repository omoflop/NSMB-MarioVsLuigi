using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteAlways]
public class CoinPlacer : MonoBehaviour
{
    #if UNITY_EDITOR

    [SerializeField] private Tilemap tilemap;
    [SerializeField] private GameObject coinPrefab;
    
    public bool spawnCoins = false;
    public bool removeCoinTiles = false;

    private void Update() {
        if (removeCoinTiles) {
            RemoveCoinTiles();
            removeCoinTiles = false;
        }
        if (!spawnCoins) return;
        SpawnCoins();
        spawnCoins = false;
    }

    public void SpawnCoins() {
        for (int i = transform.childCount - 1; i >= 0; i--) {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
        for (int x = tilemap.cellBounds.min.x; x < tilemap.cellBounds.max.x; x++){
            for (int y = tilemap.cellBounds.min.y; y < tilemap.cellBounds.max.y; y++){
                for (int z = tilemap.cellBounds.min.z; z < tilemap.cellBounds.max.z; z++){
                    TileBase t = tilemap.GetTile( new Vector3Int(x,y,z));
                    if (!t || t.name != "Coin") continue;
                    GameObject newCoin = Instantiate(coinPrefab, transform, false);
                    newCoin.transform.localPosition = new Vector3(x/2f, y/2f, 0);
                }
            }
        }
    }

    public void RemoveCoinTiles() {
        for (int x = tilemap.cellBounds.min.x; x < tilemap.cellBounds.max.x; x++){
            for (int y = tilemap.cellBounds.min.y; y < tilemap.cellBounds.max.y; y++){
                for (int z = tilemap.cellBounds.min.z; z < tilemap.cellBounds.max.z; z++){
                    TileBase t = tilemap.GetTile( new Vector3Int(x,y,z));
                    if (!t || t.name != "Coin") continue;
                    tilemap.SetTile(new Vector3Int(x, y, z), null);
                }
            }
        }
    }


    #endif
}
