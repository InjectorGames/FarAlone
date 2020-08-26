using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InjectorGames.FarAlone.Enemies;

namespace InjectorGames.FarAlone.Spawners{
    public class EnemySpawner : MonoBehaviour
    {
        [Header ("Spawner settings")]
        [SerializeField]
        Transform target;
        [SerializeField]
        GameObject enemy;
        [SerializeField]
        float spawnDelay;
        float nextSpawnTime;
        [SerializeField]
        Vector2 spawnField;
        [SerializeField]
        float pointAreaRadius;
        [SerializeField]
        public LayerMask unspawnable;
        [SerializeField]
        List<ESPoint> points = new List<ESPoint>();
        [SerializeField]
        List<GameObject> currentEnemies = new List<GameObject>();

        const int maxSpawn = 2;

        void Start()
        {
            UpdateEnemy();
            CreateField();
        }

        void Update()
        {
            if(!(IsMaxSpawned()))
            {
                Spawn();
            }
            UpdateEnemy();
        }
        
        bool IsMaxSpawned()
        {
            if(currentEnemies.Count >= maxSpawn)
                return true;
            return false;
        }

        void Spawn()
        {
            int spawnPos = Random.Range(0, points.Count);
            if(points[spawnPos].spawnable && !(points[spawnPos].IsVisible()) && CanSpawn())
            {
                nextSpawnTime = Time.time + spawnDelay;
                GameObject _enemy = Instantiate(enemy, (Vector3) points[spawnPos].position, new Quaternion(0, 0, 0, 0)) as GameObject;
                currentEnemies.Add(_enemy);
            }

        }

        bool CanSpawn()
        {
            return Time.time >= nextSpawnTime;
        }

        void UpdateEnemy()
        {
            if(currentEnemies != null)
            {
                foreach(GameObject _enemy in currentEnemies)
                {
                    if(_enemy.GetComponent<Snail>().HP <= 0)
                    {
                        currentEnemies.RemoveAt(currentEnemies.IndexOf(_enemy));
                        return;
                    }
                }
            }
        }

        void CreateField()
        {
            float spawnFieldSpace = spawnField.x * spawnField.y;
            float pointSpace = Mathf.Pow(pointAreaRadius, 2);

            int pointAmount = Mathf.FloorToInt(spawnFieldSpace / pointSpace);

            float pointFieldSpace = pointAreaRadius * spawnField.x;

            int pointAmountPerX = Mathf.FloorToInt(pointFieldSpace / pointSpace);

            int pointAmountPerY = pointAmount / pointAmountPerX;

            Vector2 leftEdge = new Vector2(this.transform.position.x - spawnField.x / 2 + pointAreaRadius / 2, this.transform.position.y - spawnField.y / 2 + pointAreaRadius / 2);

            for(int lineY = 0; lineY < pointAmountPerY; lineY++)
            {
                for(int lineX = 0; lineX < pointAmountPerX; lineX++)
                {
                    Vector2 pointPos = new Vector2(leftEdge.x + (float)lineX * pointAreaRadius, leftEdge.y + (float)lineY * pointAreaRadius);
                    bool spawnable = !(Physics2D.OverlapCircle(pointPos, pointAreaRadius, unspawnable));
                    ESPoint point = new ESPoint(spawnable, pointPos);
                    points.Add(point);
                }
            }

        }

        void OnDrawGizmos() 
        {
            Gizmos.DrawWireCube(transform.position, spawnField);
            if(points != null)
            {
                foreach (ESPoint p in points)
                {
                    Gizmos.color = (p.spawnable)?Color.green:Color.red;
                    Gizmos.DrawCube(p.position, Vector2.one * (pointAreaRadius - .1f));
                }
            }
        }
    }
}