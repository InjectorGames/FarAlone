using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InjectorGames.FarAlone.UI
{
    public class RepairWindow : Window
    {
        public Transform target;
        public float offset = 175f;

        // TODO: add list of repair elements
        // Create RepairSlot class

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
    }
}
