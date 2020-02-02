using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public enum ItemDetailType
    {
        Null,
        Cap,
        Gear,
        Grate,
        Motor,
        Ski,
    }

    public class ItemDetail : Item
    {
        /// <summary>
        /// Item type
        /// </summary>
        public override ItemType Type => ItemType.Detail;
        /// <summary>
        /// Item detail type
        /// </summary>
        public ItemDetailType detailType;
    }
}
