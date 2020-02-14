using InjectorGames.FarAlone.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InjectorGames.FarAlone.UI
{
    [RequireComponent(typeof(Button))]
    public class Slot : MonoBehaviour
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

        public Action<Slot> OnTakeEvent { get; set; }

        private void Awake()
        {
            if (itemImage == null)
                throw new NullReferenceException();
        }

        public void OnTake()
        {
            OnTakeEvent?.Invoke(this);
        }
    }
}
