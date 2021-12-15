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
        private const string OVERRIDE_COMMAND = "override";
        private const string GOTO_COMMAND = "goto";
        private const float DIMMING_FACTOR = 0.7f;

        private readonly GameContext _context;
        private readonly UnityTimeServices _timeServices;

        private DialogueUI _dialogueUI;
        private float _timeForWriteChar;
        private float _timeBeforeNextNode;
        private int _writeStep;
        private List<ITimeRemaining> _sequence;
        private List<ITimeRemaining> _goNextTimeRemaining;
        private List<IInteractable> _items;
        private List<IInteractable> _dialogues;

        #endregion


        #region ClassLifeCycles

        public DialogueUIController(GameContext context, Services services)
        {
            _context = context;
            _timeServices = services.UnityTimeServices;
        }

        #endregion


        #region IInitializeController

        public void Initialize()
        {
            _items = _context.GetTriggers(InteractableObjectType.Item);
            _dialogues = _context.GetTriggers(InteractableObjectType.Dialogue);
            foreach (IInteractable dialogue in _dialogues)
            {
                InteractableObjectBehavior dialogueBehaviour = dialogue as InteractableObjectBehavior;
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
            _timeForWriteChar = _timeServices.DeltaTime() * 10 / _dialogueUI.writeSpeed;
            _writeStep = _dialogueUI.writeStep;
            _sequence = new List<ITimeRemaining>();
            _context.dialogueUIController = this;

            VD.LoadDialogues();
        }

        #endregion


        #region ITearDownController

        public void TearDown()
        {
            List<IInteractable> dialogues = _context.GetTriggers(InteractableObjectType.Dialogue);
            foreach (IInteractable trigger in dialogues)
            {
                InteractableObjectBehavior dialogueBehaviour = trigger as InteractableObjectBehavior;
                dialogueBehaviour.OnFilterHandler -= OnFilterHandler;
                dialogueBehaviour.OnTriggerEnterHandler -= OnTriggerEnterHandler;
                dialogueBehaviour.OnTriggerExitHandler -= OnTriggerExitHandler;
            }
        }

        #endregion


        #region IExecuteController

        public void Execute()
        {
            if (VD.isActive)
            {
                if (Input.GetButtonUp("Fire1") && _sequence.ContainsSequentialTimeRemaining() &&
                    _dialogueUI.npcText.text != "")
                {
                    CutTextAnimation();
                }

                if (VD.nodeData.isPlayer)
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
        }

        #endregion


        #region Methods

        public void Begin(VIDE_Assign dialogue)
        {
            InitialPrefferences(dialogue);

            VD.OnNodeChange += SetStartNodeByDialogueID;
            VD.OnNodeChange += SetName;
            VD.OnNodeChange += PlayNodeSound;
            VD.OnNodeChange += GivePlayerItem;
            VD.OnNodeChange += RemovePlayerItem;
            VD.OnNodeChange += SetBackground;
            VD.OnNodeChange += SwitchNpcContainerState;
            VD.OnNodeChange += SwitchInteractionLock;
            VD.OnNodeChange += SwitchEventLock;
            VD.OnNodeChange += UpdateUI;
            VD.OnNodeChange += CheckItemAndChangeNode;

            VD.OnEnd += End;

            VD.BeginDialogue(dialogue);
        }

        public void End(VD.NodeData data)
        {
            VD.OnNodeChange -= SetStartNodeByDialogueID;
            VD.OnNodeChange -= SetName;
            VD.OnNodeChange -= PlayNodeSound;
            VD.OnNodeChange -= GivePlayerItem;
            VD.OnNodeChange -= RemovePlayerItem;
            VD.OnNodeChange -= SetBackground;
            VD.OnNodeChange -= SwitchNpcContainerState;
            VD.OnNodeChange -= SwitchInteractionLock;
            VD.OnNodeChange -= SwitchEventLock;
            VD.OnNodeChange -= UpdateUI;
            VD.OnNodeChange -= CheckItemAndChangeNode;

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

                foreach (PossibleAnswer choise in _dialogueUI.playerTextChoices)
                {
                    choise.Disable();
                }

                DrawText(data.comments[data.commentIndex], _timeForWriteChar);
                AddAutoSkip();
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

        private void CutTextAnimation()
        {
            if (VD.nodeData.isPlayer == false)
            {
                _sequence.RemoveSequentialTimeRemaining();
                _dialogueUI.npcText.text = VD.nodeData.comments[VD.nodeData.commentIndex];
            }
        }

        private void SetStartNodeByDialogueID(VD.NodeData data)
        {
            if (data.extraVars.ContainsKey(DialogueCommandValue.Command[DialogueCommands.SetStartNode]))
            {
                int[] tempArray = VD.ToIntArray(data.extraVars[DialogueCommandValue.Command[DialogueCommands.
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
                List<IInteractable> tempCollection = _context.GetListInteractable();
                string[] commandValues = VD.ToStringArray(data.extraVars[DialogueCommandValue.Command[DialogueCommands.
                        ActivateObject]].ToString());
                for (int i = 0; i < commandValues.Length; i++)
                {
                    foreach (InteractableObjectBehavior interactable in tempCollection)
                    {
                        if (interactable.Id == commandValues[i])
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
                List<EventSystemBehaviour> tempCollection = _context.GetTriggers<EventSystemBehaviour>
                    (InteractableObjectType.EventSystem);
                string[] commandValues = VD.ToStringArray(data.extraVars[DialogueCommandValue.
                            Command[DialogueCommands.ActivateEvent]].ToString());
                foreach (EventSystemBehaviour eventSystem in tempCollection)
                {
                    eventSystem.LockEventsByIDs(commandValues);
                }
            }
        }

        private void CheckItemAndChangeNode(VD.NodeData data)
        {
            string command = DialogueCommandValue.Command[DialogueCommands.CheckItem];
            if (data.extraVars.ContainsKey(command))
            {
                bool isItemContains = false;
                string yesCommand = DialogueCommandValue.Command[DialogueCommands.Yes];
                string noCommand = DialogueCommandValue.Command[DialogueCommands.No];
                foreach (ItemSlot itemSlot in _context.inventory.itemSlots)
                {
                    if (itemSlot.Item?.itemID== data.extraVars[command].ToString())
                    {
                        isItemContains = true;
                        break;
                    }
                }

                if (data.extraVars.ContainsKey(yesCommand) && isItemContains)
                {
                    string[] yesStrings = VD.ToStringArray(data.extraVars[yesCommand].ToString());
                    _goNextTimeRemaining.RemoveSequentialTimeRemaining();
                    ChangeNode(yesStrings);
                }

                if (data.extraVars.ContainsKey(noCommand) && !isItemContains)
                {
                    string[] noStrings = VD.ToStringArray(data.extraVars[noCommand].ToString());
                    _goNextTimeRemaining.RemoveSequentialTimeRemaining();
                    ChangeNode(noStrings);
                }
            }
        }

        private void ChangeNode(string[] data)
        {
            switch (data[0])
            {
                case (OVERRIDE_COMMAND):
                    {
                        int.TryParse(data[1], out VD.assigned.overrideStartNode);
                        break;
                    }
                case (GOTO_COMMAND):
                    {
                        int.TryParse(data[1], out int nodeID);
                        _timeBeforeNextNode = _dialogueUI.timeBeforeNextNode;
                        foreach (var node in VD.saved[VD.currentDiag].playerNodes)
                        {
                            if (node.ID == nodeID && node.isPlayer)
                            {
                                _timeBeforeNextNode = 0;
                                break;
                            }
                        }

                        List<ITimeRemaining> goToTimeRemaining = new List<ITimeRemaining>(1)
                        {
                            new TimeRemaining(() => { VD.SetNode(nodeID); }, _timeBeforeNextNode)
                        };
                        goToTimeRemaining.AddSequentialTimeRemaining();

                        break;
                    }
                default:
                    {
                        Debug.Log("Invalid checkItem mode");
                        break;
                    }
            }
        }

        private void SetBackground(VD.NodeData data)
        {
            Sprite dataSprite = data.sprite;
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
                    int temp = i;
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
                string[] commandValues = VD.ToStringArray(data.
                        extraVars[DialogueCommandValue.Command[DialogueCommands.GiveItem]].ToString());
                for (int i = 0; i < commandValues.Length; i++)
                {
                    foreach (ItemBehaviour item in _items)
                    {
                        if (item.Id == commandValues[i])
                        {
                            item.gameObject.SetActive(false);
                            _context.inventory.AddItem(item.ItemData);
                            break;
                        }
                    }
                }
            }
        }

        private void RemovePlayerItem(VD.NodeData data)
        {
            if (data.extraVars.ContainsKey(DialogueCommandValue.Command[DialogueCommands.RemoveItem]))
            {
                string[] commandValues = VD.ToStringArray(data.
                        extraVars[DialogueCommandValue.Command[DialogueCommands.RemoveItem]].ToString());
                for (int i = 0; i < commandValues.Length; i++)
                {
                    _context.inventory.RemoveItem(commandValues[i]);
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
                string path = ($"{AssetsPathGameObject.Object[GameObjectType.DialoguesComponents]}Audio/" +
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

                string tempSubstring = text.Substring(start, tempStep);
                _sequence.Add(new TimeRemaining(() =>
                {
                    _dialogueUI.npcText.text += tempSubstring;
                },
                time));

                start += tempStep;
            }

            _sequence.AddSequentialTimeRemaining();
        }

        private void AddAutoSkip()
        {
            if (VD.GetNext(false, false).isPlayer)
            {
                _timeBeforeNextNode = 0;
            }
            else
            {
                _timeBeforeNextNode = _dialogueUI.timeBeforeNextNode;
            }

            _goNextTimeRemaining = new List<ITimeRemaining>(1)
            {
                new TimeRemaining(() => { VD.Next(); }, _timeBeforeNextNode)
            };
            _goNextTimeRemaining.AddSequentialTimeRemaining();
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
            if (enteredObject.GameObject.TryGetComponent(out SpriteRenderer spriteRenderer))
            {
                Color materialColor = spriteRenderer.color;
                enteredObject.GameObject.GetComponent<SpriteRenderer>().DOColor(new Color(materialColor.r,
                    materialColor.g * 1.2f, materialColor.b, 1f), 1.0f);
            }
        }

        private void OnTriggerExitHandler(ITrigger enteredObject)
        {
            enteredObject.IsInteractable = false;
            if (enteredObject.GameObject.TryGetComponent(out SpriteRenderer spriteRenderer))
            {
                Color materialColor = spriteRenderer.color;
                enteredObject.GameObject.GetComponent<SpriteRenderer>().DOColor(new Color(materialColor.r,
                    materialColor.g, materialColor.b, 1.0f), 1.0f);
            }
        }

        #endregion
    }
}
