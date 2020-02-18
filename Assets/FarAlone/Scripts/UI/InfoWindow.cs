using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace InjectorGames.FarAlone.UI
{
    public sealed class InfoWindow : Window
    {
        #region Singletone
        public static InfoWindow Instance;

        private void SetInstance()
        {
            if (Instance != null)
                throw new Exception("Multiple instances of this class is not supported");
            Instance = this;
        }
        #endregion

        [Header("Information")]
        [SerializeField]
        private Text infoText;
        [SerializeField]
        private float fadeTime = 3.0f;

        [Header("Health")]
        [SerializeField]
        private Slider healthBar;
        [SerializeField]
        private float health = 0f;
        public float Health
        {
            get
            {
                return health;
            }
            set
            {
                health = value;
                healthBar.value = value / 100f;
            }
        }

        [Header("Stamina")]
        [SerializeField]
        private Slider staminaBar;
        [SerializeField]
        private float stamina = 0f;
        public float Stamine
        {
            get
            {
                return stamina;
            }
            set
            {
                stamina = value;
                staminaBar.value = value / 100f;
            }
        }

        private void Awake()
        {
            SetInstance();
        }

        public void ShowMessage(string message)
        {
            infoText.text = message;
            StartCoroutine(Animate(infoText, fadeTime));
        }

        private IEnumerator Animate(Text text, float time)
        {
            yield return FadeIn(text, time / 2);
            yield return new WaitForSeconds(time);
            yield return FadeOut(text, time / 2);
        }
        private IEnumerator FadeIn(Text text, float time)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, 0);

            while (text.color.a < 1.0f)
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + Time.deltaTime / time);
                yield return null;
            }
        }
        private IEnumerator FadeOut(Text text, float time)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, 1);

            while (text.color.a > 0.0f)
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - Time.deltaTime / time);
                yield return null;
            }
        }
    }
}