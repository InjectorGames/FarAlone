using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace InjectorGames.FarAlone.UI
{
    public sealed class MenuWindow : Window
    {
        #region Singletone
        public static MenuWindow Instance;

        private void SetInstance()
        {
            if (Instance != null)
                throw new Exception("Multiple instances of this class is not supported");
            Instance = this;
        }
        #endregion

        [Header("Menu")]
        [SerializeField]
        private GameObject game;
        [SerializeField]
        private GameObject menu;
        [SerializeField]
        private GameObject deathWindow;

        private void Awake()
        {
            SetInstance();

            // TEMPORARY
            if(PlayerPrefs.GetInt("IsDead", 0) == 1)
            {
                PlayerPrefs.SetInt("IsDead", 1);
                deathWindow.SetActive(true);
            }
        }

        public void OnStart()
        {
            menu.SetActive(false);
            game.SetActive(true);
        }
        public void OnOptions()
        {

        }
        public void OnExit()
        {
            Application.Quit(0);
        }
    }
}
