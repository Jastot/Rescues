using System;
using System.Collections.Generic;

namespace Rescues
{
    [Serializable]
    public struct SavingPacked
    {
        public string PlayerPosition;
        public PlayersProgress PlayersProgress;
        public GateStruct LastGate;
        public List<ItemListData> ItemBehaviours;
        public List<LevelProgress> LevelsProgress;
    }
}