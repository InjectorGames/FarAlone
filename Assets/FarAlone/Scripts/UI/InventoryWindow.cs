using InjectorGames.FarAlone.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InjectorGames.FarAlone.UI
{
    public sealed class InventoryWindow : Window
    {
        #region Singletone
        public static InventoryWindow Instance;

        private void SetInstance()
        {
            if (Instance != null)
                throw new Exception("Multiple instances of this class is not supported");
            Instance = this;
        }
        #endregion

        public Transform content;
        public GameObject takedSlot;
        public GameObject slotPrefab;

        public ItemInfo[] itemInfos;
        public Dictionary<string, int> ItemLinks { get; private set; } = new Dictionary<string, int>();

        public int CurrentWeight { get; private set; }
        public int MaxWeight { get; private set; }
        public ItemInfo TakedItem { get; private set; }
        public List<Slot> Slots { get; private set; } = new List<Slot>();

        private InventorySizeType size;
        public InventorySizeType Size
        {
            get
            {
                return size;
            }
            set
            {
                size = value;

                switch (value)
                {
                    case InventorySizeType.Small:
                        MaxWeight = 10;
                        break;
                    case InventorySizeType.Medium:
                        MaxWeight = 13;
                        break;
                    case InventorySizeType.Big:
                        MaxWeight = 17;
                        break;
                }
            }
        } 

        private void Awake()
        {
            SetInstance();
            InitalizeItems();
            Size = InventorySizeType.Small;
        }

        private void Update()
        {
            UpdateActive();
            UpdateTakedSlot();
        }

        private void InitalizeItems()
        {
            for (int i = 0; i < itemInfos.Length; i++)
            {
                var item = itemInfos[i];

#if UNITY_EDITOR
                if (string.IsNullOrEmpty(item.name))
                    throw new Exception($"Item name can not be null or empty at {i}");
                if (item.weight <= 0)
                    throw new Exception($"Item weight can not be less or equal zero at {i}");
                if (item.sprite == null)
                    throw new Exception($"Item sprite can not be null at {i}");

                for (int j = 0; j < itemInfos.Length; j++)
                {
                    if (i != j && item.name == itemInfos[j].name)
                        throw new Exception($"More than one item at {i} and {j}");
                }
#endif

                item.id = i;
                ItemLinks.Add(item.name, i);
            }
        }

        private void UpdateActive()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (IsShowing())
                {
                    Hide();
                    OnSlotRelease();
                }
                else
                {
                    Show();
                }
            }
        }

        private void UpdateTakedSlot()
        {
            if(TakedItem != null)
                takedSlot.transform.position = Input.mousePosition;
        }

        private void OnTake(Slot slot)
        {
            var info = slot.ItemInfo;
            if (TakedItem == null && TryRemoveSlot(info))
            {
                TakedItem = info;
                takedSlot.SetActive(true);
                takedSlot.GetComponent<Image>().sprite = TakedItem.sprite;
            }
        }

        public void OnSlotRelease()
        {
            if (TakedItem != null && TryAddSlot(TakedItem))
            {
                TakedItem = null;
                takedSlot.SetActive(false);
            }
        }

        public bool TryGetItemInfo(int id, out ItemInfo info)
        {
            try
            {
                info = itemInfos[id];
                return true;
            }
            catch
            {
                info = null;
                return false;
            }
        }
        public bool TryGetItemInfo(string name, out ItemInfo info)
        {
            try
            {
                info = itemInfos[ItemLinks[name]];
                return true;
            }
            catch
            {
                info = null;
                return false;
            }
        }

        public bool CanStore(int weight)
        {
            return CurrentWeight + weight <= MaxWeight;
        }

        public bool TryAddSlot(ItemInfo info)
        {
            if (!CanStore(info.weight))
                return false;

            var slot = Instantiate(slotPrefab, content).GetComponent<Slot>();
            slot.ItemInfo = info;
            slot.OnTakeEvent = OnTake;
            Slots.Add(slot);

            CurrentWeight += info.weight;
            return true;
        }
        public bool TryAddSlot(int id)
        {
            if (TryGetItemInfo(id, out ItemInfo info))
                return TryAddSlot(info);
            else
                return false;
        }
        public bool TryAddSlot(string name)
        {
            if (TryGetItemInfo(name, out ItemInfo info))
                return TryAddSlot(info);
            else
                return false;
        }

        public bool TryRemoveSlot(ItemInfo info)
        {
            for (int i = 0; i < Slots.Count; i++)
            {
                var slot = Slots[i];
                if (slot.ItemInfo != info)
                    continue;

                Destroy(slot.gameObject);
                CurrentWeight -= info.weight;
                Slots.RemoveAt(i);
                return true;
            }

            return false;
        }
        public bool TryRemoveSlot(int id)
        {
            if (TryGetItemInfo(id, out ItemInfo info))
                return TryRemoveSlot(info);
            else
                return false;
        }
        public bool TryRemoveSlot(string name)
        {
            if (TryGetItemInfo(name, out ItemInfo info))
                return TryRemoveSlot(info);
            else
                return false;
        }
    }
}