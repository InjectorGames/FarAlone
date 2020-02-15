using InjectorGames.FarAlone.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InjectorGames.FarAlone.UI
{
    public class RepairWindow : Window
    {
        public Transform content;
        public GameObject slotPrefab;

        public Transform target;
        public VisualRepair visual;
        public float offset = 175f;

        public string[] repairItems;
        // TODO: Create RepairSlot class

        public List<RepairSlot> Slots { get; private set; } = new List<RepairSlot>();

        private void Start()
        {
            var inventory = InventoryWindow.Instance;

            for (int i = 0; i < repairItems.Length; i++)
            {
                if (!inventory.TryGetItemInfo(repairItems[i], out ItemInfo info))
                    throw new Exception($"Unknown item name at {i}");

                var slot = Instantiate(slotPrefab, content).GetComponent<RepairSlot>();
                slot.ItemInfo = info;
                slot.OnRepairEvent = OnRepair;
                Slots.Add(slot);
            }
        }

        private void Update()
        {
            UpdateActive();
            UpdateWindowPosition();
        }

        private void UpdateActive()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (IsShowing())
                {
                    Hide();
                }
                else
                {
                    Show();
                }
            }
        }

        private void UpdateWindowPosition()
        {
            var viewportPosition = Camera.main.WorldToViewportPoint(target.position);
            var canvas = RepairWindowManager.Instance.canvas;

            var position = new Vector2(
                (viewportPosition.x * canvas.sizeDelta.x) - (canvas.sizeDelta.x * 0.5f),
                (viewportPosition.y * canvas.sizeDelta.y) - (canvas.sizeDelta.y * 0.5f) - offset);

            ((RectTransform)view.transform).anchoredPosition = position;
        }

        private void OnRepair(RepairSlot slot)
        {
            var inventory = InventoryWindow.Instance;

            if (inventory.TakedItem != slot.ItemInfo)
                return;

            inventory.OnSlotRelease();

            if (inventory.TryRemoveSlot(slot.ItemInfo))
                slot.IsRepaired = true;

            var percent = GetRepairedPercent();
            visual.OnRepair(percent);
        }

        public int GetRepairedCount()
        {
            var count = 0;

            for (int i = 0; i < Slots.Count; i++)
            {
                if (Slots[i].IsRepaired)
                    count++;
            }

            return count;
        }

        public float GetRepairedPercent()
        {
            return GetRepairedCount() / (float)Slots.Count;
        }
    }
}
