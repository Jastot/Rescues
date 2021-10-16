using System;
using UnityEngine;

namespace Rescues
{
    public sealed class PlayerBehaviour : MonoBehaviour
    {

        #region Fields

        public event Action Caught;

        #endregion
        
        #region Properties
        
        public Collider Collider { get; private set; }

        #endregion


        #region UnityMethods
        
        private void Awake()
        {
            Collider = GetComponent<Collider>();
        }

        #endregion

        public void PlayerWasCaught()
        {
            Caught?.Invoke();
        }
    }
}
