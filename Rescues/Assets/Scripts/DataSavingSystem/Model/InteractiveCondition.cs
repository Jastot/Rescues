using System;

namespace Rescues
{
    [Serializable]
    public abstract class InteractiveCondition
    {
        public SavingStruct SavingStruct;
        //maybe need to add Type.
        public bool IsInteractable; 
        public bool IsInteractionLocked;
    }
}