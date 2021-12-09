using System;

namespace Rescues
{
    [Serializable]
    public class SavedPuzzleUnit
    {
        #region Fields
        //TODO: Заменить на IInteractibleSaveStruct
        public SavingStruct SavingStruct;
        public PuzzleCondition PuzzleCondition;

        #endregion
    }
}