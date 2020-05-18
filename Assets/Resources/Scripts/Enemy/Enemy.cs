using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

namespace InjectorGames.FarAlone.Enemies
{
    public class Enemy : MonoBehaviour
    {
        public float HP;
        public float AtkDamage;
        public float AtkRange;
        public float AtkSpeed;
        public float MovSpeed;
        public float VisRange;

        public Transform target;

        protected virtual void Start() 
        {
            target = GameObject.FindWithTag("Player").transform;
        }
        protected virtual void Update()
        {
            HPCheck();
        }

        protected virtual void HPCheck()
        {
            if(HP <= 0 )
            {
                Destroy(gameObject);
            }

        }

        protected virtual void OnTriggerEnter2D(Collider2D trigger)
        {
            if(trigger.CompareTag("Blast"))
            {
                HP -= 7.5f;
                Destroy(trigger.gameObject);
            }
        }
    }
}
