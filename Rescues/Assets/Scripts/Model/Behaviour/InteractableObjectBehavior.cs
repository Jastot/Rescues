using System;
using UnityEngine;


namespace Rescues
{
    public class InteractableObjectBehavior : MonoBehaviour, ITrigger
    {
        #region Fields
       
        [SerializeField] private InteractableObjectType _type;

        #endregion


        #region Properties

        public Predicate<Collider2D> OnFilterHandler { get; set; }
        public Action<ITrigger> OnTriggerEnterHandler { get; set; }
        public Action<ITrigger> OnTriggerExitHandler { get; set; }
        public Action<ITrigger, InteractableObjectType> DestroyHandler { get; set; }
        public GameObject GameObject => gameObject;
        public InteractableObjectType Type { get => _type; }
        public bool IsInteractable { get; set; }       
        [field: SerializeField] public bool IsInteractionLocked { get; set; }
        public string Description { get; set; }
        [field: SerializeField] public string Id { get; set; }

        #endregion


        #region UnityMethods

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (OnFilterHandler?.Invoke(other) == true && IsInteractionLocked == false)
            {
                OnTriggerEnterHandler.Invoke(this);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (OnFilterHandler?.Invoke(other) == true && IsInteractionLocked == false)
            {
                OnTriggerExitHandler.Invoke(this);
            }
        }

        private void OnDisable()
        {
            DestroyHandler?.Invoke(this, _type);
        }

        #endregion
    }
}
