using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Items
{
    public enum ItemType
    {
        Null,
        Weapon,
        Detail,
    }

    public abstract class Item : MonoBehaviour
    {
        /// <summary>
        /// The weight of the item in the inventory
        /// </summary>
        public float weight;

        /// <summary>
        /// Item type
        /// </summary>
        public abstract ItemType Type { get; }
    }
}
