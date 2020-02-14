using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InjectorGames.FarAlone.Inventory
{
    /// <summary>
    /// Weapon item class
    /// </summary>
    public class ItemWeapon : Item
    {
        /// <summary>
        /// Weapon deal damage
        /// </summary>
        public float Damage => damage;

        /// <summary>
        /// Weapon deal damage
        /// </summary>
        [SerializeField]
        protected float damage;
    }
}
