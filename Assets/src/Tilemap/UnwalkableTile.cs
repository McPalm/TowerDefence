using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class UnwalkableTile : TileBase
{
    public Sprite sprite;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        tileData.sprite = sprite;
    }

    public override bool StartUp(Vector3Int location, ITilemap tilemap, GameObject go)
    {
        if (EditorApplication.isPlaying)
        {
            GameObject o = new GameObject("Obstruction");
            o.transform.position = location + new Vector3(.5f, .5f);
            o.AddComponent<Building.Obstruction>();
        }
        
        return true;
    }



#if UNITY_EDITOR

    [MenuItem("Assets/Create/Obstruction Tile")]
    public static void CreateObstructionTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Obstruction Tile", "New Obstruction Tile", "asset", "Save Obstruction Tile", "Assets");

        if (path == "")
            return;

        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<UnwalkableTile>(), path);
    }
#endif
}
