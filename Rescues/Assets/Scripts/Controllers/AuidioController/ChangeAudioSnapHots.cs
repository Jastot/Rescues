using UnityEngine;


namespace Rescues 
{
    public sealed class ChangeAudioSnapHots : MonoBehaviour
    {
        #region Fields

        private AudioControllerContext _audioContext;
        [InjectAudioInterfaces] private IAudioSnapsHots _audioSnapsHots; 

        #endregion


        #region UnityMethods

        private void Awake()
        {
            _audioContext = Resources.Load<AudioControllerContext>(
                AssetsPathGameObject.AudioData[AudioDataType.AudioControllerContext]);
            _audioContext.Inject(this);
        }

        private void OnEnable() =>
            _audioSnapsHots.MuffledMusic();

        private void OnDisable() =>
             _audioSnapsHots.UnMuffledMusic(); 

        #endregion
    }
}


