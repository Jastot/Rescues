using System;
using System.Collections.Generic;
using System.Linq;
using Rescues.NPC.Controllers;
using Rescues.NPC.Models;
using UnityEngine;


namespace Rescues
{
    public class Location : MonoBehaviour
    {

        #region Filed

        [SerializeField] private Transform _cameraPosition;
        [SerializeField] private List<NPCPatrollingData> _levelNPC;

        public NPCLevelController NpcLevelController;
        public NPCStorage _storage;
        [NonSerialized] public Transform CurrentNPCStorage;
        #endregion
        
        
        #region Properties
        
        public Vector3 CameraPosition => _cameraPosition.position;
        public List<CurveWay> СurveWays { get; private set; }
        public List<NPCPatrollingData> LevelNPC => _levelNPC;

        #endregion
        
        
        #region UnityMethods
        
        private void Awake()
        {
            СurveWays = transform.GetComponentsInChildren<CurveWay>().ToList();
            NpcLevelController = new NPCLevelController();
            _storage = new NPCStorage();
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        #endregion
        
        
    }
    
}