using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InjectorGames.FarAlone
{
    public class VisualRepair : MonoBehaviour
    {
        public float Percent { get; protected set; }

        public virtual void OnRepair(float percent)
        {
            Percent = percent;
        }
    }
}
