using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Rendering;
using UnityEngine.UI;

using Reserve; // namespace with Inventory class
using Items;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public sealed class PlayerController : MonoBehaviour
{
    public Inventory inventory;
    public float playerSpeed = 1f;
    public float cameraSpeed = 1f;
    public Transform handLeft;

    private Rigidbody2D rb;
    private Animator animator;

    public PlayerController()
    {
        inventory = new Inventory(InventoryType.Small);
    }

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit(0);
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


    private void OnTriggerStay2D(Collider2D collider)
    {
        if(collider.gameObject.GetComponent<Item>() as Item)
        {
            if  (
                Input.GetKeyDown(KeyCode.E) &&
                this.inventory.CanStore(collider.gameObject.GetComponent<Item>().weight)
                )
            {
                Destroy(collider.gameObject);
                this.inventory.Add_Element(collider.gameObject.GetComponent<Item>());
                Debug.Log("Elements: " + this.inventory.elements.Count);
                Debug.Log("Current weight: " + this.inventory.current_weight);
                // I can add the name of the object later
                InfoText.instance.ShowMessage("Object added");
            }
        }
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
        {
            animator.SetBool("IsWalking", true);

            if (Input.GetKey(KeyCode.LeftShift))
                animator.speed = 2f;
            else
                animator.speed = 1f;
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }
    }
}
