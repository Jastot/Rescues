using System;
using System.Collections.Generic;

namespace Rescues
{
    [Serializable]
    public class SavedEventSequenceUnit
    {
        //TODO: Заменить на IInteractibleSaveStruct
        public SavingStruct savingStruct;
        public int indexInList;
    }
}