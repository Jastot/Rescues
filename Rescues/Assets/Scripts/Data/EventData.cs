using System;
using UnityEngine;
using UnityEngine.Events;


namespace Rescues
{
    [Serializable]
    public struct EventData : IInteractable
    {
        #region Fields

        /// <summary>
        /// Is event repeat after end?
        /// </summary>
        public bool IsRepeating;
        [SerializeField] private bool isInteractionLocked;

        /// <summary>
        /// Time before invoke event
        /// </summary>
        public float TimeBeforeInvoke;
        [SerializeField] private string id;

        /// <summary>
        /// Event himself
        /// </summary>
        public UnityEvent Event;

        public bool IsInteractable { get; set; }
        public bool IsInteractionLocked { get => isInteractionLocked; set => isInteractionLocked = value; }
        public string Description { get; set; }
        public string Id { get => id; set => id = value; }

        #endregion
    }
}
