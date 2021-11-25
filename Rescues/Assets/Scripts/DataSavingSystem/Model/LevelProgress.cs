using System.Collections.Generic;

namespace Rescues
{
    public sealed class LevelProgress
    {
        #region Fields

        //TODO: Saving NPC data
        public string LevelsName;
        public IGate LastGate;
        public List<QuestListData> QuestListData = new List<QuestListData>();
        public List<PuzzleListData> PuzzleListData = new List<PuzzleListData>();
        public List<InteractiveCondition> ListOfInteractable = new List<InteractiveCondition>();
        
        #endregion
    }
}