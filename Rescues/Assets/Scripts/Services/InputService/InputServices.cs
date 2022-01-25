using System.Collections.Generic;
using UnityEngine;

namespace Rescues
{
    public sealed class InputServices : Service
    {
        #region Fields

        public readonly InputAxis HorizontalAxis;
        public readonly InputAxis VerticalAxis;
        public readonly InputButton CancelButton;
        public readonly InputButton PickUpButton;
        public readonly InputButton InventoryButton;
        public readonly InputButton NotepadButton;
        public readonly InputButton MouseScrollButton;
        public readonly InputButton UseButton;

        private List<IInput> _inputs;

        #endregion


        #region ClassLifeCycles

        public InputServices(Contexts contexts) : base(contexts)
        {
            HorizontalAxis = new InputAxis(KeyCode.D, KeyCode.A);
            VerticalAxis = new InputAxis(KeyCode.W, KeyCode.S);
            CancelButton = new InputButton(KeyCode.Escape);
            PickUpButton = new InputButton(KeyCode.Space);
            InventoryButton = new InputButton(KeyCode.I);
            NotepadButton = new InputButton(KeyCode.J);
            MouseScrollButton = new InputButton(KeyCode.Mouse2);
            UseButton = new InputButton(KeyCode.E);

            _inputs = new List<IInput>(8);
            _inputs.Add(HorizontalAxis);
            _inputs.Add(VerticalAxis);
            _inputs.Add(CancelButton);
            _inputs.Add(PickUpButton);
            _inputs.Add(InventoryButton);
            _inputs.Add(NotepadButton);
            _inputs.Add(MouseScrollButton);
            _inputs.Add(UseButton);
        }

        #endregion


        #region Methods

        public void UpdateInputs()
        {
            foreach (var input in _inputs)
                input.UpdateInputValues();
        }

        #endregion
    }
}