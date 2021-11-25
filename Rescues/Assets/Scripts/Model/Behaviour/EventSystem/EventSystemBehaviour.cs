using System.Collections.Generic;


namespace Rescues
{
    public sealed class EventSystemBehaviour : InteractableObjectBehavior
    {
        #region Fields

        public List<EventData> OnTriggerEnterEvents;
        public List<EventData> OnTriggerExitEvents;
        public List<EventData> OnButtonInTriggerEvents;

        #endregion


        #region Methods

        public void ActivateButtonInTriggerEvent()
        {
            ActivateEvent(OnButtonInTriggerEvents);
        }

        public void ActivateEvent(List<EventData> events)
        {
            for (int i = 0; i < events.Count; i++)
            {
                if (events[i].IsInteractionLocked == false)
                {
                    TimeRemainingExtensions.AddTimeRemaining(new TimeRemaining(events[i]));
                }
            }
        }

        #endregion
    }
}
