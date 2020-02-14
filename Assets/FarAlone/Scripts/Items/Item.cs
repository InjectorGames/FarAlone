using InjectorGames.FarAlone.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InjectorGames.FarAlone.Items
{
    public class Item : MonoBehaviour
    {
        private void Start()
        {
            if (!InventoryWindow.Instance.TryGetItemInfo(name, out _))
                throw new Exception("Unknown item name");
        }
    }
}
