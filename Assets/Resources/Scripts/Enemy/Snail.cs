using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InjectorGames.FarAlone.Controllers;

namespace InjectorGames.FarAlone.Enemies
{
    public class Snail : Enemy
    {
        [SerializeField]
        public Rigidbody2D rb;

        protected override void Start()
        {
            base.Start();
            rb = this.GetComponent<Rigidbody2D>();  
            name = "Snail";
        }

        protected override void Update()
        {
            base.Update();
        }

        protected override void HPCheck()
        {
            base.HPCheck();
        }

        void Attack()
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = 0f;

            PlayerController.Instance.Healt -= AtkDamage;
        }

        protected override void OnTriggerEnter2D(Collider2D trigger)
        {
            base.OnTriggerEnter2D(trigger);
        }
    }
}