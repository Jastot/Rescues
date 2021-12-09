using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace Rescues
{
    [Serializable]
    public sealed class LevelProgress
    {
        #region Fields

        //TODO: Saving NPC data
        public string levelsName;
        public List<SavedQuestUnit> questListData = new List<SavedQuestUnit>();
        public List<SavedPuzzleUnit> puzzleListData = new List<SavedPuzzleUnit>();
        public List<SavedEventSequenceUnit> eventSequenceData = new List<SavedEventSequenceUnit>();
        public List<SavedDialogUnit> dialogListData = new List<SavedDialogUnit>();

        #endregion
    }
}