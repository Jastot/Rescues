using System;
using Rescues.NPC.Controllers;
using UnityEngine;

namespace Rescues.NPC.Models
{
    public class BaseNPC: MonoBehaviour, IExecuteController
    {
        #region Fields

        [NonSerialized]public NPCData NpcData;
        [NonSerialized]public NPCWayPoints[] NpcWayPointsArray;
        [NonSerialized]public CurveWay CurrentCurveWay;
        [NonSerialized]public int wayPointCounter = 0;
        [NonSerialized]public int Direction;
        [NonSerialized]public float Distanse;
        [NonSerialized]public bool InRage=false;
        [NonSerialized]public Transform DetectedPlayer = null;
        [NonSerialized]public DragonBones.UnityArmatureComponent NPCArmature;
        public int Modificator { get => _modificator; }

        private int _modificator = 1;
        private PhysicalServices _physicsService;
        private Vector3 _visionDirection;
        private Animator _animator;
        private TimeRemaining _timeRemaining;
        private NPCCatch _npcCatch;
        
        #endregion

        #region UnityMethods

        private void Awake()
        {
            _timeRemaining = new TimeRemaining(ResetWaitState, 0.0f);
            _physicsService = Services.SharedInstance.PhysicalServices;
            _animator = GetComponent<Animator>();
            _npcCatch = new NPCCatch();
            InRage = false;
            NpcData.NpcStruct.NPCState = NPCState.None;
        }
        private void OnTriggerEnter(Collider other)
        {
           
        }

        #endregion

        #region Methods

        public void Execute()
        {
            switch (GetWaitState())
            {
                case NPCState.None: 
                {
                    _animator.Play("Base Layer.None");
                    break;
                }
                case NPCState.Patrol:
                {
                    _animator.Play("Base Layer.Patrol");
                    break;
                }
                case NPCState.Pursuit:
                {
                    _animator.Play("Base Layer.Inspection");
                    break;
                }
            }
        }
        public NPCState GetWaitState()
        {
            return new NPCState();
        }
        
        public void SetVisionDirection(Vector3 visionDirection)
        {
            _visionDirection = visionDirection.normalized;
        }
        public void InvertModificator()
        {
            _modificator *= -1;
        }
        
        private void SetInspectionState()
        {
            var v =NpcData.NpcStruct.NPCState == NPCState.Patrol
                ? NpcData.NpcStruct.NPCState = NPCState.Patrol
                :NpcData.NpcStruct.NPCState = NPCState.Pursuit;
        }
        
        public void WaitTime(float waitTime)
        {
            SetInspectionState();
            _timeRemaining?.AddTimeRemaining(waitTime);
        }
        
        public void Vision()
        {
            if (_physicsService != null)
            {
                var hit = _physicsService.VisionDetectionPlayer(transform.position,
                    _visionDirection, NpcData.NpcStruct.NPCvision,Color.cyan);
            
                if (hit)
                {
                    DetectedPlayer = hit.collider.transform;
                    InRage = true;
                    _npcCatch.CatchZoneCheck(this,_physicsService,_visionDirection);
                    //ScreenInterface.GetInstance().Execute(ScreenType.GameOver);
                }
                else
                {
                    DetectedPlayer = null;
                    InRage = false;
                }
            }
        }

        public void Catch()
        {
            
        }

        private void ResetWaitState()
        {
            NpcData.NpcStruct.NPCState = NPCState.Patrol;
            wayPointCounter += Modificator;
        }
        #endregion
        
    }
}