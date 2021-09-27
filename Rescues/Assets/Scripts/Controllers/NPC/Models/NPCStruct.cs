using System;
using Rescues.NPC.Controllers;
using UnityEngine;

namespace Rescues.NPC.Models
{
    [Serializable]
    public struct NPCStruct
    {
        public float PatrollingSpeed;
        public float InRageSpeed;
        public float NPCvision;
        public NPCState NPCState;
    }
}