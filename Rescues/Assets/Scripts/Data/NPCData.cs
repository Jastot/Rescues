using Rescues.NPC.Models;
using UnityEngine;

namespace Rescues
{
    [CreateAssetMenu(fileName = "NPCData", menuName = "Data/NPC/NPCData")]
    public class NPCData: ScriptableObject
    {
        #region Fields
        
        public NPCStruct NpcStruct;

        #endregion
    }
}