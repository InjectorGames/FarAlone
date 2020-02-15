using InjectorGames.FarAlone.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InjectorGames.FarAlone.UI
{
    public class RepairSlot : MonoBehaviour
    {
        public Image itemImage;

        private ItemInfo itemInfo;
        public ItemInfo ItemInfo
        {
            get
            {
                return itemInfo;
            }
            set
            {
                itemInfo = value;
                itemImage.sprite = itemInfo.sprite;
            }
        }

        private bool isRepaired;
        public bool IsRepaired
        {
            get
            {
                return isRepaired;
            }
            set
            {
                isRepaired = value;
                itemImage.color = value ? Color.white : Color.black;
            }
        }

        public Action<RepairSlot> OnRepairEvent { get; set; }

        public void OnRepair()
        {
            OnRepairEvent?.Invoke(this);
        }
    }
}
