using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InjectorGames.FarAlone.WorldObjects
{
    public class WorldObject : MonoBehaviour
    {
        [SerializeField]
        public float HP;
        [SerializeField]
        public bool destroyable;

    }
}
