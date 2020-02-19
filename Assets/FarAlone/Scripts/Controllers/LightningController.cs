using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

using Random = UnityEngine.Random;

namespace InjectorGames.FarAlone.Controllers
{
    public sealed class LightningController : MonoBehaviour
    {
        #region Singletone
        public static LightningController Instance;

        private void SetInstance()
        {
            if (Instance != null)
                throw new Exception("Multiple instances of this class is not supported");
            Instance = this;
        }
        #endregion

        [SerializeField]
        private Light2D globalLight;

        // TODO: move constant values to settings
        private float waitDelay = 5;
        private float emitDelay = 0;

        private void Awake()
        {
            SetInstance();
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
