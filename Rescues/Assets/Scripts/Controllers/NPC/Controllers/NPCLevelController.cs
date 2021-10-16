using System.Collections.Generic;
using System.Linq;
using Rescues.NPC.Models;
using UnityEngine;

namespace Rescues.NPC.Controllers
{
    public sealed class NPCLevelController
    {
        #region Fields
        
        private List<GameObject> enemyObjects;
        private NPCMoveController _npcMoveController;
        private NPCVisionController _npcVisionController;
        private List<BaseNPC> _listOfNPC;
        private CurveWayController _curveWayController;
        
        #endregion

        public NPCLevelController()
        {
            enemyObjects = new List<GameObject>();
            _listOfNPC = new List<BaseNPC>();
            _npcMoveController = new NPCMoveController(_listOfNPC);
            _npcVisionController = new NPCVisionController(_listOfNPC);
            _npcMoveController.Initialize();
        }
        
        #region Methods

        // ставит npc на заявленную точку и запускает передвижение
        public void SpawnUnits(List<NPCPatrollingData> npcData,Transform parent)
        {
            int i = 0;
            foreach (var npc in npcData)
            {
                enemyObjects.Add(Object.Instantiate(npc._npcData.NpcStruct.Prefab, Vector3.zero, Quaternion.identity,parent));
                //var startWayPoint = npc._wayPoints[0];
                //enemyObjects[i].transform.position = startWayPoint.transform.position;
                var BaseNPC =enemyObjects[i].GetComponent<BaseNPC>();
                BaseNPC.NpcData = npc._npcData;
                BaseNPC.NpcWayPointsArray = npc._wayPoints;
                BaseNPC.NpcData.NpcStruct.index = i;
                AddUnit(BaseNPC);
                i++;
            }

        }

        public void SetCurveController(CurveWayController curveWayController)
        {
            _curveWayController = curveWayController;
        }
        
        public void SetCurvesForNPCs()
        {
            foreach (var npc in _listOfNPC)
            {
               var curve =  _curveWayController.GetCurve(npc.NpcWayPointsArray[0], WhoCanUseCurve.NPC);
               //npc.NpcWayPointsArray[0].transform.position;
               npc.transform.position = curve.GetStartPointPosition;
               npc.CurrentCurveWay = curve;
               CorrectDistance(npc);
            }
        }

        private void CorrectDistance(BaseNPC npc)
        {
            npc.Distanse= 0;
            npc.Distanse = npc.CurrentCurveWay.LeftmostPoint.x<0 ?
                npc.CurrentCurveWay.StartCharacterPosition.x - npc.CurrentCurveWay.LeftmostPoint.x:
                npc.CurrentCurveWay.StartCharacterPosition.x + npc.CurrentCurveWay.LeftmostPoint.x;
        }
        
        public void StartExecuting()
        {
            _npcMoveController.Execute();
            _npcVisionController.Execute();
        }
        
        public void DestroyUnit(int index)
        {
            Object.Destroy(enemyObjects[index]);
        }

        public void AddUnit(BaseNPC baseNpc)
        {
            _listOfNPC.Add(baseNpc);
        }

        public void DeleteUnit(BaseNPC baseNpc)
        {
            _listOfNPC?.Remove(baseNpc);
        }

        public void DeleteAllUnit()
        {
            _listOfNPC.Clear();
        }
        
        #endregion
       
    }
}