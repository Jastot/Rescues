using System;
using UnityEngine.Serialization;

namespace Rescues
{
    [Serializable]
    public struct GateStruct
    {
        public string goToLevelName;
        public string goToLocationName;
        public int goToGateId;

    }
}