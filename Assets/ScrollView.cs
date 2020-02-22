using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace InjectorGames.FarAlone.UI
{
    public class ScrollView : MonoBehaviour, IDragHandler
    {
        public void OnDrag(PointerEventData eventData)
        {
            transform.position = Input.mousePosition;
        }

    }
}
