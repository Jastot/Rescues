using System;
using UnityEngine;
using UnityEngine.Events;


namespace Rescues
{
    [Serializable]
    public sealed class EventData : IInteractable
    {
        #region Fields

        [SerializeField] private bool _isInteractionLocked;
        /// <summary>
        /// Is event repeat after end?
        /// </summary>
        public bool IsRepeating;

        /// <summary>
        /// Time before invoke event
        /// </summary>
        public float TimeBeforeInvoke;
        [SerializeField] private string _id = "-1";

        /// <summary>
        /// Event himself
        /// </summary>
        public UnityEvent Event;

        #endregion


        #region Properties

        public string Id { get => _id; set => _id = value; }
        public bool IsInteractable { get; set; }
        public bool IsInteractionLocked { get => _isInteractionLocked; set => _isInteractionLocked = value; }
        public string Description { get; set; }

        #endregion
    }
}
