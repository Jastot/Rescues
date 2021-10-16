using UnityEngine;


namespace Rescues
{
    public class ActivatorController : IExecuteController, IInitializeController
    {
        #region Fields

        private PlayerActivator _playerActivator;

        #endregion


        #region IInitializeController

        public void Initialize()
        {
            _playerActivator = Object.FindObjectOfType<PlayerActivator>();
        }

        #endregion


        #region IExecuteController

        public void Execute()
        {
            if (Input.GetButtonUp("Use") && _playerActivator.Triggers.Count > 0)
            {
                foreach (var trigger in _playerActivator.Triggers)
                {
                    trigger.ActivateButtonInTriggerEvent();
                }
            }
        }

        #endregion
    }
}
