using UnityEngine;


namespace Rescues
{
    public sealed class NotepadController : IInitializeController, ITearDownController, IExecuteController
    {
        #region Fields

        private readonly GameContext _context;
        private NotepadBehaviour _notepadBehaviour;
        private NotepadTriggerBehaviour[] _notepadTriggerBehaviours;

        #endregion


        #region ClassLifeCycles

        public NotepadController(GameContext context)
        {
            _context = context;
        }

        #endregion


        #region IInitializeController

        public void Initialize()
        {           
            _notepadBehaviour = Object.FindObjectOfType<NotepadBehaviour>(true);
            _notepadBehaviour.Initialize();
            _context.notepad = _notepadBehaviour;

            _notepadTriggerBehaviours = Object.FindObjectsOfType<NotepadTriggerBehaviour>(true);
            foreach (var ntb in _notepadTriggerBehaviours)
                ntb.TriggerActivation += ProcessTriggers;
        }

        #endregion


        #region ITearDownController

        public void TearDown()
        {
            foreach (var ntb in _notepadTriggerBehaviours)
                ntb.TriggerActivation -= ProcessTriggers;
        }

        #endregion


        #region IExecuteController

        public void Execute()
        {
            if (_notepadBehaviour.isActiveAndEnabled)
                _notepadBehaviour.ShowPageButtons();
        }

        #endregion


        #region Methods

        private void ProcessTriggers(NotepadTrigger[] triggers)
        {
            foreach (var trigger in triggers)
                _notepadBehaviour.ProcessTrigger(trigger);
        }

        #endregion
    }
}
