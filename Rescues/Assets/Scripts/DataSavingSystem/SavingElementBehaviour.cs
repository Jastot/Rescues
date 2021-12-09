using System;
using System.Collections.Generic;
using Rescues;
using UnityEngine;

namespace DataSavingSystem
{
    public sealed class SavingElementBehaviour : MonoBehaviour
    {
        [SerializeField] private List<EventSystemBehaviour> PreviousParts = new List<EventSystemBehaviour>();
        public Action<SavedEventSequenceUnit,string> SaveSequence = delegate { };
        
        public void SavePreviousElement(int numberInList)
        {
            var part = new SavedEventSequenceUnit
            {
                savingStruct = new SavingStruct()
                {
                    Id = PreviousParts[numberInList].Id,
                    Name = PreviousParts[numberInList].name
                },
                indexInList = numberInList
            };
            SaveSequence.Invoke(
                part,
                gameObject.transform.parent.parent.name);
        }

        public void GetPartInfo(int index,out string Id, out string Name)
        {
            if (index<PreviousParts.Count)
            {
                Id = PreviousParts[index].Id;
                Name = PreviousParts[index].name;
            }
            else
            {
                Id = "";
                Name = "";
            }
        }

        public void SetPartStateFalse(int index)
        {
            foreach (var eventData in PreviousParts[index].OnTriggerEnterEvents)
                        eventData?.Event.Invoke();
            foreach (var eventData in PreviousParts[index].OnTriggerExitEvents)
                        eventData?.Event.Invoke();
            foreach (var eventData in PreviousParts[index].OnButtonInTriggerEvents)
                        eventData?.Event.Invoke();
            
        }
    }
}