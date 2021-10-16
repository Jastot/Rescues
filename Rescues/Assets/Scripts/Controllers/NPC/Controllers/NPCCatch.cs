using Rescues.NPC.Models;
using UnityEngine;

namespace Rescues.NPC.Controllers
{
    public sealed class NPCCatch
    {
        public void CatchZoneCheck(BaseNPC baseNpc,PhysicalServices physicsService,Vector3 visionDirection)
        {
            Vector3 upPosition = baseNpc.transform.position + Vector3.up;
            var hit = physicsService.VisionDetectionPlayer(upPosition,
                visionDirection, baseNpc.NpcData.NpcStruct.NPCcatch, Color.red);
            if (hit)
            {
                hit.transform.GetComponentInParent<PlayerBehaviour>().PlayerWasCaught();
            }
        }
    }
}