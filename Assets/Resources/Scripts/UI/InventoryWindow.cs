using InjectorGames.FarAlone.Controllers;
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
        [SerializeField]
        public static InventoryWindow Instance;

        private void SetInstance()
        {
            if (Instance != null)
                throw new Exception("Multiple instances of this class is not supported");
            Instance = this;
        }
        #endregion

        [Header("Inventory")]
        [SerializeField]
        private Transform content;
        [SerializeField]
        private GameObject takedSlot;
        [SerializeField]
        private GameObject slotPrefab;

        [SerializeField]
        private List<Slot> slots = new List<Slot>();

        private Item takedItem;
        public Item TakedItem => takedItem;


        private void Awake()
        {
            SetInstance();
        }

        private void Update()
        {
            UpdateActive();
            UpdateTakedSlot();

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
            if(takedItem != null)
                takedSlot.transform.position = Input.mousePosition;
        }

        private void OnTake(Slot slot)
        {
            var item = slot.Item;

            if (!InventoryController.Instance.TryRemoveItem(item))
                return;

            takedItem = item;
            takedSlot.GetComponent<Image>().sprite = item.Sprite;
            takedSlot.SetActive(true);
        }

        public void OnSlotRelease()
        {
            if (takedItem == null || !InventoryController.Instance.TryAddItem(takedItem))
                return;

            takedSlot.SetActive(false);
            takedItem = null;
        }

        public void AddSlot(Item item)
        {
            var slot = Instantiate(slotPrefab, content).GetComponent<Slot>();
            slot.Item = item;
            slot.OnPressEvent = OnTake;
            slots.Add(slot);
        }

        public void RemoveSlot(Item item)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                var slot = slots[i];

                if (item != slot.Item)
                    continue;

                Destroy(slot.gameObject);
                slots.RemoveAt(i);
                return;
            }
        }

        public bool TryUseTakedItem()
        {
            if (takedItem == null)
                return false;

            takedSlot.SetActive(false);
            takedItem = null;
            return true;
        }
    }
}