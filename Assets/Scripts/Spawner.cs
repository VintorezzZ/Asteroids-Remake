using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private Player playerPrefab;
        private List<SpawnPoint> _spawnPoints = new List<SpawnPoint>();

        private void Awake()
        {
            _spawnPoints.AddRange(GetComponentsInChildren<SpawnPoint>());
        }

        public Player SpawnPlayer()
        {
            var player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            player.Init();
            return player;
        }

        public void SpawnEnemyShip()
        {
            var spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Count - 1)].transform.position;
            var ship = SpawnObject(PoolType.EnemyShip, spawnPoint, Quaternion.identity);
            
            if(ship)
                ship.GetComponent<EnemyShip>().Init(GameManager.Instance.player);
        }

        public void SpawnAsteroids(int count, PoolType type, Vector3 position = new Vector3())
        {
            for (int i = 0; i < count; i++)
            {
                Vector3 spawnPosition = Vector3.zero;
                
                if(type == PoolType.BigAsteroid)
                    spawnPosition = _spawnPoints[Random.Range(0, _spawnPoints.Count - 1)].transform.position;
                else if (type == PoolType.SmallAsteroid)
                    spawnPosition = position;
                else
                    return;
                
                var asteroid = SpawnObject(type, spawnPosition, Quaternion.identity)?.GetComponent<Asteroid>();
                
                if(asteroid)
                {
                    if(type == PoolType.BigAsteroid)
                        GameManager.Instance.aliveAsteroids.Add(asteroid);
                    
                    asteroid.Init();
                }            
            }
        }

        private PoolItem SpawnObject(PoolType type, Vector3 position, Quaternion rotation)
        {
            if (GameManager.Instance.IsGameOver)
                return null;
            
            var item = PoolManager.Get(type);

            if (!item)
                return null;
            
            item.gameObject.SetActive(true);
            item.transform.position = position;
            item.transform.rotation = rotation;
            GameManager.Instance.aliveEntities.Add(item);
            return item;
        }
    }
}