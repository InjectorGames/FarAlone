using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InjectorGames.FarAlone.UI
{
    public sealed class RepairWindowManager : MonoBehaviour
    {
        #region Singletone
        public static RepairWindowManager Instance;

        private void SetInstance()
        {
            if (Instance != null)
                throw new Exception("Multiple instances of this class is not supported");
            Instance = this;
        }
        #endregion

        public RectTransform canvas;
        public RepairWindow ship;

        private void Awake()
        {
            SetInstance();
        }
    }
}