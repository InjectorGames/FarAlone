using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InjectorGames.FarAlone.UI
{
    public class ShipRepairWindow : MonoBehaviour
    {
        public static ShipRepairWindow Instance;

        public Transform ship;
        public RectTransform canvas;
        public RectTransform repairWindow;

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            UpdateWindowPosition();
        }

        private void UpdateWindowPosition()
        {
            Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(ship.position);
            Vector2 WorldObject_ScreenPosition = new Vector2((ViewportPosition.x * canvas.sizeDelta.x) - (canvas.sizeDelta.x * 0.5f), (ViewportPosition.y * canvas.sizeDelta.y) - (canvas.sizeDelta.y * 0.5f));
            repairWindow.anchoredPosition = WorldObject_ScreenPosition;
        }
    }
}
