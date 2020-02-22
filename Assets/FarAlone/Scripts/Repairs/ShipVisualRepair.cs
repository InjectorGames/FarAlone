using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.SceneManagement;

namespace InjectorGames.FarAlone.Repairs
{
    public class ShipVisualRepair : VisualRepair
    {
        [Header("Ship Repair")]
        [SerializeField]
        protected Light2D windowLight;
        [SerializeField]
        protected float lightIntensity = 0f;
        [SerializeField]
        protected float intensityDelay = float.PositiveInfinity;

        private void Update()
        {
            UpdateWindowLight();
        }

        public override void OnRepair(float percent)
        {
            base.OnRepair(percent);

            if(percent >= 1f)
            {
                lightIntensity = 1f;
                intensityDelay = float.PositiveInfinity;
                PlayerPrefs.SetInt("IsDead", 2);
                SceneManager.LoadScene(0);
            }
            else if(percent >= 0.25f)
            {
                lightIntensity = 0.5f;
                intensityDelay = 1;
            }
        }

        protected void UpdateWindowLight()
        {
            windowLight.intensity = Mathf.Lerp(windowLight.intensity, lightIntensity, Time.deltaTime * 2f);

            if(intensityDelay < 0f)
            {
                lightIntensity = Random.Range(0.25f, 0.75f);
                intensityDelay = Random.Range(0f, 1f);
            }

            intensityDelay -= Time.deltaTime;
        }
    }
}
