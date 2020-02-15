using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace InjectorGames.FarAlone
{
    public class ShipVisualRepair : VisualRepair
    {
        public Light2D windowLight;

        private float lightIntensity = 0f;
        private float intensityDelay = float.PositiveInfinity;

        private void Update()
        {
            UpdateWindowLight();
        }

        public override void OnRepair(float percent)
        {
            base.OnRepair(percent);

            Debug.Log(percent);

            if(percent >= 1f)
            {
                lightIntensity = 1f;
                intensityDelay = float.PositiveInfinity;
            }
            else if(percent >= 0.25f)
            {
                lightIntensity = 0.5f;
                intensityDelay = 1;
            }
        }

        private void UpdateWindowLight()
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
