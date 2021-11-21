using System;
using UnityEngine;
using UnityEngine.EventSystems;
using VIDE_Data;


namespace Rescues
{
    public sealed class InputController : IExecuteController
    {
        #region Fields

        public EventSystem eventSystem;

        private readonly GameContext _context;
        private readonly CameraServices _cameraServices;
        private readonly PhysicalServices _physicsService;
        private PlayerStates _lastState;
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
                if (_lastState != PlayerStates.Idle && _lastState != PlayerStates.Moving)
                {
                    _isStateLocked = false;
                    SwitchState(PlayerStates.Idle);
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
                if (_inputAxis.x != 0)
                {
                    _context.character.Move(_inputAxis.x);
                    _lastState = PlayerStates.Moving;
                    _cancelState.Invoke();
                    _cancelState = () => { };
                }
                else
                {
                    _context.character.SetIdle();
                    if (Input.GetButtonUp("Vertical"))
                    {
                        SwitchState(PlayerStates.GoByGateWay);
                    }

                    if (Input.GetButtonUp("PickUp"))
                    {
                        SwitchState(PlayerStates.PickUp);
                    }

                    if (Input.GetButtonUp("Inventory"))
                    {
                        SwitchState(PlayerStates.Inventory);
                    }

                    if (Input.GetButtonUp("Notepad"))
                    {
                        SwitchState(PlayerStates.Notepad);
                    }

                    if (Input.GetButtonUp("Use"))
                    {
                        SwitchState(PlayerStates.Use);
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
        }

        #endregion


        #region Methods

        private void LockState()
        {
            _context.character.ForceSetIdle();
            _isStateLocked = true;
        }

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

        private void SwitchState(PlayerStates playerState)
        {
            _cancelState.Invoke();

            switch (playerState)
            {
                case PlayerStates.Use:
                    {
                        _cancelState = () => { };

                        var dialogue = GetInteractableObject<DialogueBehaviour>(InteractableObjectType.Dialogue);
                        if (dialogue != null)
                        {
                            _context.dialogueUIController.Begin(dialogue.assignDialog);
                            VD.OnEnd += (data) => _isStateLocked = false;
                            _cancelState += () =>
                            {
                                _context.dialogueUIController.End(new VD.NodeData());
                                VD.OnEnd -= (data) => _isStateLocked = false;
                            };
                            LockState();
                        }

                        var item = GetInteractableObject<ItemBehaviour>(InteractableObjectType.Item);
                        if (item != null)
                        {
                            LockState();
                            //TODO Need animation for this
                            TimeRemainingExtensions.AddTimeRemaining(new TimeRemaining(() =>
                            {
                                item.gameObject.SetActive(false);
                                _context.inventory.AddItem(item.ItemData);
                                _context.WorldGameData.SaveItem(new ItemListData()
                                    {Name = item.ItemData.Name,
                                        ItemCondition = (ItemCondition)2});
                                _isStateLocked = false;
                            },
                            item.PickUpTime));
                        }

                        var puzzleObject = GetInteractableObject<PuzzleBehaviour>(InteractableObjectType.Puzzle);
                        if (puzzleObject != null && !puzzleObject.Puzzle.IsFinished && !puzzleObject.Puzzle.IsActive)
                        {
                            puzzleObject.Puzzle.Activate();
                            //Intercept control
                            LockState();
                            _cancelState += puzzleObject.Puzzle.Close;
                        }
                    }
                    break;

                case PlayerStates.PickUp:
                    {
                        var trap = GetInteractableObject<TrapBehaviour>(InteractableObjectType.Trap);
                        if (trap != null)
                        {
                            if (_context.inventory.Contains(trap.TrapInfo.RequiredTrapItem))
                            {
                                LockState();
                                //TODO Need animation for this
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

                case PlayerStates.GoByGateWay:
                    {
                        var gate = GetInteractableObject<Gate>(InteractableObjectType.Gate);
                        if (gate != null)
                        {
                            LockState();
                            //TODO Need animation for this
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
            }

            #region CloseOnSecondClick

            if (playerState != _lastState)
            {
                _lastState = playerState;
                switch (playerState)
                {
                    case PlayerStates.Use:
                        {
                            //TODO not implemented
                            var hidingPlace = GetInteractableObject<HidingPlaceBehaviour>(InteractableObjectType.HidingPlace);
                            if (hidingPlace != null)
                            {
                                _context.character.StartHiding(hidingPlace);
                            }

                            //TODO not working yet
                            var stand = GetInteractableObject<StandBehaviour>(InteractableObjectType.Stand);
                            if (stand != null)
                            {
                                stand.StandWindow.SetActive(true);
                                _cancelState += () =>
                                {
                                    stand.StandWindow.SetActive(false);
                                };
                            }
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
                }
            }
            else
            {
                _lastState = PlayerStates.Idle;
            }

            #endregion
        }

        #endregion
    }
}

