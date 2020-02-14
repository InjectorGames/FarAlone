using InjectorGames.FarAlone.Items;
using InjectorGames.FarAlone.Players;
using InjectorGames.FarAlone.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InjectorGames.FarAlone.Inventory
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class InventoryTrigger : MonoBehaviour
    {
        private void OnTriggerStay2D(Collider2D collider)
        {
            UpdateItemPickUp(collider);
        }

        private void UpdateItemPickUp(Collider2D collider)
        {
            if (collider.tag == "Item" && Input.GetAxis("Use") == 1.0f && InventoryWindow.Instance.TryAddSlot(collider.gameObject.name))
            {
                Destroy(collider.gameObject);
                InfoWindow.Instance.ShowMessage($"{collider.gameObject.name} added");

               // while (windowContent.transform.childCount > 0)
               //     Destroy(windowContent.transform.GetChild(0).gameObject);
            }
        }
    }
}
