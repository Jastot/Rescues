using UnityEngine;


namespace Rescues
{
    public sealed class EventSystemController : IInitializeController, ITearDownController
    {
        #region Fields

        private readonly GameContext _context;

        #endregion


        #region ClassLifeCycles

        public EventSystemController(GameContext context)
        {
            _context = context;
        }

        #endregion


        #region IInitializeController

        public void Initialize()
        {
            var eventSystems = _context.GetTriggers(InteractableObjectType.EventSystem);
            foreach (var es in eventSystems)
            {
                var onTriggerEvent = es as InteractableObjectBehavior;
                onTriggerEvent.OnFilterHandler += OnFilterHandler;
                onTriggerEvent.OnTriggerEnterHandler += OnTriggerEnterHandler;
                onTriggerEvent.OnTriggerExitHandler += OnTriggerExitHandler;
            }
        }

        #endregion


        #region ITearDownController

        public void TearDown()
        {
            var eventSystems = _context.GetTriggers(InteractableObjectType.EventSystem);
            foreach (var es in eventSystems)
            {
                var onTriggerEvent = es as InteractableObjectBehavior;
                onTriggerEvent.OnFilterHandler -= OnFilterHandler;
                onTriggerEvent.OnTriggerEnterHandler -= OnTriggerEnterHandler;
                onTriggerEvent.OnTriggerExitHandler -= OnTriggerExitHandler;
            }
        }

        #endregion


        #region Methods

        private bool OnFilterHandler(Collider2D obj)
        {
            return obj.CompareTag(TagManager.PLAYER);
        }

        private void OnTriggerEnterHandler(ITrigger enteredObject)
        {
            var eventSystem = enteredObject as EventSystemBehaviour;
            if (eventSystem.IsInteractionLocked == false)
            {
                eventSystem.IsInteractable = true;
                eventSystem.ActivateEventData(eventSystem.OnTriggerEnterEvents); 
            }
        }

        private void OnTriggerExitHandler(ITrigger enteredObject)
        {
            var eventSystem = enteredObject as EventSystemBehaviour;
            if (eventSystem.IsInteractionLocked == false)
            {
                eventSystem.IsInteractable = false;
                eventSystem.ActivateEventData(eventSystem.OnTriggerExitEvents); 
            }
        }

        #endregion
    }
}
