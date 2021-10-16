using UnityEngine;

namespace Rescues
{
    public sealed class NPCStorage
    {
        public GameObject CreateNPC(Transform parent)
        {
            var gm = new GameObject("NPC");
            gm.transform.parent = parent;
            return gm;
        }
    }
}