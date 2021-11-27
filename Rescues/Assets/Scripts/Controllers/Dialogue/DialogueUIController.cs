using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using VIDE_Data;


namespace Rescues
{
    public sealed class DialogueUIController : IInitializeController, ITearDownController, IExecuteController
    {
        #region Fields

        private const string EXTRA_DATA_DEFAULT = "ExtraData";
        private const float DIMMING_FACTOR = 0.7f;
        private readonly GameContext _context;
        private readonly Services _services;
        private DialogueUI _dialogueUI;
        private float _timeForWriteChar;
        private int _writeStep;
        private List<ITimeRemaining> _sequence;
        private List<IInteractable> _items;
        private List<IInteractable> _dialogues;

        #endregion


        #region ClassLifeCycles

        public DialogueUIController(GameContext context, Services services)
        {
            _context = context;
            _services = services;
        }

        #endregion


        #region IInitializeController

        public void Initialize()
        {
            _items = _context.GetTriggers(InteractableObjectType.Item);
            _dialogues = _context.GetTriggers(InteractableObjectType.Dialogue);
            foreach (var dialogue in _dialogues)
            {
                var dialogueBehaviour = dialogue as InteractableObjectBehavior;
                dialogueBehaviour.OnFilterHandler += OnFilterHandler;
                dialogueBehaviour.OnTriggerEnterHandler += OnTriggerEnterHandler;
                dialogueBehaviour.OnTriggerExitHandler += OnTriggerExitHandler;
            }

            _dialogueUI = Object.FindObjectOfType<DialogueUI>(true);
            _dialogueUI.dialogContainer.SetActive(false);
            _dialogueUI.npcImage.color = _dialogueUI.npcImageNormalColor;
            _dialogueUI.playerImage.color = _dialogueUI.playerImageNormalColor;
            //Возможно расширение возможности и добавление команды в ExtraVars для дополнительного вызова
            SetNameColor();
            _timeForWriteChar = _services.UnityTimeServices.DeltaTime() * 10 / _dialogueUI.writeSpeed;
            _writeStep = _dialogueUI.writeStep;
            _sequence = new List<ITimeRemaining>();
            _context.dialogueUIController = this;

            VD.LoadDialogues();
        }

        #endregion


        #region ITearDownController

        public void TearDown()
        {
            var dialogues = _context.GetTriggers(InteractableObjectType.Dialogue);
            foreach (var trigger in dialogues)
            {
                var dialogueBehaviour = trigger as InteractableObjectBehavior;
                dialogueBehaviour.OnFilterHandler -= OnFilterHandler;
                dialogueBehaviour.OnTriggerEnterHandler -= OnTriggerEnterHandler;
                dialogueBehaviour.OnTriggerExitHandler -= OnTriggerExitHandler;
            }
        }

        #endregion


        #region IExecuteController

        public void Execute()
        {
            if (VD.isActive && !VD.nodeData.isPlayer && _dialogueUI.npcText.text != "" && Input.GetButtonUp("Fire1"))
            {
                CallNext();
            }

            if (VD.isActive && VD.nodeData.isPlayer)
            {
                if (Input.GetKeyUp(KeyCode.Alpha1))
                {
                    SetPlayerChoice(0);
                }

                if (Input.GetKeyUp(KeyCode.Alpha2))
                {
                    SetPlayerChoice(1);
                }

                if (Input.GetKeyUp(KeyCode.Alpha3))
                {
                    SetPlayerChoice(2);
                }

                if (Input.GetKeyUp(KeyCode.Alpha4))
                {
                    SetPlayerChoice(3);
                }

                if (Input.GetKeyUp(KeyCode.Alpha5))
                {
                    SetPlayerChoice(4);
                }
            }
        }

        #endregion


        #region Methods

        public void Begin(VIDE_Assign dialogue)
        {
            InitialPrefferences(dialogue);

            VD.OnNodeChange += UpdateUI;
            VD.OnNodeChange += SetName;
            VD.OnNodeChange += PlayNodeSound;
            VD.OnNodeChange += GivePlayerItem;
            VD.OnNodeChange += CheckItemAndOverrideStartNode;
            VD.OnNodeChange += SetBackground;
            VD.OnNodeChange += SetStartNode;
            VD.OnNodeChange += SwitchNpcContainerState;
            VD.OnNodeChange += SwitchInteractionLock;
            VD.OnNodeChange += SwitchEventLock;
            VD.OnEnd += End;

            VD.BeginDialogue(dialogue);
        }

        public void End(VD.NodeData data)
        {
            VD.OnNodeChange -= UpdateUI;
            VD.OnNodeChange -= SetName;
            VD.OnNodeChange -= PlayNodeSound;
            VD.OnNodeChange -= GivePlayerItem;
            VD.OnNodeChange -= CheckItemAndOverrideStartNode;
            VD.OnNodeChange -= SetBackground;
            VD.OnNodeChange -= SetStartNode;
            VD.OnNodeChange -= SwitchNpcContainerState;
            VD.OnNodeChange -= SwitchInteractionLock;
            VD.OnNodeChange -= SwitchEventLock;
            VD.OnEnd -= End;

            _dialogueUI.npcText.text = "";
            _dialogueUI.dialogContainer.SetActive(false);
            VD.EndDialogue();
        }

        private void UpdateUI(VD.NodeData data)
        {
            if (data.isPlayer)
            {
                if (_dialogueUI.npcImage.color == _dialogueUI.npcImageNormalColor)
                {
                    _dialogueUI.npcImage.color *= DIMMING_FACTOR;
                    _dialogueUI.playerImage.color = _dialogueUI.playerImageNormalColor;
                }
                SetPlayerChoices(data);
            }
            else
            {
                if (_dialogueUI.playerImage.color == _dialogueUI.playerImageNormalColor)
                {
                    _dialogueUI.playerImage.color *= DIMMING_FACTOR;
                    _dialogueUI.npcImage.color = _dialogueUI.npcImageNormalColor;
                }

                foreach (var choise in _dialogueUI.playerTextChoices)
                {
                    choise.Disable();
                }

                DrawText(data.comments[data.commentIndex], _timeForWriteChar);
            }
        }

        private void InitialPrefferences(VIDE_Assign dialogue)
        {
            _dialogueUI.playerImage.sprite = dialogue.defaultPlayerSprite;
            _dialogueUI.npcImage.sprite = dialogue.defaultNPCSprite;
            _dialogueUI.playerLabel.text = _context.character.Name;
            _dialogueUI.npcLabel.text = dialogue.alias;
            _dialogueUI.dialogContainer.SetActive(true);
            _dialogueUI.playerContainer.SetActive(true);
            _dialogueUI.npcContainer.SetActive(true);
            _dialogueUI.background.enabled = true;
            _dialogueUI.npcBackGround.enabled = false;
            _dialogueUI.playerBackground.enabled = false;
        }

        private void CallNext()
        {
            CutTextAnimation();
            VD.Next();
        }

        private void CutTextAnimation()
        {
            _sequence.RemoveSequentialTimeRemaining();
            _dialogueUI.npcText.text = VD.nodeData.comments[VD.nodeData.commentIndex];
        }

        private void SetStartNode(VD.NodeData data)
        {
            if (data.extraVars.ContainsKey(DialogueCommandValue.Command[DialogueCommands.SetStartNode]))
            {
                var tempArray = VD.ToIntArray(data.extraVars[DialogueCommandValue.Command[DialogueCommands.
                    SetStartNode]].ToString());
                foreach (DialogueBehaviour dialogue in _dialogues)
                {
                    if (dialogue.assignDialog.assignedID == tempArray[1])
                    {
                        dialogue.assignDialog.overrideStartNode = tempArray[0];
                        break;
                    }
                }
            }
        }

        private void SwitchInteractionLock(VD.NodeData data)
        {
            if (data.extraVars.ContainsKey(DialogueCommandValue.Command[DialogueCommands.ActivateObject]))
            {
                var tempCollection = _context.GetListInteractable();
                var commandValues = VD.ToStringArray(data.extraVars[DialogueCommandValue.Command[DialogueCommands.
                        ActivateObject]].ToString().ToLower());
                for (int i = 0; i < commandValues.Length; i++)
                {
                    foreach (InteractableObjectBehavior interactable in tempCollection)
                    {
                        if (interactable.Id.ToLower() == commandValues[i])
                        {
                            interactable.IsInteractionLocked = !interactable.IsInteractionLocked;
                            break;
                        }
                    } 
                }
            }
        }

        private void SwitchEventLock(VD.NodeData data)
        {
            if (data.extraVars.ContainsKey(DialogueCommandValue.Command[DialogueCommands.ActivateEvent]))
            {
                var tempCollection = _context.GetTriggers<EventSystemBehaviour>(InteractableObjectType.EventSystem);
                var commandValues = VD.ToStringArray(data.extraVars[DialogueCommandValue.
                            Command[DialogueCommands.ActivateEvent]].ToString().ToLower());
                for (int j = 0; j < commandValues.Length; j++)
                {
                    var breakToken = false;
                    foreach (EventSystemBehaviour eventSystem in tempCollection)
                    {
                        for (int i = 0; i < eventSystem.OnTriggerEnterEvents.Count; i++)
                        {
                            if (eventSystem.OnTriggerEnterEvents[i].Id.ToLower() == commandValues[j])
                            {
                                eventSystem.OnTriggerEnterEvents[i].IsInteractionLocked = !eventSystem.
                                    OnTriggerEnterEvents[i].IsInteractionLocked;
                                breakToken = true;
                                break;
                            }
                        }

                        if (breakToken)
                        {
                            break;
                        }

                        for (int i = 0; i < eventSystem.OnTriggerExitEvents.Count; i++)
                        {
                            if (eventSystem.OnTriggerExitEvents[i].Id.ToLower() == commandValues[j])
                            {
                                eventSystem.OnTriggerExitEvents[i].IsInteractionLocked = !eventSystem.
                                    OnTriggerExitEvents[i].IsInteractionLocked;
                                breakToken = true;
                                break;
                            }
                        }

                        if (breakToken)
                        {
                            break;
                        }

                        for (int i = 0; i < eventSystem.OnButtonInTriggerEvents.Count; i++)
                        {
                            if (eventSystem.OnButtonInTriggerEvents[i].Id.ToLower() == commandValues[j])
                            {
                                eventSystem.OnButtonInTriggerEvents[i].IsInteractionLocked = !eventSystem.
                                    OnButtonInTriggerEvents[i].IsInteractionLocked;
                                breakToken = true;
                                break;
                            }
                        }

                        if (breakToken)
                        {
                            break;
                        }
                    } 
                }
            }
        }

        private void CheckItemAndOverrideStartNode(VD.NodeData data)
        {
            if (data.extraVars.ContainsKey(DialogueCommandValue.Command[DialogueCommands.CheckItem]))
            {
                if (data.extraVars.ContainsKey(DialogueCommandValue.Command[DialogueCommands.Yes]))
                {
                    foreach (var itemSlot in _context.inventory.itemSlots)
                    {
                        if (itemSlot.Item?.Name.ToLower() == data.
                            extraVars[DialogueCommandValue.Command[DialogueCommands.CheckItem]].ToString().ToLower())
                        {
                            VD.assigned.overrideStartNode =
                                (int)data.extraVars[DialogueCommandValue.Command[DialogueCommands.Yes]];
                            return;
                        }
                    }
                }

                if (data.extraVars.ContainsKey(DialogueCommandValue.Command[DialogueCommands.No]))
                {
                    VD.assigned.overrideStartNode = (int)data.extraVars[DialogueCommandValue.
                        Command[DialogueCommands.No]];
                }
            }
        }

        private void SetBackground(VD.NodeData data)
        {
            var dataSprite = data.sprite;
            if (dataSprite)
            {
                _dialogueUI.background.sprite = dataSprite;
            }
        }

        private void SetNameColor()
        {
            _dialogueUI.playerLabel.color = _dialogueUI.playerLabelColor;
            _dialogueUI.npcLabel.color = _dialogueUI.npcLabelColor;
        }

        private void SetName(VD.NodeData data)
        {
            if (data.extraVars.ContainsKey(DialogueCommandValue.Command[DialogueCommands.SetNpcName]))
            {
                _dialogueUI.npcLabel.text = data.extraVars[DialogueCommandValue.Command[DialogueCommands.
                    SetNpcName]].ToString();
            }

            if (data.extraVars.ContainsKey(DialogueCommandValue.Command[DialogueCommands.SetPlayerName]))
            {
                _dialogueUI.playerLabel.text = data.extraVars[DialogueCommandValue.
                    Command[DialogueCommands.SetPlayerName]].ToString();
            }
        }

        private void SetSprite(VD.NodeData data, int choise)
        {
            if (data.creferences[choise].sprites != null && data.creferences[choise].extraData != EXTRA_DATA_DEFAULT)
            {
                if (data.creferences[choise].extraData == DialogueCommandValue.Command[DialogueCommands.SetNpcSprite])
                {
                    _dialogueUI.npcImage.sprite = data.creferences[choise].sprites;
                }

                if (data.creferences[choise].extraData == DialogueCommandValue.
                    Command[DialogueCommands.SetPlayerSprite])
                {
                    _dialogueUI.playerImage.sprite = data.creferences[choise].sprites;
                }
            }
        }

        private void SetPlayerChoices(VD.NodeData data)
        {
            for (int i = 0; i < _dialogueUI.playerTextChoices.Length; i++)
            {
                _dialogueUI.playerTextChoices[i].RemoveAllListeners();
                if (i < data.comments.Length)
                {
                    _dialogueUI.playerTextChoices[i].Enable();
                    _dialogueUI.playerTextChoices[i].Text = data.comments[i];
                    var temp = i;
                    _dialogueUI.playerTextChoices[i].AddListener(() => SetPlayerChoice(temp));
                    _dialogueUI.playerTextChoices[i].AddListener(() => SetSprite(data, temp));
                }
                else
                {
                    _dialogueUI.playerTextChoices[i].Disable();
                }
            }
        }

        private void GivePlayerItem(VD.NodeData data)
        {
            if (data.extraVars.ContainsKey(DialogueCommandValue.Command[DialogueCommands.GiveItem]))
            {
                var commandValues = VD.ToStringArray(data.
                        extraVars[DialogueCommandValue.Command[DialogueCommands.GiveItem]].ToString().ToLower());
                for (int i = 0; i < commandValues.Length; i++)
                {
                    foreach (ItemBehaviour item in _items)
                    {
                        if (item.Id.ToLower() == commandValues[i])
                        {
                            item.gameObject.SetActive(false);
                            _context.inventory.AddItem(item.ItemData);
                            break;
                        }
                    } 
                }
            }
        }

        private void SwitchBackgroundMode()
        {
            if (_dialogueUI.background.enabled)
            {
                _dialogueUI.playerBackground.enabled = true;
                _dialogueUI.playerBackground.sprite = _dialogueUI.background.sprite;
                _dialogueUI.playerBackground.color = _dialogueUI.background.color;
                _dialogueUI.background.enabled = false;
            }
            else
            {
                _dialogueUI.background.enabled = true;
                _dialogueUI.background.sprite = _dialogueUI.playerBackground.sprite;
                _dialogueUI.background.color = _dialogueUI.playerBackground.color;
                _dialogueUI.playerBackground.enabled = false;
            }
        }

        private void SetPlayerChoice(int choice)
        {
            if (choice < VD.nodeData.comments.Length && choice >= 0)
            {
                VD.nodeData.commentIndex = choice;
                VD.Next();
            }
        }

        private void SwitchNpcContainerState(VD.NodeData data)
        {
            if (data.extraVars.ContainsKey(DialogueCommandValue.Command[DialogueCommands.SwitchNpcContainerState]))
            {
                _dialogueUI.npcContainer.SetActive(!_dialogueUI.npcContainer.activeSelf);
                SwitchBackgroundMode();
            }
        }

        private void PlayNodeSound(VD.NodeData data)
        {
            if (data.extraVars.ContainsKey(DialogueCommandValue.Command[DialogueCommands.PlayMusic]))
            {
                var path = ($"{AssetsPathGameObject.Object[GameObjectType.DialoguesComponents]}Audio/" +
                    $"{data.extraVars[DialogueCommandValue.Command[DialogueCommands.PlayMusic]]}");
                _dialogueUI.nodeSoundContainer.Initialization(Resources.Load<AudioClip>(path));

            }
        }

        private void DrawText(string text, float time)
        {
            _sequence.Clear();
            _dialogueUI.npcText.text = "";
            int start = 0;
            int tempStep = _writeStep;
            while (start < text.Length)
            {
                if ((start + tempStep) >= text.Length)
                {
                    tempStep = text.Length - start;
                }

                var tempSubstring = text.Substring(start, tempStep);
                _sequence.Add(new TimeRemaining(() =>
                {
                    _dialogueUI.npcText.text += tempSubstring;
                },
                time));

                start += tempStep;
            }

            _sequence.Add(new TimeRemaining(() => { VD.Next(); }, time));
            TimeRemainingExtensions.AddSequentialTimeRemaining(_sequence);
        }

        #endregion


        #region InteractableObjectMethods

        private bool OnFilterHandler(Collider2D obj)
        {
            return obj.CompareTag(TagManager.PLAYER);
        }

        private void OnTriggerEnterHandler(ITrigger enteredObject)
        {
            enteredObject.IsInteractable = true;
            var materialColor = enteredObject.GameObject.GetComponent<SpriteRenderer>().color;
            enteredObject.GameObject.GetComponent<SpriteRenderer>().DOColor(new Color(materialColor.r,
                materialColor.g * 1.2f, materialColor.b, 1f), 1.0f);
        }

        private void OnTriggerExitHandler(ITrigger enteredObject)
        {
            enteredObject.IsInteractable = false;
            var materialColor = enteredObject.GameObject.GetComponent<SpriteRenderer>().color;
            enteredObject.GameObject.GetComponent<SpriteRenderer>().DOColor(new Color(materialColor.r,
                materialColor.g, materialColor.b, 1.0f), 1.0f);
        }

        #endregion
    }
}
