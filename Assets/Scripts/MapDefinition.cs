using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Level Definition")]
public class MapDefinition : ScriptableObject {
    public SceneAsset levelScene;
    [SerializeField]
    private string displayName;
    public MapType mapType;
    public GameObject mapPreview;

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