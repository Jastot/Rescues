using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


namespace Rescues
{
    public sealed class GameMenuBehaviour : BaseUi
    {
        #region Fields
        
        [SerializeField] private Button _resume;
        [SerializeField] private Button _save;
        [SerializeField] private Button _load;
        [SerializeField] private Button _settigns;
        [SerializeField] private Button _mainMenu;
        [SerializeField] private Button _exit;
        
        #endregion


        #region UnityMethods

        private void OnEnable()
        {
            _resume.onClick.AddListener(Resume);
            _save.onClick.AddListener(Save);
            _load.onClick.AddListener(Load);
            _settigns.onClick.AddListener(Settigns);
            _mainMenu.onClick.AddListener(ToMainMenu);
            _exit.onClick.AddListener(Exit);
        }

        private void OnDisable()
        {
            _resume.onClick.RemoveListener(Resume);
            _save.onClick.RemoveListener(Save);
            _load.onClick.RemoveListener(Load);
            _settigns.onClick.RemoveListener(Settigns);
            _mainMenu.onClick.RemoveListener(ToMainMenu);
            _exit.onClick.RemoveListener(Exit);
        }

        #endregion


        #region Methods

        public override void Show()
        {
            gameObject.SetActive(true);
            ShowUI.Invoke();
        }

        public override void Hide()
        {
            gameObject.SetActive(false);
            HideUI.Invoke();
        }
        
        public void SwitchState()
        {
            if (gameObject.activeSelf)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }

        private void Resume()
        {
            Hide();
        }

        private void Save()
        {
            //TODO Need emplementation
            Debug.Log("Not emplemented yet");
        }

        private void Load()
        {
            //TODO Need emplementation
            Debug.Log("Not emplemented yet");
        }

        private void Settigns()
        {
            //TODO Need emplementation
            Debug.Log("Not emplemented yet");
        }

        private void ToMainMenu()
        {
            SceneManager.LoadSceneAsync("MainMenu");
        }

        private void Exit()
        {
            #if (UNITY_EDITOR)
            {
                UnityEditor.EditorApplication.isPlaying = false;
            }
            #else
            {
                Application.Quit();
            }
            #endif
        }

        #endregion
    }
}
