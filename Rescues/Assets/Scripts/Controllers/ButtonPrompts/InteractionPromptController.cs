using System.Collections.Generic;
using UnityEngine;

namespace Rescues
{
    public sealed class InteractionPromptController : Controllers, IInitializeController
    {
        #region Fields

        private readonly GameContext _context;
        private readonly InputServices _inputService;

        private readonly Dictionary<InteractionPrompt, KeyCode> _prompts = new Dictionary<InteractionPrompt, KeyCode>();

        private Canvas _canvas;
        private Dictionary<InteractableObjectBehavior, InputPromptView> _activePrompts = new Dictionary<InteractableObjectBehavior, InputPromptView>();
        private Stack<InputPromptView> _availablePrompts = new Stack<InputPromptView>();

        private InputPromptView _promptPrefab;
        private string _promptPrefabPath = "Prefabs/UI/InputPrompts/InputPrompt";
        private string _promptCanvasPrefabPath = "Prefabs/UI/InputPrompts/PromptCanvas";

        private Vector3 _offset = Vector3.up;

        #endregion


        #region ClassLifeCycles

        public InteractionPromptController(GameContext context, Services services)
        {
            _context = context;
            _inputService = services.InputServices;

            FillPrompts();

            _promptPrefab = Resources.Load<InputPromptView>(_promptPrefabPath);
        }

        #endregion


        #region IInitializeController

        public override void Initialize()
        {
            var interactables = _context.GetListInteractable();
            foreach (var interactable in interactables)
            {
                if (!(interactable is InteractableObjectBehavior) ||
                    (interactable as InteractableObjectBehavior).InteractionPrompt == InteractionPrompt.None)
                    continue;

                var trigger = interactable as ITrigger;
                trigger.OnTriggerEnterHandler += OnTriggerEnterHandler;
                trigger.OnTriggerExitHandler += OnTriggerExitHandler;
            }
        }

        #endregion


        #region ITearDownController

        public override void TearDown()
        {
            var interactables = _context.GetListInteractable();
            foreach (var interactable in interactables)
            {
                var trigger = interactable as ITrigger;
                trigger.OnTriggerEnterHandler -= OnTriggerEnterHandler;
                trigger.OnTriggerExitHandler -= OnTriggerExitHandler;
            }

            if (_canvas != null)
                Object.Destroy(_canvas);
        }

        #endregion


        #region Methods

        private void FillPrompts()
        {
            _prompts.Add(InteractionPrompt.Use, _inputService.UseButton.Key);
            _prompts.Add(InteractionPrompt.Inventory, _inputService.InventoryButton.Key);
            _prompts.Add(InteractionPrompt.Notepad, _inputService.NotepadButton.Key);
            _prompts.Add(InteractionPrompt.PickUp, _inputService.PickUpButton.Key);
            _prompts.Add(InteractionPrompt.HorizontalAxisPositive, _inputService.HorizontalAxis.Positive.Key);
            _prompts.Add(InteractionPrompt.HorizontalAxisNegative, _inputService.HorizontalAxis.Negative.Key);
            _prompts.Add(InteractionPrompt.VerticalAxisPositive, _inputService.VerticalAxis.Positive.Key);
            _prompts.Add(InteractionPrompt.VerticalAxisNegative, _inputService.VerticalAxis.Negative.Key);
        }

        private void OnTriggerEnterHandler(ITrigger enteredObject)
        {
            var interactable = enteredObject as InteractableObjectBehavior;

            if (interactable != null)
                ShowPrompt(interactable);
        }

        private void OnTriggerExitHandler(ITrigger enteredObject)
        {
            var interactable = enteredObject as InteractableObjectBehavior;

            if (interactable != null)
                HidePrompt(interactable);
        }

        private void ShowPrompt(InteractableObjectBehavior interactable)
        {
            if (interactable.IsInteractionLocked)
                return;

            if (_canvas == null)
            {
                _canvas = Object.Instantiate(Resources.Load<Canvas>(_promptCanvasPrefabPath).gameObject).GetComponent<Canvas>();
            }

            if (_availablePrompts.Count == 0)
                CreateNewPrompt();

            var prompt = _availablePrompts.Pop();

            if (!_prompts.ContainsKey(interactable.InteractionPrompt))
                return;

            prompt.SetText(_prompts[interactable.InteractionPrompt].ToString());
            prompt.transform.position = interactable.transform.position + _offset;
            prompt.gameObject.SetActive(true);

            _activePrompts.Add(interactable, prompt);
        }

        private void HidePrompt(InteractableObjectBehavior interactable)
        {
            if (!_activePrompts.ContainsKey(interactable))
                return;

            var prompt = _activePrompts[interactable];
            _activePrompts.Remove(interactable);
            prompt.gameObject.SetActive(false);

            _availablePrompts.Push(prompt);
        }

        private void CreateNewPrompt()
        {
            var prompt = Object.Instantiate(_promptPrefab.gameObject, _canvas.transform).GetComponent<InputPromptView>();
            prompt.gameObject.SetActive(false);
            _availablePrompts.Push(prompt);
        }

        #endregion
    }
}