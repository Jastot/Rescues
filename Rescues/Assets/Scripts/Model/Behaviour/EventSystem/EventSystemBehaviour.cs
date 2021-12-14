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
            ActivateEventData(OnButtonInTriggerEvents);
        }

        public void ActivateEventData(List<EventData> data)
        {
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i].IsInteractionLocked == false)
                {
                    ExecuteAdvancedOptions(data[i]);
                    TimeRemainingExtensions.AddTimeRemaining(new TimeRemaining(data[i]));
                }
            }
        }

        public void LockEventsByIDs(string[] commandValues)
        {
            for (int j = 0; j < commandValues.Length; j++)
            {
                for (int i = 0; i < OnTriggerEnterEvents.Count; i++)
                {
                    if (OnTriggerEnterEvents[i].Id == commandValues[j])
                    {
                        OnTriggerEnterEvents[i].IsInteractionLocked = !OnTriggerEnterEvents[i].IsInteractionLocked;
                        if (OnTriggerEnterEvents[i].IsInteractionLocked == false)
                        {
                            OnTriggerEnterEvents[i].Event.Invoke();
                        }
                        break;
                    }
                }

                for (int i = 0; i < OnTriggerExitEvents.Count; i++)
                {
                    if (OnTriggerExitEvents[i].Id == commandValues[j])
                    {
                        OnTriggerExitEvents[i].IsInteractionLocked = !OnTriggerExitEvents[i].IsInteractionLocked;
                        break;
                    }
                }

                for (int i = 0; i < OnButtonInTriggerEvents.Count; i++)
                {
                    if (OnButtonInTriggerEvents[i].Id == commandValues[j])
                    {
                        OnButtonInTriggerEvents[i].IsInteractionLocked = !OnButtonInTriggerEvents[i].
                            IsInteractionLocked;
                        break;
                    }
                }
            }
        }

        private void ExecuteAdvancedOptions(EventData data)
        {
            foreach (KeyValuePair<EventSystemCommands, string> command in data.objectManipulations)
            {
                switch (command.Key)
                {
                    case EventSystemCommands.LockEventByID:
                        {
                            LockEventsByIDs(ToStringArray(command.Value));
                            break;
                        }
                    default:
                        break;
                }
            }
        }

        private string[] ToStringArray(string stringToConvert)
        {
            List<string> strings = new List<string>();
            char[] delimeters = { ',', '-', '_', ' ' };
            string[] split = stringToConvert.Split(delimeters);
            for (int i = 0; i < split.Length; i++)
            {
                if (string.IsNullOrEmpty(split[i]) == false)
                {
                    strings.Add(split[i]);
                }
            }

            return strings.ToArray();
        }

        #endregion

        #region UnityMethods

        private void OnValidate()
        {
            Type = InteractableObjectType.EventSystem;
        }

        #endregion
    }
}
