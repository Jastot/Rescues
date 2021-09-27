using System;
using Rescues.NPC.Controllers;
using UnityEngine;

namespace Rescues.NPC.Models
{
    public class BaseNPC: MonoBehaviour, IExecuteController
    { 
        public NPCStruct _npcStruct;
        public RouteData RouteData;
        public NPCData NpcData;
        
        private PhysicsService _physicsService;
        private Vector3 _visionDirection;
        private Animator _animator;

        private void Awake()
        {
            _physicsService = Services.SharedInstance.PhysicsService;
            _animator = GetComponent<Animator>();
            NpcData.NpcStruct.NPCState = NPCState.None;
        }
        private void OnTriggerEnter(Collider other) 
        {
        }
        
        private void OnTriggerExit(Collider other) 
        {
            
        }


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
            return 
        }
        
        public void Vision()
        {
            var hit = _physicsService.VisionDetectionPlayer(transform.position, _visionDirection, EnemyData.VisionDistance);

            if (hit)
            {
                ScreenInterface.GetInstance().Execute(ScreenType.GameOver);
            }
        }
    }
}