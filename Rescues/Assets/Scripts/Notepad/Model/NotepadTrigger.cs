using System;
using UnityEngine;


namespace Rescues
{
    [Serializable]
    public struct NotepadTrigger
    {
        #region Fields

        [SerializeField] private NoteCategory _category;
        [SerializeField] private string _entryID;
        [SerializeField, Min(-1)] private int _bulletpointID;
        [SerializeField] private NotepadEntryAction _action;

        #endregion


        #region Properties

        public NoteCategory Category => _category;
        public string EntryID => _entryID;
        public int BulletpointID => _bulletpointID;
        public NotepadEntryAction Action => _action;

        #endregion
    }
}