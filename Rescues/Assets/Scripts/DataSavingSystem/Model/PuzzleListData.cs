using System;

namespace Rescues
{
    [Serializable]
    public struct PuzzleListData
    {
        #region Fields

        public SavingStruct SavingStruct;
        public PuzzleCondition PuzzleCondition { get; set; }

        #endregion
    }
}