#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using SAGE.Framework.SaveGame;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace platformer_2d.demo
{
    [Serializable]
    public class MapReaderJson
    {
        public int type;
        public string value;
    }

    public class MapTool2D : MonoBehaviour
    {
        public Grid grid;
        public Tilemap tilemap;

        public Tilemap drawMap;
        public Tile tile;

        public List<MapReaderJson> mapReaderJsons = new List<MapReaderJson>();

        [ContextMenu("DebugTilemap")]
        public void DebugTilemap()
        {
            mapReaderJsons.Clear();
            for (int x = tilemap.cellBounds.xMin; x < tilemap.cellBounds.xMax; x++)
            {
                for (int y = tilemap.cellBounds.yMin; y < tilemap.cellBounds.yMax; y++)
                {
                    Vector3Int localPlace = new Vector3Int(x, y, (int)tilemap.transform.position.y);
                    //Vector3 place = tilemap.CellToWorld(localPlace);
                    if (tilemap.HasTile(localPlace))
                    {
                        switch (tilemap.GetTile(localPlace).name)
                        {
                            case "Square":
                                mapReaderJsons.Add(new MapReaderJson
                                {
                                    type = 1,
                                    value = ParseValueString(localPlace)
                                });
                                break;
                            case "Circle":
                                mapReaderJsons.Add(new MapReaderJson
                                {
                                    type = 2,
                                    value = ParseValueString(localPlace)
                                });
                                break;
                            case "Triangle":
                                mapReaderJsons.Add(new MapReaderJson
                                {
                                    type = 3,
                                    value = ParseValueString(localPlace)
                                });
                                break;
                        }
                    }
                }
            }

            SaveGameAPI.Instance.Save(mapReaderJsons, "map.json");
            //Debug.Log(Application.persistentDataPath);
        }

        [ContextMenu("DrawMap")]
        public void DrawMap()
        {
            mapReaderJsons.Clear();
            SaveGameAPI.Instance.Load<List<MapReaderJson>>("map.json", list =>
            {
                mapReaderJsons = list;
                if (mapReaderJsons == null)
                {
                    return;
                }

                foreach (var mapReaderJson in mapReaderJsons)
                {
                    if (mapReaderJson.type == 1)
                    {
                        Vector3Int value = ParseValue(mapReaderJson.value);
                        tilemap.SetTile(value, tile);
                    }
                }
            });
        }

        private Vector3Int ParseValue(string value)
        {
            string[] values = value.Split('|');
            return new Vector3Int(int.Parse(values[0]), int.Parse(values[1]), int.Parse(values[2]));
        }

        private string ParseValueString(Vector3Int value)
        {
            return string.Join("|", new string[] { value.x.ToString(), value.y.ToString(), value.z.ToString() });
        }
    }
}

#endif