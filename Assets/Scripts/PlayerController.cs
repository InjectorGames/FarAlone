using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D))]
public sealed class PlayerController : MonoBehaviour
{
    public float speed = 0.5f;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>() ?? throw new NullReferenceException();
    }

    private void FixedUpdate()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        if(horizontal * vertical == 0)
            rb.velocity = new Vector3(horizontal * speed, vertical * speed, 0f);
        else
            rb.velocity = new Vector3(horizontal * (speed / 1.414f), vertical * (speed / 1.414f), 0f);
    }
}
