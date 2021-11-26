using System.Collections.Generic;


namespace Rescues
{
    /// <summary>
    /// Добавляется на объект, с которым должен взаимойдествоать главный герой
    /// </summary>
    public sealed class PuzzleBehaviour : InteractableObjectBehavior
    {
        #region Fields

        public Puzzle Puzzle;
        public ItemData ItemData;
        public List<EventData> finishEvents;

        #endregion


        #region UnnityMethods

        private void OnValidate()
        {
            Type = InteractableObjectType.Puzzle;
        }

        #endregion
    }
}
