using System;
using UnityEngine;


namespace Rescues
{
    public sealed class NotepadTriggerBehaviour : MonoBehaviour
    {
        #region Fields

        [SerializeField] private NotepadTrigger[] _notepadTriggers;
        public event Action<NotepadTrigger[]> TriggerActivation;

        #endregion


        #region Methods

        public void TriggerNotepadEntries()
        {
            TriggerActivation?.Invoke(_notepadTriggers);
        }

        #endregion
    }
}