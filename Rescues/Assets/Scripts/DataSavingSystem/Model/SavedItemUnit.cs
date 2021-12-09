using System;

namespace Rescues
{
    [Serializable]
    public class SavedItemUnit
    {
        #region Fields
        //TODO: Заменить на IInteractibleSaveStruct
        public SavingStruct SavingStruct;
        public ItemCondition ItemCondition;

        #endregion
    }
}