using InjectorGames.FarAlone.Controllers;
using InjectorGames.FarAlone.Items;
using InjectorGames.FarAlone.Repairs;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InjectorGames.FarAlone.UI
{
    public class RepairWindow : Window
    {
        [SerializeField]
        protected Transform content;
        [SerializeField]
        protected GameObject slotPrefab;

        [Header("Repair Target")]
        [SerializeField]
        protected Transform target;
        [SerializeField]
        protected VisualRepair visual;
        [SerializeField]
        protected float offset;
        [SerializeField]
        protected string[] repairItems;

        public List<RepairSlot> Slots { get; private set; } = new List<RepairSlot>();

        private void Start()
        {
            var itemController = ItemController.Instance;

            for (int i = 0; i < repairItems.Length; i++)
            {
                if (!itemController.TryGetItem(repairItems[i], out Item item))
                    throw new Exception($"Unknown item name at {i}");

                var slot = Instantiate(slotPrefab, content).GetComponent<RepairSlot>();
                slot.Item = item;
                slot.OnPressEvent = OnRepair;
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
            var canvas = RepairWindowManager.Instance.Canvas;

            var position = new Vector2(
                (viewportPosition.x * canvas.sizeDelta.x) - (canvas.sizeDelta.x * 0.5f),
                (viewportPosition.y * canvas.sizeDelta.y) - (canvas.sizeDelta.y * 0.5f) - offset);

            ((RectTransform)view.transform).anchoredPosition = position;
        }

        private void OnRepair(Slot slot)
        {
            var repairSlot = (RepairSlot)slot;
            var inventory = InventoryWindow.Instance;

            if (repairSlot.IsRepaired || inventory.TakedItem != repairSlot.Item || !inventory.TryUseTakedItem())
                return;

            repairSlot.IsRepaired = true;

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
