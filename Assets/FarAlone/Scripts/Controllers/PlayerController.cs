﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Rendering;
using UnityEngine.UI;
using InjectorGames.FarAlone.UI;

namespace InjectorGames.FarAlone.Players
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
    public sealed class PlayerController : MonoBehaviour
    {
        #region Singletone
        public static PlayerController Instance;

        private void SetInstance()
        {
            if (Instance != null)
                throw new Exception("Multiple instances of this class is not supported");
            Instance = this;
        }
        #endregion

        public float HP;
        
        /* Movement & stamina variables */
        public float walkingSpeed;
        public float runningSpeed;

        private float currentSpeed;


        public float stamina {get; private set;}
        private float maxStamina = 100f;

        private const float staminaLose = 5f;
        private const float staminaRest = 1f;
        private float restDelay;
        private float runDelay;

        public bool canRun = true;

        /* end */

        public float cameraSpeed = 1f;
        public Transform handLeft;
        public Transform blastSpawnPoint;
        public GameObject blastPreafab;

        private float shootDelay;
        private Rigidbody2D rb;
        private Animator animator;
        private Transform HeBarChild;

        private void Awake()
        {
            SetInstance();
        }

        private void Start()
        {
            stamina = maxStamina; //Movement
            /* Delays */
            shootDelay = 0f;
            restDelay = 0f;
            runDelay = 0f;
            /* End */
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
            // TODO: open pause menu instead
            if (Input.GetKeyDown(KeyCode.Escape))
                Application.Quit(0);

            UpdateShooting();
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

            runDelay -= Time.deltaTime;
            restDelay -= Time.deltaTime;

            if (Input.GetKey(KeyCode.LeftShift) && canRun && stamina > 5) {
                currentSpeed = runningSpeed;
                if(runDelay < 0f)
                {
                    runDelay = 0.5f;
                    stamina -= staminaLose;
                }

                if(stamina <= 0)
                {
                    canRun = false;
                }
            }
            else
            {
                currentSpeed = walkingSpeed;

                if(stamina < 0)
                    stamina = 0;

                if(stamina < maxStamina && stamina >= 0 && restDelay < 0f)
                {
                    restDelay = 0.7f;
                    stamina += staminaRest;
                    canRun = true;
                }
            }


            if (horizontal * vertical == 0)
                rb.velocity = new Vector2(horizontal * currentSpeed, vertical * currentSpeed);
            else
                rb.velocity = new Vector2(horizontal * (currentSpeed / 1.414f), vertical * (currentSpeed / 1.414f));
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
            {
                for(int i = 0; i < transform.childCount; i++)
                {
                    if(transform.GetChild(i).tag != "Status Bar")
                    {
                        transform.eulerAngles = new Vector3(0f, 0f, 0f);
                    }
                }
            }
            else
            {
                for(int i = 0; i < transform.childCount; i++)
                {
                    if(transform.GetChild(i).tag != "Status Bar")
                    {
                        transform.eulerAngles = new Vector3(0f, 180f, 0f);
                    }
                }
            }
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

                if (currentSpeed == runningSpeed)
                    animator.speed = runningSpeed;
                else
                    animator.speed = walkingSpeed;
            }
            else
            {
                animator.SetBool("IsWalking", false);
            }
        }

        private void UpdateShooting()
        {
            shootDelay -= Time.deltaTime;

            if (shootDelay < 0f && Input.GetAxis("Fire1") == 1.0f)
            {
                shootDelay = 0.25f;

                // TODO: move shooting offset to weapon specification
                var shootPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var shootDistance = Vector3.Distance(blastSpawnPoint.position, shootPosition);
                var offset = new Vector3(UnityEngine.Random.Range(-shootDistance, shootDistance) / 20f, UnityEngine.Random.Range(-shootDistance, shootDistance) / 20f);
                var direction = shootPosition + offset - blastSpawnPoint.position;

                if (direction.magnitude > 1.414f)
                {
                    var blast = Instantiate(blastPreafab);
                    var blastTransform = blast.transform;
                    blastTransform.position = blastSpawnPoint.position;
                    blastTransform.rotation = blastSpawnPoint.rotation;

                    var blastRigidbody = blast.GetComponent<Rigidbody2D>();
                    blastRigidbody.velocity = direction.normalized * 10f;

                    Destroy(blast, 5f);
                }
            }
        }
    }
}