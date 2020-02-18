using InjectorGames.FarAlone.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace InjectorGames.FarAlone.Controllers
{
    public sealed class ItemController : MonoBehaviour
    {
        #region Singletone
        public static ItemController Instance;

        private void SetInstance()
        {
            if (Instance != null)
                throw new Exception("Multiple instances of this class is not supported");
            Instance = this;
        }
        #endregion

        [SerializeField]
        private DetailItem[] detailItems;
        [SerializeField]
        private GunItem[] gunItems;

        private List<Item> items;
        private Dictionary<string, int> itemLinks;

        private void Awake()
        {
            SetInstance();
            InitializeItems();
        }

        private void InitializeItems()
        {
            items = new List<Item>();
            itemLinks = new Dictionary<string, int>();

            for (int i = 0; i < detailItems.Length; i++)
            {
                var item = detailItems[i];
                items.Add(item);
                itemLinks.Add(item.Name, i);
            }

            for (int i = 0; i < gunItems.Length; i++)
            {
                var item = gunItems[i];
                items.Add(item);
                itemLinks.Add(item.Name, i);
            }

            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                item.SetID(i);
                

#if UNITY_EDITOR
                if (string.IsNullOrEmpty(item.Name))
                    throw new Exception($"Item name can not be null or empty at {i}");
                if (item.Weight <= 0)
                    throw new Exception($"Item weight can not be less or equal zero at {i}");
                if (item.Sprite == null)
                    throw new Exception($"Item sprite can not be null at {i}");

                for (int j = 0; j < items.Count; j++)
                {
                    if (i != j && item.Name == items[j].Name)
                        throw new Exception($"More than one item at {i} and {j}");
                }
#endif
            }
        }

        public bool TryGetItem<T>(int id, out T item) where T : Item
        {
            try
            {
                item = (T)items[id];
                return true;
            }
            catch
            {
                item = null;
                return false;
            }
        }
        public bool TryGetItem<T>(string name, out T item) where T : Item
        {
            try
            {
                item = (T)items[itemLinks[name]];
                return true;
            }
            catch
            {
                item = null;
                return false;
            }
        }
    }
}
