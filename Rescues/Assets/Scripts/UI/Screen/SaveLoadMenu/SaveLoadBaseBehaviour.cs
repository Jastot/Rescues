using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rescues
{
    public sealed class SaveLoadBaseBehaviour: MonoBehaviour, IPointerDownHandler
    {
        #region Fields

        public InputField InputField;
        [NonSerialized] public InputField.SubmitEvent se = new InputField.SubmitEvent();

        #endregion

        
        #region UnityMethods

        public void OnPointerDown(PointerEventData eventData)
        {
            var input = gameObject.GetComponent<InputField>();
            input.onEndEdit = se;
        }

        #endregion
    }
}