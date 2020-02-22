using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;
using InjectorGames.FarAlone.UI;

namespace InjectorGames.FarAlone.Controllers
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

        [Header("Information")]
        [SerializeField]
        private float health; // TODO: use health interval as 1.0f
        private const float maxHealth = 100f;
        public float Healt
        {
            get
            {
                return health;
            }
            set
            {
                health = value;
                // TODO: check for death
            }
        }

        [SerializeField]
        private float stamina;
        private const float maxStamina = 100f;
        public float Stamina => Stamina;

        private const float staminaLose = 8.3f;
        private const float staminaRest = 3.9f;

        [SerializeField]
        private float light; //light from flashLight
        private float batteryPower = 0.5f; //TODO: create new objects of batteries which will affect to batteryPower
        private const float maxLight = 100f;

        [SerializeField]
        private bool lightStatus = false;
        public bool LightStatus => lightStatus;

        

        [Header("Movement")]
        [SerializeField]
        private float walkingSpeed = 2.5f;
        [SerializeField]
        private float runningSpeed = 3.25f;
        [SerializeField]
        private float currentSpeed;

        [SerializeField]
        private bool canRun = true;
        public bool CanRun => canRun;

        [Header("Control")]
        [SerializeField]
        private float cameraSpeed = 1f;
        [SerializeField]
        private Transform handLeft;
        [SerializeField]
        private GameObject Blaster;
        [SerializeField]
        private GameObject FlashLight;
        [SerializeField]
        private Rigidbody2D rb;
        [SerializeField]
        private Animator animator;

        [SerializeField]
        public Transform blastSpawnPoint; // TODO: move this to weapon item class
        [SerializeField]
        public GameObject blastPreafab;
        [SerializeField]
        private float shootDelay = 0f;

        private void Awake()
        {
            SetInstance();
        }
        private void Start()
        {
            stamina = maxStamina;
            health = maxHealth;
            currentSpeed = walkingSpeed;
            if(Blaster.activeSelf)
            {
                light = FlashLight.GetComponent<Light2D>().intensity * 100f;
                if(light <= 0)
                    lightStatus = false;
                if(lightStatus)
                {
                    FlashLight.SetActive(true);
                }
                else
                {
                    FlashLight.SetActive(false);
                }
            }

            var camera = Camera.main;
            camera.transparencySortMode = TransparencySortMode.CustomAxis;
            camera.transparencySortAxis = new Vector3(0.0f, 1.0f, 0.0f);
        }
        private void Update()
        {
            // TODO: open pause menu instead
            if (Input.GetKeyDown(KeyCode.Escape))
                Application.Quit(0);
            if (health <= 0)
                Destroy(this.gameObject); //TODO: Game over

            if(Blaster.activeSelf)
            {
                UpdateShooting();
                UpdateFlashlight();
            }

        }
        private void FixedUpdate()
        {
            UpdateInfo();
            UpdateMovement();
            UpdateCameraFollow();
            UpdateDirection();
            UpdateHandRotation();
            UpdateWalkAnimation();
        }

        private void UpdateInfo()
        {
            var infoWindow = InfoWindow.Instance;

            infoWindow.Health = health;
            infoWindow.Stamine = stamina;
            infoWindow.Light = light;
            if(Blaster.activeSelf)
            {
                infoWindow.flashLightBar.gameObject.SetActive(true);
            }

        }
        private void UpdateMovement()
        {
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");

            if (Input.GetKey(KeyCode.LeftShift) && canRun && stamina > 0) {
                currentSpeed = runningSpeed;
               
                stamina -= Time.deltaTime * staminaLose;

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
                if(stamina > maxStamina)
                {
                    stamina = maxStamina;
                }
                

                if(stamina < maxStamina && stamina >= 0 )
                {
                    stamina += Time.deltaTime * staminaRest;
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

        private void UpdateFlashlight()
        {
            if(light <= 0)
            {
                lightStatus = false;
            }

            light = FlashLight.GetComponent<Light2D>().intensity * 100f;

            if(lightStatus)
            {
                light -= Time.deltaTime * batteryPower;
                FlashLight.GetComponent<Light2D>().intensity = light / 100f;
            }

            if(lightStatus && Input.GetKey(KeyCode.F))
            {
                lightStatus = false;
                FlashLight.SetActive(false);
                return;
            }
            else if(lightStatus == false && Input.GetKey(KeyCode.F) && light > 0)
            {
                lightStatus = true;
                FlashLight.SetActive(true);
                return;
            }


        }
    }
}
