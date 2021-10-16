using UnityEngine;


namespace Rescues
{
    public sealed class MainPuzzleController : IInitializeController, ITearDownController, IExecuteController
    {
        #region Fields

        private readonly GameContext _context;
        private readonly PhysicalServices _physicalServices;
        private PuzzlesControllers _puzzleControllers;

        #endregion


        #region ClassLifeCycles

        public MainPuzzleController(GameContext context, Services services)
        {
            _context = context;
            _physicalServices = services.PhysicalServices;
            _puzzleControllers = new PuzzlesControllers();
        }      

        #endregion


        #region IInitializeController

        public void Initialize()
        {
            var puzzleInteracts = _context.GetTriggers(InteractableObjectType.Puzzle);
            var mainPuzzleParent = new GameObject("Puzzles");            

            foreach (var trigger in puzzleInteracts)
            {
                var puzzleBehaviour = trigger as PuzzleBehaviour;
                puzzleBehaviour.OnFilterHandler += OnFilterHandler;
                puzzleBehaviour.OnTriggerEnterHandler += OnTriggerEnterHandler;
                puzzleBehaviour.OnTriggerExitHandler += OnTriggerExitHandler;

                foreach (var somePuzzleController in _puzzleControllers.ControllersList)
                {
                    if (somePuzzleController.Value == puzzleBehaviour.Puzzle.GetType())
                    {
                        var puzzleInstance = GameObject.Instantiate(puzzleBehaviour.Puzzle, mainPuzzleParent.transform);
                        
                        puzzleBehaviour.Puzzle = puzzleInstance;
                        somePuzzleController.Key.Initialize(puzzleInstance);
                        puzzleInstance.Closed += puzzle => _physicalServices.UnPause();
                        puzzleInstance.Activated += puzzle => _physicalServices.Pause();
                        foreach (var customEvent in puzzleBehaviour.finishEvents)
                        {
                            puzzleInstance.Finished += puzzle => customEvent.Event.Invoke();
                        }
                    }
                }
            }
        }

        #endregion


        #region ITearDownController

        public void TearDown()
        {
            var puzzles = _context.GetTriggers(InteractableObjectType.Puzzle);
            foreach (var trigger in puzzles)
            {
                var puzzleBehaviour = trigger as PuzzleBehaviour;
                puzzleBehaviour.OnFilterHandler -= OnFilterHandler;
                puzzleBehaviour.OnTriggerEnterHandler -= OnTriggerEnterHandler;
                puzzleBehaviour.OnTriggerExitHandler -= OnTriggerExitHandler;
            }
        }

        #endregion


        #region IExecuteController

        public void Execute()
        {
            foreach (var controller in _puzzleControllers.ControllersList)
            {
                var temp = controller.Key as IExecuteController;
                temp?.Execute();
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
            enteredObject.IsInteractable = true;
        }

        private void OnTriggerExitHandler(ITrigger enteredObject)
        {
            enteredObject.IsInteractable = false;
        }

        #endregion
    }
}