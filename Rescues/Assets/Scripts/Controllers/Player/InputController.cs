using System;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Rescues
{
    public sealed class InputController : IExecuteController
    {
        #region Fields

        public EventSystem eventSystem;

        private readonly GameContext _context;
        private readonly CameraServices _cameraServices;
        private readonly PhysicalServices _physicsService;
        private PlayerStates _playerState;
        private PlayerStates _lastState;
        private GameObject _interfaceWindow;
        private Action _cancelState = () => { };
        private Vector2 _inputAxis;
        private bool _isStateLocked;

        #endregion


        #region ClassLifeCycles

        public InputController(GameContext context, Services services)
        {
            _context = context;
            _cameraServices = services.CameraServices;
            _physicsService = services.PhysicalServices;
        }

        #endregion


        #region IExecuteController

        public void Execute()
        {
            if (Input.GetButtonUp("Cancel"))
            {
                var puzzleObject = GetInteractableObject<PuzzleBehaviour>(InteractableObjectType.Puzzle);
                if (puzzleObject != null && puzzleObject.Puzzle.IsActive)
                {
                    puzzleObject.Puzzle.Close();
                }
                else
                {
                    _context.gameMenu.SwitchState();
                }
            }

            _inputAxis.x = Input.GetAxis("Horizontal");
            _inputAxis.y = Input.GetAxis("Vertical");

            if (_physicsService.IsPaused == false && _isStateLocked == false)
            {
                _context.character.OnSceneLoad(_inputAxis.x);

                if (Input.GetButtonUp("Vertical"))
                {
                    _playerState = PlayerStates.GoByGateWay;
                    SwitchState();                   
                }

                if (Input.GetButtonUp("PickUp"))
                {
                    _playerState = PlayerStates.PickUp;
                    SwitchState();                  
                }

                if (Input.GetButtonUp("Inventory"))
                {
                    _playerState = PlayerStates.Inventory;
                    SwitchState();                    
                }

                if (Input.GetButtonUp("Notepad"))
                {
                    _playerState = PlayerStates.Notepad;
                    SwitchState();                   
                }

                if (Input.GetButtonUp("Use"))
                {
                    _playerState = PlayerStates.Use;
                    SwitchState();
                }

                if (Input.GetButtonDown("Mouse ScrollPressed"))
                {
                    _cameraServices.FreeCamera();
                }

                if (Input.GetButton("Mouse ScrollPressed"))
                {
                    _cameraServices.FreeCameraMovement();
                }

                if (Input.GetButtonUp("Mouse ScrollPressed"))
                {
                    _cameraServices.LockCamera();
                }
            }
        }

        #endregion


        #region Methods

        private T GetInteractableObject<T>(InteractableObjectType type) where T : class
        {
            var interactableObjects = _context.GetTriggers(type);
            T behaviour = default;

            foreach (var trigger in interactableObjects)
            {
                if (trigger.IsInteractable)
                {
                    behaviour = trigger as T;
                    break;
                }
            }

            return behaviour;
        }

        private void SwitchState()
        {
            _cancelState.Invoke();

            if (_playerState != _lastState)
            {
                switch (_playerState)
                {
                    case PlayerStates.GoByGateWay:
                        {
                            var gate = GetInteractableObject<Gate>(InteractableObjectType.Gate);
                            if (gate != null)
                            {
                                _isStateLocked = true;
                                TimeRemainingExtensions.AddTimeRemaining(new TimeRemaining(() =>
                                {
                                    gate.GoByGateWay();
                                    _isStateLocked = false;
                                },
                                gate.LocalTransferTime));
                            }

                            _cancelState = () => { };
                        }
                        break;

                    case PlayerStates.PickUp:
                        {
                            var trap = GetInteractableObject<TrapBehaviour>(InteractableObjectType.Trap);
                            if (trap != null)
                            {
                                if (_context.inventory.Contains(trap.TrapInfo.RequiredTrapItem))
                                {
                                    _isStateLocked = true;
                                    TimeRemainingExtensions.AddTimeRemaining(new TimeRemaining(() =>
                                    {
                                        trap.CreateTrap();
                                        _context.inventory.RemoveItem(trap.TrapInfo.RequiredTrapItem);
                                        _isStateLocked = false;
                                    },
                                    trap.TrapInfo.BaseTrapData.CraftingTime));
                                }
                            }

                            _cancelState = () => { };
                        }
                        break;

                    case PlayerStates.Inventory:
                        {
                            _context.inventory.gameObject.SetActive(true);

                            _cancelState = () =>
                            {
                                _context.inventory.gameObject.SetActive(false);
                            };
                        }
                        break;

                    case PlayerStates.Notepad:
                        {
                            _context.notepad.gameObject.SetActive(true);

                            _cancelState = () =>
                            {
                                _context.notepad.gameObject.SetActive(false);
                            };
                        }
                        break;

                    case PlayerStates.Use:
                        {
                            var item = GetInteractableObject<ItemBehaviour>(InteractableObjectType.Item);
                            if (item != null)
                            {
                                _isStateLocked = true;
                                TimeRemainingExtensions.AddTimeRemaining(new TimeRemaining(() =>
                                {
                                    item.gameObject.SetActive(false);
                                    _context.inventory.AddItem(item.ItemData);
                                    _isStateLocked = false;
                                },
                                item.PickUpTime));
                            }

                            var puzzleObject = GetInteractableObject<PuzzleBehaviour>(InteractableObjectType.Puzzle);
                            if (puzzleObject != null && !puzzleObject.Puzzle.IsFinished && !puzzleObject.Puzzle.IsActive)
                            {
                                puzzleObject.Puzzle.Activate();
                            }

                            var hidingPlace = GetInteractableObject<HidingPlaceBehaviour>(InteractableObjectType.HidingPlace);
                            if (hidingPlace != null)
                            {
                                _context.character.StartHiding(hidingPlace);
                            }

                            _cancelState = () => { };

                            var stand = GetInteractableObject<StandBehaviour>(InteractableObjectType.Stand);
                            if (stand != null)
                            {
                                stand.StandWindow.SetActive(true);
                                _cancelState = () =>
                                {
                                    stand.StandWindow.SetActive(false);
                                };
                            }                           
                        }
                        break; 
                }
                _lastState = _playerState;
            }
            else
            {
                _lastState = default;
            }
        }

        #endregion
    }
}

