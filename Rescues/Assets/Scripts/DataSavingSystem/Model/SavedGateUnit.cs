using System;
using UnityEngine.Serialization;

namespace Rescues
{
    [Serializable]
    public class SavedGateUnit//:InteractiveCondition
    {
        //TODO: Добавить IInteractibleSaveStruct
        public string goToLevelName;
        public string goToLocationName;
        public int goToGateId;

    }
}