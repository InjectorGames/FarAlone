using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        List<GameObject> enemies = new List<GameObject>();

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
            if(enemies.Count >= maxSpawn)
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
                enemies.Add(_enemy);
            }

        }

        bool CanSpawn()
        {
            return Time.time >= nextSpawnTime;
        }

        void UpdateEnemy()
        {
            if(enemies != null)
            {
                foreach(GameObject _enemy in enemies)
                {
                    Debug.Log(_enemy.GetComponent<Enemy>().HP);
                    if(_enemy.GetComponent<Enemy>().HP <= 0)
                    {
                        enemies.RemoveAt(enemies.IndexOf(_enemy));
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
            Gizmos.DrawWireCube(transform.position, new Vector2(spawnField.x, spawnField.y));
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