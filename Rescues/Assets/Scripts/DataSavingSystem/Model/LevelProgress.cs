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
        public List<QuestListData> questListData = new List<QuestListData>();
        public List<PuzzleListData> puzzleListData = new List<PuzzleListData>();
        public List<InteractiveCondition> listOfInteractable = new List<InteractiveCondition>();
        
        #endregion
    }
}