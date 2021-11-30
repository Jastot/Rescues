using System;

namespace Rescues
{
    [Serializable]
    public struct QuestListData
    {
        #region Fields
        
        public SavingStruct SavingStruct;
        public QuestCondition QuestCondition { get; set; }

        #endregion
    }
}