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
        [SerializeField]
        protected Image itemImage;
        public Image ItemImage => itemImage;

        protected Item item;
        public Item Item
        {
            get
            {
                return item;
            }
            set
            {
                item = value;
                itemImage.sprite = item.Sprite;
            }
        }

        public Action<Slot> OnPressEvent { get; set; }

        private void Awake()
        {
            if (itemImage == null)
                throw new NullReferenceException();
        }

        public void OnPress()
        {
            OnPressEvent?.Invoke(this);
        }
    }
}
