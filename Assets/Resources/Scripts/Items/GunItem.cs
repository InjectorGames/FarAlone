using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace InjectorGames.FarAlone.Items
{
    [Serializable]
    public class GunItem : WeaponItem
    {
        [SerializeField]
        protected GameObject bulletPrefab;
        public GameObject BulletPrefab => bulletPrefab;
    }
}
