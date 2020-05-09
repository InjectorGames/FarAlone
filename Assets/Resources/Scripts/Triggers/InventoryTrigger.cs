using InjectorGames.FarAlone.Controllers;
using InjectorGames.FarAlone.Items;
using InjectorGames.FarAlone.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: change namespace
namespace InjectorGames.FarAlone.Inventory
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class InventoryTrigger : MonoBehaviour
    {
        [SerializeField]
        private GameObject Weapon;

        private void OnTriggerStay2D(Collider2D collider)
        {
            UpdateItemPickUp(collider);
        }

        private void UpdateItemPickUp(Collider2D collider)
        {
            if (collider.CompareTag("Item") && Input.GetAxis("Use") == 1.0f && InventoryController.Instance.TryAddItem(collider.gameObject.name))
            {
                if(collider.gameObject.name == "Blaster")
                    Weapon.SetActive(true);
                Destroy(collider.gameObject);
                InfoWindow.Instance.ShowMessage($"{collider.gameObject.name} added");
            }
        }
    }
}
