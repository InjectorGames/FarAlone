using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Rendering;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public sealed class PlayerController : MonoBehaviour
{
    public float playerSpeed = 1f;
    public float cameraSpeed = 1f;
    public Transform handLeft;

    private Rigidbody2D rb;
    private Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>() ?? throw new NullReferenceException();
        animator = GetComponent<Animator>() ?? throw new NullReferenceException();

        if (handLeft == null)
            throw new NullReferenceException();

        var camera = Camera.main;
        camera.transparencySortMode = TransparencySortMode.CustomAxis;
        camera.transparencySortAxis = new Vector3(0.0f, 1.0f, 0.0f);
    }

    private void FixedUpdate()
    {
        UpdateMovement();
        UpdateCameraFollow();
        UpdateDirection();
        UpdateHandRotation();
        UpdateWalkAnimation();
    }

    private void UpdateMovement()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        if (horizontal * vertical == 0)
            rb.velocity = new Vector2(horizontal * playerSpeed, vertical * playerSpeed) * (Input.GetKey(KeyCode.LeftShift) ? 2f : 1f);
        else
            rb.velocity = new Vector2(horizontal * (playerSpeed / 1.414f), vertical * (playerSpeed / 1.414f)) * (Input.GetKey(KeyCode.LeftShift) ? 2f : 1f);
    }

    private void UpdateCameraFollow()
    {
        var cameraTransform = Camera.main.transform;
        var newPosition = Vector3.Lerp(cameraTransform.position, transform.position, cameraSpeed);
        newPosition.z = -1f;
        cameraTransform.position = newPosition;
    }

    private void UpdateDirection()
    {
        var mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (transform.position.x > mouseWorldPosition.x)
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        else
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
    }

    private void UpdateHandRotation()
    {
        var mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var relative = handLeft.InverseTransformPoint(mouseWorldPosition);
        var angle = Mathf.Atan2(relative.x, relative.y) * Mathf.Rad2Deg;
        handLeft.Rotate(0, 0, -angle);
    }

    private void UpdateWalkAnimation()
    {
        if (rb.velocity.magnitude > 0f)
            animator.SetBool("IsWalking", true);
        else
            animator.SetBool("IsWalking", false);
    }
}
