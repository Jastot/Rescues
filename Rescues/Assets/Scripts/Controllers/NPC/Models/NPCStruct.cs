using System;
using Rescues.NPC.Controllers;
using UnityEngine;

namespace Rescues.NPC.Models
{
    [Serializable]
    public struct NPCStruct
    {
        public int index;
        public GameObject Prefab;
        public float PatrollingSpeed;
        public float InRageSpeed;
        public NPCState NPCState;
        [Header("Дальность взгляда")]
        public float NPCvision;
        [Header("Дальность поимки")]
        public float NPCcatch;
    }
}