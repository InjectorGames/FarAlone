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

    public float nextWaypointDistance;
    public Transform target;
    private Rigidbody2D rb;
    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;
    private float distanceToTarget;
    private float elapsedTime = 0f;

    private Seeker seeker;

    void Start()
    {
        name = "NAME";
        nextWaypointDistance = 3;

        rb = this.GetComponent<Rigidbody2D>();
        seeker = this.GetComponent<Seeker>();
    
        InvokeRepeating("UpdatePath", 0f,.5f);
        seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    void UpdatePath()
    {
        if(seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        Debug.Log("test");
    }


    void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void Update() 
    {
        distanceToTarget = Vector2.Distance(rb.position, target.position);

        if(HP <= 0)
            Destroy(gameObject);
        if(distanceToTarget <= AtkRange)
        {
            if(Time.time >= elapsedTime)
            {
                elapsedTime = Time.time + AtkSpeed;
                Attack();
            }
        }
        else
        {
            elapsedTime = Time.time + AtkSpeed;
        }

        
    }
    

   void FixedUpdate()
   {
       if(distanceToTarget <= VisRange)
        {   
            if(path == null)
                return;
            if(currentWaypoint >= path.vectorPath.Count)
            {
                reachedEndOfPath = true;
                return;
            }else
            {
                reachedEndOfPath = false;
            }

            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            Vector2 force = direction * MovSpeed * Time.deltaTime;     

            rb.AddForce(force);

            float waypointDistance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

            if(waypointDistance < nextWaypointDistance)
            {
                currentWaypoint++;
            }
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
