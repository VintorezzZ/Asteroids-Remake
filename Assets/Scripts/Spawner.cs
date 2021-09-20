using System.Collections.Generic;
using System.Linq;
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
            ship.GetComponent<EnemyShip>().Init(GameManager.instance.player);
        }

        public void SpawnAsteroids(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var position = _spawnPoints[Random.Range(0, _spawnPoints.Count - 1)].transform.position;
                var asteroid = SpawnObject(PoolType.BigAsteroid, position, Quaternion.identity).GetComponent<Asteroid>();
                asteroid.Init();
                GameManager.instance.aliveAsteroids.Add(asteroid);
            }
        }

        private PoolItem SpawnObject(PoolType type, Vector3 position, Quaternion rotation)
        {
            var item = PoolManager.Get(type);
            item.gameObject.SetActive(true);
            item.transform.position = position;
            item.transform.rotation = rotation;
            return item;
        }
    }
}