using System;
using System.Collections.Generic;


namespace Rescues
{
    [Serializable]
    public struct ItemEventsData
    {
        #region Fields

        /// <summary>
        /// The item which you drag and drop
        /// </summary>
        public ItemData ItemData;

        /// <summary>
        /// The event which depends on your item
        /// </summary>
        public List<EventData> Events;

        #endregion
    }
}
