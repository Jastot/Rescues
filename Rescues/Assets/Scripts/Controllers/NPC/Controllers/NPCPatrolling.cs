using System.Collections.Generic;
using Rescues.NPC.Models;
using UnityEngine;

namespace Rescues.NPC.Controllers
{
    public sealed class NPCPatrolling
    {
        #region Methods

        public void Patrolling(List<BaseNPC> whoIsPatrolling)
        {
            foreach (var npc in whoIsPatrolling)
            {
                if (!npc.InRage)
                {
                    var wayPointInfo = npc.NpcWayPointsArray;
                    if (Vector3.Distance(npc.transform.position,
                        wayPointInfo[npc.wayPointCounter].transform.position) > 0.5f)
                    {
                        if (npc.transform.position.x - wayPointInfo[npc.wayPointCounter].transform.position.x == 0f)
                        {
                            npc.Direction = 0;
                        }
                        else
                        {
                            if (npc.transform.position.x - wayPointInfo[npc.wayPointCounter].transform.position.x <
                                0.5f)
                                npc.Direction = 1;
                            else
                                npc.Direction = -1;
                        }
                        Vision(npc);
                    }
                    else
                    {
                        var modificator = npc.Modificator;
                        if (npc.wayPointCounter + modificator > wayPointInfo.Length - 1 ||
                            npc.wayPointCounter + modificator < 0)
                            npc.InvertModificator();
                        npc.Direction = 0;
                        npc.WaitTime(wayPointInfo[npc.wayPointCounter].WaitTime);
                    }
                }
                else
                {
                    GoingToPlayer(npc);
                }
            }
        }

        public void GoingToPlayer(BaseNPC npc)
        {
            TorchDirection(npc);
            Vision(npc);
        }

        public void Vision(BaseNPC npc)
        {
            Vector3 movementDirection = Vector3.zero;
            movementDirection.x = npc.Direction;
            npc.SetVisionDirection(movementDirection);
        }

        public void TorchDirection(BaseNPC npc)
        {
            if (npc.transform.position.x-npc.DetectedPlayer.position.x < 0.5f)
            {
                npc.Direction = 1;
            }
            else
            {
                npc.Direction = -1;
            }
        }
        #endregion
    }
}