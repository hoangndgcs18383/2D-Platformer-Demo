using System;
using System.Collections.Generic;
using SAGE.Framework.SaveGame;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace platformer_2d.demo
{
    [Serializable]
    public class MapObject
    {
        public GameObject gameObject;
        public bool isSpawned = false;
    }
    
    public class MapController : MonoBehaviour
    {
        [SerializeField] private Tilemap tilemap;
        [SerializeField] private Tile tile;

        [SerializeField] private int maxTileX = 20;
        [SerializeField] private int maxTileY = 20;

        [SerializeField] private Coin coinPrefab;
        [SerializeField] private AIBehaviour enemyPrefab;

        private Dictionary<Vector2Int, Tile> _tiles = new Dictionary<Vector2Int, Tile>();
        private Dictionary<Vector2Int, MapObject> _coins = new Dictionary<Vector2Int, MapObject>();
        private Dictionary<Vector2Int, MapObject> _enemies = new Dictionary<Vector2Int, MapObject>();

        private PlayerView _playerView;
        private Vector2Int _cachedCellPosition;
        private bool _isInitialized;

        private void Start()
        {
            _isInitialized = false;
            Application.targetFrameRate = 60;
        }

        public void SetPlayerView(PlayerView playerView)
        {
            _playerView = playerView;
        }

        public void InitializeMap()
        {
            tilemap.ClearAllTiles();
            FetchMap();
            Vector3Int cellPosition = tilemap.WorldToCell(_playerView.transform.position);
            CheckCellTilePosition(cellPosition);
            tilemap.RefreshAllTiles();
            RemoveAllTilesNotInCellPosition(cellPosition);
            _isInitialized = true;
        }

        private void LateUpdate()
        {
            if (_playerView == null || !_isInitialized) return;
            Vector3Int cellPosition = tilemap.WorldToCell(_playerView.transform.position);
            CheckCellTilePosition(cellPosition);
            tilemap.RefreshAllTiles();
            RemoveAllTilesNotInCellPosition(cellPosition);
        }

        private void FetchMap()
        {
            _tiles.Clear();
            _coins.Clear();
            _enemies.Clear();

            SaveGameAPI.Instance.Load<List<MapReaderJson>>("map.json", list =>
            {
                if (list == null)
                {
                    return;
                }

                foreach (var mapReaderJson in list)
                {
                    switch (mapReaderJson.type)
                    {
                        case 1:
                            _tiles.Add(ParseValue(mapReaderJson.value), tile);
                            break;
                        case 2:
                            _coins.Add(ParseValue(mapReaderJson.value), new MapObject());
                            break;
                        case 3:
                            _enemies.Add(ParseValue(mapReaderJson.value), new MapObject());
                            break;
                    }
                }
            });
        }

        private void CheckCellTilePosition(Vector3Int cellPosition)
        {
            for (int x = cellPosition.x - maxTileX; x < cellPosition.x + maxTileX; x++)
            {
                for (int y = cellPosition.y - maxTileY; y < cellPosition.y + maxTileY; y++)
                {
                    Vector2Int key = new Vector2Int(x, y);
                    if (_tiles.ContainsKey(key))
                    {
                        tilemap.SetTile(new Vector3Int(x, y, 0), _tiles[key]);
                    }

                    if (_coins.ContainsKey(key) && !_coins[key].isSpawned)
                    {
                        //spawn coin
                        GameObject coin = ObjectPoolManager.Instance.GetOrCreateObject(GameConstant.COIN,
                            new Vector3(x, y, 0),
                            Quaternion.identity);
                        
                        _coins[key].isSpawned = true;
                        _coins[key].gameObject = coin;
                    }

                    if (_enemies.ContainsKey(key) && !_enemies[key].isSpawned)
                    {
                        //spawn enemy
                        GameObject aiBehaviour = ObjectPoolManager.Instance.GetOrCreateObject(GameConstant.ENEMY_AI,
                            new Vector3(x, y, 0),
                            Quaternion.identity);
                        
                        _enemies[key].isSpawned = true;
                        _enemies[key].gameObject = aiBehaviour;
                    }
                }
            }
        }

        private void RemoveAllTilesNotInCellPosition(Vector3Int cellPosition)
        {
            _cachedCellPosition = (Vector2Int)cellPosition;
            //remove tiles that are not in the view
            if (_tiles.Count > 0)
            {
                foreach (var t in _tiles)
                {
                    if (Vector2Int.Distance(t.Key, _cachedCellPosition) > maxTileX)
                    {
                        tilemap.SetTile(new Vector3Int(t.Key.x, t.Key.y, 0), null);
                    }
                }
            }

            if (_coins.Count > 0)
            {
                List<Vector2Int> coinsToReturn = new List<Vector2Int>();

                foreach (var c in _coins.Keys)
                {
                    if (_coins[c].isSpawned &&
                        Vector2Int.Distance(c, _cachedCellPosition) > maxTileX)
                    {
                        ObjectPoolManager.Instance.ReturnObjectToPool(GameConstant.COIN, _coins[c].gameObject);
                        coinsToReturn.Add(c);
                    }
                }

                for (int i = 0; i < coinsToReturn.Count; i++)
                {
                    _coins[coinsToReturn[i]].isSpawned = false;
                }
            }

            if (_enemies.Count > 0)
            {
                List<Vector2Int> enemyToReturn = new List<Vector2Int>();
                
                foreach (var e in _enemies.Keys)
                {
                    if (_enemies[e].isSpawned &&
                        Vector2Int.Distance(e, _cachedCellPosition) > maxTileX)
                    {
                        ObjectPoolManager.Instance.ReturnObjectToPool(GameConstant.ENEMY_AI, _enemies[e].gameObject);
                        enemyToReturn.Add(e);
                    }
                }
                
                for (int i = 0; i < enemyToReturn.Count; i++)
                {
                    _enemies[enemyToReturn[i]].isSpawned = false;
                }
            }
        }

        private Vector2Int ParseValue(string value)
        {
            string[] values = value.Split('|');
            return new Vector2Int(int.Parse(values[0]), int.Parse(values[1]));
        }
        
        public void RemoveCoin(Vector2Int position)
        {
            if (_coins.ContainsKey(position))
            {
                _coins.Remove(position);
            }
        }
    }
}