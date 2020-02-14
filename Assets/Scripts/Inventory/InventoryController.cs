using InjectorGames.FarAlone.Players;
using InjectorGames.FarAlone.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InjectorGames.FarAlone.Inventory
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class InventoryController : MonoBehaviour
    {
        public InventorySizeType size;
        public float currentWeight;
        public float maxWeight;
        public bool isOpened;

        public GameObject windowPanel;
        public GameObject windowContent;

        [SerializeField]
        public List<ItemType> items;

        private void Awake()
        {
            size = InventorySizeType.Small;
            items = new List<ItemType>();
            SetSizeType(InventorySizeType.Small);
        }

        private void Update()
        {
            UpdateWindowStatus();
        }

        private void UpdateWindowStatus()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (isOpened)
                {
                    isOpened = false;
                    windowPanel.SetActive(false);
                    PlayerController.Instance.isControllable = true;
                }
                else
                {
                    isOpened = true;
                    windowPanel.SetActive(true);
                    PlayerController.Instance.isControllable = false;
                }
            }
        }

        private void OnTriggerStay2D(Collider2D collider)
        {
            UpdateItemPickUp(collider);
        }

        private void UpdateItemPickUp(Collider2D collider)
        {
            var item = collider.gameObject.GetComponent<Item>();

            if (item != null && Input.GetAxis("Use") == 1.0f && AddItem(item))
            {
                Destroy(collider.gameObject);
                InfoWindow.Instance.ShowMessage($"{item.type} added");

               // while (windowContent.transform.childCount > 0)
               //     Destroy(windowContent.transform.GetChild(0).gameObject);
            }
        }

        public void SetSizeType(InventorySizeType size)
        {
            this.size = size;

            switch (size)
            {
                case InventorySizeType.Small:
                    maxWeight = 10.0f;
                    break;
                case InventorySizeType.Medium:
                    maxWeight = 13.0f;
                    break;
                case InventorySizeType.Big:
                    maxWeight = 17.0f;
                    break;
            }
        }

        public bool AddItem(Item item)
        {
            Debug.Log(Input.GetAxis("Use"));
            if (!CanStore(item.weight))
                return false;

            items.Add(item.type);
            currentWeight += item.weight;
            return true;
        }

        public bool CanStore(float weight)
        {
            return currentWeight + weight < maxWeight;
        }
    }
}
