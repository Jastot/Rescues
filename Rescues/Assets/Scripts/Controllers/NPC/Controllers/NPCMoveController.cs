using System.Collections.Generic;
using Rescues.NPC.Models;

namespace Rescues.NPC.Controllers
{
    public sealed class NPCMoveController: IInitializeController, IExecuteController
    {
        #region Fields

        private NPCPatrolling _npcPatrollingController;
        private NPCUsingCurways _npcUsingCurways;
        private List<BaseNPC> _whoIsPatrollingOnLocation;
        
        #endregion

        public NPCMoveController(List<BaseNPC> whoIsPatrollingOnLocation)
        {
            _whoIsPatrollingOnLocation = whoIsPatrollingOnLocation;
        }
        
        #region Methods

        public void Initialize()
        {
            _npcPatrollingController = new NPCPatrolling();
            _npcUsingCurways = new NPCUsingCurways();
        }

        public void Execute()
        {
            _npcPatrollingController?.Patrolling(_whoIsPatrollingOnLocation);
            _npcUsingCurways?.MoveNPCs(_whoIsPatrollingOnLocation);
        }
        
        #endregion
    }
}