using UnityEngine;

namespace Rescues
{ 
    public sealed class InputButton : IInput
    {  
        #region Properties

        public KeyCode Key { get; private set; }
        public bool IsDown { get; private set; }
        public bool IsHeld { get; private set; }
        public bool IsUp { get; private set; }

        #endregion


        #region ClassLifeCycles

        public InputButton(KeyCode key)
        {
            Key = key;
        }

        #endregion


        #region IInput

        public void UpdateInputValues()
        {
            IsDown = Input.GetKeyDown(Key);
            IsHeld = Input.GetKey(Key);
            IsUp = Input.GetKeyUp(Key);
        }

        #endregion


        #region Methods

        public void SetNewKey(KeyCode key)
        {
            Key = key;
        }

        #endregion
    }
}