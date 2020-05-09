using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InjectorGames.FarAlone.Repairs
{
    public class VisualRepair : MonoBehaviour
    {
        [Header("Repair")]
        [SerializeField]
        private float percent;
        public float Percent => percent;

        public virtual void OnRepair(float percent)
        {
            this.percent = percent;
        }
    }
}
