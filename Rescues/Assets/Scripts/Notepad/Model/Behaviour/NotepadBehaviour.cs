using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace Rescues
{
    public sealed class NotepadBehaviour : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Button _leftButton;
        [SerializeField] private Button _rightButton;
        [SerializeField] private Button _questsBookmark;
        [SerializeField] private Button _dialoguesBookmark;
        [SerializeField] private Button _charactersBookmark;
        [SerializeField] private Button _loreBookmark;
        [SerializeField] private TextMeshProUGUI _notepadText;

        private NotepadTextContent _notepadTextContent;
        private NotepadEntriesHolder _notepadEntriesHolder;

        private NoteCategory _lastViewedCategory;

        #endregion


        #region Methods

        public void Initialize()
        {
            _notepadTextContent = new NotepadTextContent();
            _notepadEntriesHolder = new NotepadEntriesHolder();

            _questsBookmark.onClick.AddListener(() => DisplayText(NoteCategory.Quest));
            _dialoguesBookmark.onClick.AddListener(() => DisplayText(NoteCategory.Dialogue));
            _charactersBookmark.onClick.AddListener(() => DisplayText(NoteCategory.Character));
            _loreBookmark.onClick.AddListener(() => DisplayText(NoteCategory.Lore));

            _leftButton.onClick.AddListener(() => TurnThePage(false));
            _rightButton.onClick.AddListener(() => TurnThePage(true));
        }

        public void ProcessTrigger(NotepadTrigger entry)
        {
            switch (entry.Action)
            {
                case NotepadEntryAction.Add:
                    {
                        _notepadEntriesHolder.AddEntry(entry.Category, entry.EntryID, entry.BulletpointID);
                        break;
                    }
                case NotepadEntryAction.CrossOut:
                    {
                        _notepadEntriesHolder.CrossOutEntry(entry.Category, entry.EntryID, entry.BulletpointID);
                        break;
                    }
                case NotepadEntryAction.Remove:
                    {
                        _notepadEntriesHolder.RemoveEntry(entry.Category, entry.EntryID, entry.BulletpointID);
                        break;
                    }
                default:
                    {
                        throw new System.Exception("Each NotepadTrigger in NotepadTriggerBehaviour component must have value assigned to its Category field");
                    }
            }
        }

        private void DisplayText(NoteCategory category)
        {
            if (category == NoteCategory.None)
                category = NoteCategory.Quest;

            _notepadText.text = _notepadTextContent.GetTextToDisplay(category, 
                                                                    _notepadEntriesHolder.GetEntries(category));
            _notepadText.pageToDisplay = 1;

            _lastViewedCategory = category;
        }

        public void ShowPageButtons()
        {
            var isManyPages = _notepadText.textInfo.pageCount > 1;

            _leftButton.gameObject.SetActive(isManyPages);
            _rightButton.gameObject.SetActive(isManyPages);
        }

        private void TurnThePage(bool doIncrease)
        {
            if (doIncrease && _notepadText.pageToDisplay < _notepadText.textInfo.pageCount)
                _notepadText.pageToDisplay++;

            if (!doIncrease && _notepadText.pageToDisplay > 1)
                _notepadText.pageToDisplay--;
        }

        #endregion


        #region UnityMethods

        private void OnEnable()
        {
            DisplayText(_lastViewedCategory);
        }

        private void OnDestroy()
        {
            _questsBookmark.onClick.RemoveAllListeners();
            _dialoguesBookmark.onClick.RemoveAllListeners();
            _charactersBookmark.onClick.RemoveAllListeners();
            _loreBookmark.onClick.RemoveAllListeners();

            _leftButton.onClick.RemoveAllListeners();
            _rightButton.onClick.RemoveAllListeners();
        }

        #endregion
    }
}
