using System;
using System.Collections.Generic;

namespace Rescues
{
    [Serializable]
    public struct SavingPacked
    {
        public string PlayerPosition;
        public PlayersProgress PlayersProgress;
        public SavedGateUnit LastGate;
        public List<SavedItemUnit> ItemBehaviours;
        public List<LevelProgress> LevelsProgress;
    }
}