using InjectorGames.FarAlone.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InjectorGames.FarAlone.UI
{
    public class RepairSlot : Slot
    {
        [SerializeField]
        protected bool isRepaired;
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
    }
}
