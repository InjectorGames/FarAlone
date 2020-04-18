using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using InjectorGames.FarAlone.Controllers;

public class Enemy : MonoBehaviour
{
    public float HP;
    public float AtkDamage;
    public float AtkRange;
    public float AtkSpeed;
    public float MovSpeed;
    public float VisRange;

    public Transform target;
    private Rigidbody2D rb;

    private float elapsedTime = 0f;


    void Start()
    {
        name = "Snail";

        rb = this.GetComponent<Rigidbody2D>();
    
    }

    void Update()
    {
        HPCheck();
    }

    void HPCheck()
    {
        if(HP <= 0 )
        {
            Destroy(gameObject);
        }

    }




    void OnTriggerEnter2D(Collider2D trigger)
    {
        if(trigger.CompareTag("Blast"))
        {
            HP -= 7.5f;
            Debug.Log(HP);
            Destroy(trigger.gameObject);
        }
    }

    void Attack()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = 0f;

        PlayerController.Instance.Healt -= AtkDamage;
    }
}
