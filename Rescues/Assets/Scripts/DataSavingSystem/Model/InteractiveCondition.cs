using System;

namespace Rescues
{
    [Serializable]
    public sealed class InteractiveCondition
    {
        public SavingStruct SavingStruct;
        public bool IsInteractable;
        public bool IsInteractionLocked;
    }
}