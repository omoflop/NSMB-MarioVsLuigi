using System;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Level Definition")]
public class MapDefinition : ScriptableObject {
    private string _scenePath;
    [SerializeField] private string displayName;
    public MapType mapType;
    public GameObject mapPreview;

    // Used internally so users don't have to worry about finding the scene's path
    #if UNITY_EDITOR
        [SerializeField] private SceneAsset scene;
        
        private void OnValidate() {
            _scenePath = AssetDatabase.GetAssetPath(scene);
        }
    #endif
    
    

    public string GetScenePath() {
        return _scenePath;
    }
    
    public string GetDisplayName() {
        string str = displayName;
        if (mapType == MapType.Custom) str = $"[C] {str}";
        if (mapType == MapType.Debug) str = $"[WIP] {str}";
        return str;
    }
}

public enum MapType {
    Normal,
    Custom,
    Debug
}