using System;
using UnityEngine;


namespace Rescues
{
    public abstract class InteractableObjectBehavior : MonoBehaviour, ITrigger
    {
        #region Fields

        /// <summary>
        /// Тип интерактивного объекта
        /// </summary>
        [SerializeField] private InteractableObjectType _type;

        /// <summary>
        /// Блокирует взаимодействие игрока с этим триггером
        /// </summary>
        [SerializeField] private bool _isInteractionLocked;

        #endregion


        #region Properties

        public Predicate<Collider2D> OnFilterHandler { get; set; }
        public Action<ITrigger> OnTriggerEnterHandler { get; set; }
        public Action<ITrigger> OnTriggerExitHandler { get; set; }
        public Action<ITrigger, InteractableObjectType> DestroyHandler { get; set; }
        public bool IsInteractable { get; set; }

        /// <summary>
        /// Блокирует взаимодействие игрока с этим триггером
        /// </summary>
        public bool IsInteractionLocked
        {
            get => _isInteractionLocked;
            set
            {
                _isInteractionLocked = value;
            }
        }

        public string Description { get; set; }
        public GameObject GameObject => gameObject;
        public InteractableObjectType Type { get => _type; }
        [field: SerializeField] public string Id { get; set; }

        #endregion


        #region UnityMethods

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (OnFilterHandler?.Invoke(other) == true && _isInteractionLocked == false)
            {
                OnTriggerEnterHandler.Invoke(this);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (OnFilterHandler?.Invoke(other) == true && _isInteractionLocked == false)
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
