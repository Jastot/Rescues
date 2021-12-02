using System;
using System.Collections.Generic;
using Rescues;
using UnityEngine;

namespace DataSavingSystem
{
    public sealed class SavingElementBehaviour : MonoBehaviour
    {
        [SerializeField] private List<EventSystemBehaviour> PreviousParts = new List<EventSystemBehaviour>();
        public Action<EventSequence,string,int> SaveSequence = delegate { };
        
        public void SavePreviousElement(int numberInList)
        {
            var part = new EventSequence
            {
                savingStruct = new SavingStruct()
                {
                    Id = PreviousParts[numberInList].Id,
                    Name = PreviousParts[numberInList].name
                },
                indexInList = numberInList
            };
            Debug.Log("INVOKE: id: "+part.savingStruct.Id+" name: "+part.savingStruct.Name+ " index: "+part.indexInList);
            SaveSequence.Invoke(part,gameObject.transform.parent.parent.name,numberInList);
        }

        public void GetPartInfo(int index,out string Id, out string Name)
        {
            Id = PreviousParts[index].Id;
            Name = PreviousParts[index].name;
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