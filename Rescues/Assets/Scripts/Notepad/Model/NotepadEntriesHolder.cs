using System;
using System.Collections.Generic;


namespace Rescues
{
    [Serializable]
    public sealed class NotepadEntriesHolder
    {
        #region Fields

        private List<NotepadEntry> _quests = new List<NotepadEntry>();
        private List<NotepadEntry> _dialogues = new List<NotepadEntry>();
        private List<NotepadEntry> _characters = new List<NotepadEntry>();
        private List<NotepadEntry> _lore = new List<NotepadEntry>();

        private List<NotepadEntry> _currentCategory;

        #endregion


        #region Methods

        public void AddEntry(NoteCategory category, string entryName, int bulletpointId)
        {
            _currentCategory = SelectCategory(category);

            var entryIndex = _currentCategory.FindIndex(x => x.EntryName.Equals(entryName));

            if (entryIndex < 0)
            {

                _currentCategory.Add(new NotepadEntry
                {
                    EntryName = entryName,
                    BulletPoints = new List<NotepadBulletpoint> {
                        new NotepadBulletpoint { Id = bulletpointId < 0 ? 0 : bulletpointId
                        }
                    }
                });
            }
            else
            {
                _currentCategory[entryIndex].AddBulletpoint(bulletpointId);
            }
        }

        public void RemoveEntry(NoteCategory category, string entryName, int bulletpointId)
        {
            _currentCategory = SelectCategory(category);

            var entryIndex = _currentCategory.FindIndex(x => x.EntryName.Equals(entryName));

            if (entryIndex < 0)
                return;

            if (bulletpointId >= 0)
                _currentCategory[entryIndex].RemoveBulletpoint(bulletpointId);
            else
                _currentCategory.RemoveAt(entryIndex);
        }

        public void CrossOutEntry(NoteCategory category, string entryName, int bulletpointId)
        {
            _currentCategory = SelectCategory(category);

            var entryIndex = _currentCategory.FindIndex(x => x.EntryName.Equals(entryName));

            if (entryIndex < 0)
                return;

            if (bulletpointId >= 0)
                _currentCategory[entryIndex].CrossOutBulletpoint(bulletpointId);
            else
                _currentCategory[entryIndex].CrossOutEntry();
        }

        public List<NotepadEntry> GetEntries(NoteCategory category) => SelectCategory(category);

        private List<NotepadEntry> SelectCategory(NoteCategory category)
        {
            switch (category)
            {
                case NoteCategory.Quest:
                    {
                        return _quests;
                    }
                case NoteCategory.Dialogue:
                    {
                        return _dialogues;
                    }
                case NoteCategory.Character:
                    {
                        return _characters;
                    }
                case NoteCategory.Lore:
                    {
                        return _lore;
                    }
                default:
                    {
                        throw new System.Exception("Each NotepadTrigger is NotepadTriggerBehaviour component must have value assigned to its Category field");
                    }
            }
        }

        #endregion
    }
}