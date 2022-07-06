using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "ScriptableObjects/Map Definition")]
public class MapDefinition : ScriptableObject {
    [SerializeField] private int _sceneId = -1;
    [SerializeField] private string displayName;
    public MapType mapType;
    public GameObject mapPreview;

    // Used internally so users don't have to worry about finding the scene's path
    #if UNITY_EDITOR_WIN
        [SerializeField] private SceneAsset scene;
        
        private void OnValidate() {
            _sceneId = SceneUtility.GetBuildIndexByScenePath(AssetDatabase.GetAssetPath(scene));
        }
    #endif
    
    public string GetDisplayName() {
        string str = displayName;
        if (mapType == MapType.Custom) str = $"[C] {str}";
        if (mapType == MapType.Debug) str = $"[WIP] {str}";
        return str;
    }

    public int GetSceneIndex() {
        return _sceneId;
    }
}

public enum MapType {
    Normal,
    Custom,
    Debug
}