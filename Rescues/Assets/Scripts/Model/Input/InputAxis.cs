using UnityEngine;

namespace Rescues
{
    public sealed class InputAxis : IInput
    {
        #region Properties

        public InputButton Positive{ get; private set; }
        public InputButton Negative { get; private set; }
        public float Value { get; private set; }
        public bool IsReceivingInput { get; private set; }
        public bool IsReleased { get; private set; }

        #endregion


        #region ClassLifeCycles

        public InputAxis(KeyCode positive, KeyCode negative)
        {
            Positive = new InputButton(positive);
            Negative = new InputButton(negative);
        }

        #endregion


        #region IInput

        public void UpdateInputValues()
        {
            Positive.UpdateInputValues();
            Negative.UpdateInputValues();

            if (Positive.IsHeld == Negative.IsHeld)
                Value = 0f;
            else if (Positive.IsHeld)
                Value = 1f;
            else
                Value = -1f;

            IsReceivingInput = Positive.IsHeld || Negative.IsHeld;

            IsReleased = Positive.IsUp && !Negative.IsHeld ||
                Negative.IsUp && !Positive.IsHeld;
        }

        #endregion


        #region Methods

        public void SetNewKeys(KeyCode positive, KeyCode negative)
        {
            Positive = new InputButton(positive);
            Negative = new InputButton(negative);
        }

        #endregion
    }
}