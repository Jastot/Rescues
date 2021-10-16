using System;
using Rescues.NPC.Models;
using UnityEngine;

namespace Rescues
{
    [Serializable]
    public sealed class NPCPatrollingData
    {
        [SerializeField] public NPCData _npcData; 
        [SerializeField] public NPCWayPoints[] _wayPoints;
    }
}