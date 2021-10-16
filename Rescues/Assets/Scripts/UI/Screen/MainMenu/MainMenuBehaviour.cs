using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


namespace Rescues
{
    public sealed class MainMenuBehaviour : BaseUi
    {
        #region Fields
        
        [SerializeField] private Button _newGameButton;
        [SerializeField] private Button _loadButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _exitButton;

        #endregion


        #region UnityMethods

        private void OnEnable()
        {
            _newGameButton.onClick.AddListener(NewGameButtonClick);
            _loadButton.onClick.AddListener(LoadButtonClick);
            _settingsButton.onClick.AddListener(ShowSettingsButtonClick);
            _exitButton.onClick.AddListener(ExitButtonClick);
        }

        private void OnDisable()
        {
            _newGameButton.onClick.RemoveListener(NewGameButtonClick);
            _loadButton.onClick.RemoveListener(LoadButtonClick);
            _settingsButton.onClick.RemoveListener(ShowSettingsButtonClick);
            _exitButton.onClick.RemoveListener(ExitButtonClick);
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

        private void NewGameButtonClick()
        {
            SceneManager.LoadSceneAsync("HotelScene");
        }

        private void LoadButtonClick()
        {
            //TODO Need emplementation
            Debug.Log("Not emplemented yet");
            //ScreenInterface.GetInstance().Execute(ScreenType.Load);
        }

        private void ShowSettingsButtonClick()
        {
            //TODO Need emplementation
            Debug.Log("Not emplemented yet");
            //ScreenInterface.GetInstance().Execute(ScreenType.Settings);
        }

        private void ExitButtonClick()
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
