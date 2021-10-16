using System.Collections.Generic;
using UnityEngine;


namespace Rescues
{
    public class PlayerActivator : MonoBehaviour
    {
        #region Properties

        public OntriggerEvent CurrentTrigger { get; private set; }
        public List<OntriggerEvent> Triggers { get; private set; }

        #endregion


        #region UnityMethods

        private void Awake()
        {
            Triggers = new List<OntriggerEvent>(2);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var enteredObject = collision.GetComponent<OntriggerEvent>();
            CurrentTrigger = enteredObject;
            if (CurrentTrigger != null)
            {
                Triggers.Add(CurrentTrigger);
                CurrentTrigger.ActivateTriggerEnterEvent(); 
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            var exitedObject = collision.GetComponent<OntriggerEvent>();
            Triggers.Remove(exitedObject);
            if (CurrentTrigger == exitedObject && CurrentTrigger != null)
            {
                CurrentTrigger.ActivateTriggerExitEvent();
                CurrentTrigger = null;
            }
        }

        #endregion
    }
}
