using System;
using UnityEngine;
using UnityEngine.Events;


namespace Rescues
{
    [Serializable]
    public sealed class EventData : IInteractable
    {
        #region Fields

        [Tooltip("That's lock an iteraction")]
        [SerializeField] private bool _isInteractionLocked;

        [Tooltip("Is event repeat after end?")]
        public bool IsRepeating;

        [Tooltip("Time before invoke event")]
        public float TimeBeforeInvoke;

        [Tooltip("ID of the event")]
        [SerializeField] private string _id = "-1";

        [Tooltip("Event himself")]
        public UnityEvent Event;

        [Tooltip("Special commands for advanced manipulations")]
        public SerializableDictionary<EventSystemCommands, string> objectManipulations;

        #endregion


        #region Properties

        public string Id { get => _id; set => _id = value; }
        public bool IsInteractable { get; set; }
        public bool IsInteractionLocked { get => _isInteractionLocked; set => _isInteractionLocked = value; }
        public string Description { get; set; }

        #endregion
    }
}
