using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Items
{
    public class ItemWeapon : Item
    {
        /// <summary>
        /// Item type
        /// </summary>
        public override ItemType Type => ItemType.Weapon;
        /// <summary>
        /// Weapon deal damage
        /// </summary>
        public float damage;
    }
}
