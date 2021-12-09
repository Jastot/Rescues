using System;

namespace Rescues
{
    [Serializable]
    public class SavedQuestUnit
    {
        #region Fields
        //TODO: Заменить на IInteractibleSaveStruct
        public SavingStruct SavingStruct;
        public QuestCondition QuestCondition { get; set; }

        #endregion
    }
}