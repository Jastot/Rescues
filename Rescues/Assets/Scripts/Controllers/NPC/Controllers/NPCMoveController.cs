namespace Rescues.NPC.Controllers
{
    public class NPCMoveController: IInitializeController, IExecuteController
    {
        private NPCPatrolling _npcPatrollingController;
        private NPCUsingPlayersWay _npcUsingPlayersWay;


        public void Initialize()
        {
            _npcPatrollingController = new NPCPatrolling();
            _npcUsingPlayersWay = new NPCUsingPlayersWay();
            
        }

        public void Execute()
        {
            throw new System.NotImplementedException();
        }
    }
}