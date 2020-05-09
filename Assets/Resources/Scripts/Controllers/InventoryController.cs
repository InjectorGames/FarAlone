using InjectorGames.FarAlone.Items;
using InjectorGames.FarAlone.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace InjectorGames.FarAlone.Controllers
{
    public sealed class InventoryController : MonoBehaviour
    {
        #region Singletone
        public static InventoryController Instance;

        private void SetInstance()
        {
            if (Instance != null)
                throw new Exception("Multiple instances of this class is not supported");
            Instance = this;
        }
        #endregion

        [Header("Invenotory")]
        [SerializeField]
        private int currentWeight = 0;
        public int CurrentWeight => currentWeight;

        [SerializeField]
        private int maxWeight = 10;
        public int MaxWeight => maxWeight;

        [SerializeField]
        private List<Item> items = new List<Item>();

        private void Awake()
        {
            SetInstance();
        }

        public bool CanStore(int weight)
        {
            return currentWeight + weight <= maxWeight;
        }

        public bool TryAddItem(Item item)
        {
            if (!CanStore(item.Weight))
                return false;

            items.Add(item);
            currentWeight += item.Weight;
            InventoryWindow.Instance.AddSlot(item);
            return true;
        }
        public bool TryAddItem(int id)
        {
            if (ItemController.Instance.TryGetItem(id, out Item item))
                return TryAddItem(item);
            else
                return false;
        }
        public bool TryAddItem(string name)
        {
            if (ItemController.Instance.TryGetItem(name, out Item item))
                return TryAddItem(item);
            else
                return false;
        }

        public bool TryRemoveItem(Item item)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (item != items[i])
                    continue;

                items.RemoveAt(i);
                currentWeight -= item.Weight;
                InventoryWindow.Instance.RemoveSlot(item);
                return true;
            }

            return false;
        }
        public bool TryRemoveItem(int id)
        {
            if (ItemController.Instance.TryGetItem(id, out Item item))
                return TryRemoveItem(item);
            else
                return false;
        }
        public bool TryRemoveItem(string name)
        {
            if (ItemController.Instance.TryGetItem(name, out Item item))
                return TryRemoveItem(item);
            else
                return false;
        }
    }
}
