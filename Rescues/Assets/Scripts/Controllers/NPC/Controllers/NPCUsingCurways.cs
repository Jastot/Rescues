using System;
using System.Collections.Generic;
using System.Linq;
using PathCreation;
using Rescues.NPC.Models;
using UnityEngine;

namespace Rescues.NPC.Controllers
{
    public sealed class NPCUsingCurways
    {
        #region Methods

        public void MoveNPCs(List<BaseNPC> whoIsPatrolling)
        {
            foreach (var npc in whoIsPatrolling)
            {
                //альтернативу бы !=null...
                if (npc.CurrentCurveWay!=null)
                {
                    if ((Math.Abs(npc.transform.position.x - npc.CurrentCurveWay.PathCreator.path.localPoints[0].x) >
                         0.001f &&
                         npc.Direction != 1) ||
                        (Math.Abs(npc.transform.position.x -
                                  npc.CurrentCurveWay.PathCreator.path.localPoints.Last().x) > 0.001f &&
                         npc.Direction != -1))
                    {
                        if (!npc.InRage)
                            npc.Distanse += npc.Direction * npc.NpcData.NpcStruct.PatrollingSpeed * Time.deltaTime;
                        else
                            npc.Distanse += npc.Direction * npc.NpcData.NpcStruct.InRageSpeed * Time.deltaTime;
                        npc.transform.position =
                            npc.CurrentCurveWay.PathCreator.path.GetPointAtDistance(npc.Distanse,
                                EndOfPathInstruction.Stop);
                    }
                    // if (npc.Direction > 0 && npc.NPCArmature._armature.flipX)
                    // {
                    //     Flip(npc.NPCArmature);
                    // }
                    // else if (npc.Direction < 0 && !npc.NPCArmature._armature.flipX)
                    // {
                    //     Flip(npc.NPCArmature);
                    // }
                }
            }
        }
        private void Flip(DragonBones.UnityArmatureComponent armatureComponent)
        {
            armatureComponent._armature.flipX = !armatureComponent._armature.flipX;
        }
     
        #endregion
    }
}