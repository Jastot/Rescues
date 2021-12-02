using System.Collections.Generic;


namespace Rescues
{
    public class SequentialTimeRemainingsContainer
    {
        #region Fields

        public int currentSeqElementIndex;
        public List<List<ITimeRemaining>> sequentialTimeRemainings;

        #endregion


        #region ClassLifeCycles

        public SequentialTimeRemainingsContainer()
        {
            sequentialTimeRemainings = new List<List<ITimeRemaining>>();
        }

        #endregion
    }
}
