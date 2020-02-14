﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

using Random = UnityEngine.Random;

namespace InjectorGames.FarAlone
{
    [RequireComponent(typeof(Light2D))]
    public class LightningController : MonoBehaviour
    {
        private Light2D globalLight;

        private float waitDelay = 5;
        private float emitDelay = 0;

        private void Start()
        {
            globalLight = GetComponent<Light2D>() ?? throw new NullReferenceException();
        }

        private void Update()
        {
            UpdateLightning();
        }

        private void UpdateLightning()
        {
            if (waitDelay < 0f)
            {
                if (Random.Range(0, 10) == 0)
                    waitDelay += Random.Range(0.05f, 0.25f);
                else
                    waitDelay += Random.Range(5, 30);

                emitDelay = Random.Range(0.05f, 0.25f);
                globalLight.intensity = 1.0f;
            }

            if (emitDelay < 0f)
            {
                emitDelay = float.MaxValue;
                globalLight.intensity = 0.2f;
            }

            waitDelay -= Time.deltaTime;
            emitDelay -= Time.deltaTime;
        }
    }
}
