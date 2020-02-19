using System;
using UnityEngine;

namespace InjectorGames.FarAlone.Items
{
    [Serializable]
    public class WeaponItem : Item
    {
        [SerializeField]
        protected float damage;
        public float Damage => damage;

        [SerializeField]
        protected float useDelay;
        public float UseDelay => damage;
    }
}
