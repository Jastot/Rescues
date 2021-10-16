using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Rescues
{
    public sealed class NotepadBehaviour : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Button _leftButton;
        [SerializeField] private Button _rightButton;
        [SerializeField] private List<Text> _questTexts;

        private byte _currentQuestIndex;

        #endregion

        #region Methods

        public void AddQuest(string questText)
        {
            if (GetIndexOfQuest(questText) != _currentQuestIndex)
            {
                return;
            }

            if (_currentQuestIndex == _questTexts.Count - 1)
            {
                _questTexts.Add(_questTexts[_currentQuestIndex]);
            }
            _questTexts[_currentQuestIndex].text = questText;
            _currentQuestIndex++;
        }

        public void RemoveQuest(string questText)
        {
            byte removedIndex = GetIndexOfQuest(questText);
            _currentQuestIndex--;

            for (int i = removedIndex; i < _currentQuestIndex; i++)
            {
                _questTexts[i] = _questTexts[i + 1];
            }
            _questTexts[_currentQuestIndex].text = "";
        }

        /// <summary>
        /// By default return currentQuestIndex
        /// </summary>
        /// <param name="questText"></param>
        /// <returns></returns>
        private byte GetIndexOfQuest(string questText)
        {
            byte index = _currentQuestIndex;
            for (int i = 0; i < _currentQuestIndex; i++)
            {
                if (_questTexts[i].text == questText)
                {
                    index = (byte)i;
                    break;
                }
            }
            return index;
        }

        #endregion
    }
}
