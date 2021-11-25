using System;
using UnityEngine;


namespace Rescues
{
    [CreateAssetMenu(fileName = nameof(AudioControllerContext), menuName = "Data/Audio/" + nameof(AudioControllerContext), order = 0)]
    public sealed class AudioControllerContext : ScriptableObject
    {
        #region Fields

        private AudioController _testAudioController; 

        #endregion


        #region Methods

        public AudioController GetObjectOfType(Type targetType, string targetName = null)
        {
            if (_testAudioController == null)
            {
                throw new ApplicationException($"{nameof(AudioController)} is not initialized yet!!!");
            }
            var audioController = _testAudioController;
            if (targetType.Equals(audioController.GetType()))
            {
                throw new ApplicationException($"Please use {nameof(AudioController)}'s interfaces only!");
            }
            if (targetType.IsAssignableFrom(audioController.GetType()))
            {
                if (targetName == null)
                {
                    return audioController;
                }
            }
            throw new ApplicationException($"{targetType.Name} is not Assignable From {audioController.GetType().Name}");
        }

        public void BindAuidoController(AudioController testAudioController)
        {
            if (_testAudioController != null)
            {
                Debug.Log("Already Init!");
                return;
            }
            _testAudioController = testAudioController;
        } 

        #endregion
    }
}
