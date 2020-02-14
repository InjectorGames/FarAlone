using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace InjectorGames.FarAlone.Inventory
{
    public class Item : MonoBehaviour
    {
        /// <summary>
        /// Type of the item
        /// </summary>
        public ItemType type;
        /// <summary>
        /// The weight of the item in the inventory
        /// </summary>
        public float weight;
    }
}
