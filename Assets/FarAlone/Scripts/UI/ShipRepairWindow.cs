using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InjectorGames.FarAlone.UI
{
    public sealed class ShipRepairWindow : Window
    {
        #region Singletone
        public static ShipRepairWindow Instance;

        private void SetInstance()
        {
            if (Instance != null)
                throw new Exception("Multiple instances of this class is not supported");
            Instance = this;
        }
        #endregion

        public Transform ship;
        public RectTransform canvas;

        public float offset = 175f;

        private void Awake()
        {
            SetInstance();
            // TODO: add null reference check
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
            var viewportPosition = Camera.main.WorldToViewportPoint(ship.position);
            var position = new Vector2((viewportPosition.x * canvas.sizeDelta.x) - (canvas.sizeDelta.x * 0.5f), (viewportPosition.y * canvas.sizeDelta.y) - (canvas.sizeDelta.y * 0.5f) - offset);
            ((RectTransform)view.transform).anchoredPosition = position;
        }
    }
}
