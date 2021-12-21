using UnityEngine;


namespace Rescues
{
    public class PianoUse : IExecuteController
    {
        #region Fields

        private readonly PianoPuzzle _pianoPuzzle;

        #endregion


        #region Constructors

        public PianoUse(PianoPuzzle pianoPuzzle)
        {
            _pianoPuzzle = pianoPuzzle;
        }

        #endregion


        #region Methods

        public void Execute()
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                _pianoPuzzle.OnPianoButtonDown?.Invoke(_pianoPuzzle.CurrentButton);
            }
        } 

        #endregion
    }
}
