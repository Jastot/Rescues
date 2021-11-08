using System.Collections.Generic;

namespace Rescues
{
    public sealed class NPCVisionController: IExecuteController
    {
        private readonly List<BaseNPC> _baseNpSs;

        #region UnityMethods

        public NPCVisionController(List<BaseNPC> baseNPSs)
        {
            _baseNpSs = baseNPSs;
        }
        public void Execute()
        {
            foreach (var npc in _baseNpSs)
            {
                if (npc.NpcData.NpcStruct.NPCState != NPCState.Dead)
                {
                    npc.Vision();
                }
            }
        }

        #endregion
    }
}