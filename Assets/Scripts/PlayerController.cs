using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Rendering;
using UnityEngine.UI;

using Reserve; //namespace with Inventory class
using Items;
using PlayerText;

[RequireComponent(typeof(Rigidbody2D))]
public sealed class PlayerController : MonoBehaviour
{
    public Inventory inventory;
    public float playerSpeed = 1f;
    public float cameraSpeed = 1f;

    private Rigidbody2D rb;

    public PlayerController()
    {
        inventory = new Inventory(InventoryType.Small);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>() ?? throw new NullReferenceException();

        Camera.main.transparencySortMode = TransparencySortMode.CustomAxis;
        Camera.main.transparencySortAxis = new Vector3(0.0f, 1.0f, 0.0f);
    }

    private void FixedUpdate()
    {
        UpdateMovement();
        UpdateCameraFollow();
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
            if(Input.GetKeyDown(KeyCode.E) && this.inventory.IsFull())
            {
                Destroy(collider.gameObject);
                this.inventory.Add_Element(collider.gameObject.GetComponent<Item>());
                Debug.Log(this.inventory.elements.Count);
            }
        }
    }

}
