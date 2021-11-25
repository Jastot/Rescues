using UnityEngine;


namespace Rescues
{
    public sealed class AudioModel
    {
        #region CONSTANT

        private const string EFFECTS_VOLUME = "EffectsVolume";
        private const string MUSIC_VOLUME = "MusicVolume";
        private const string MASTER_VOLUME = "MasterVolume";
        private const string VOICE_VOLUME = "VoiceVolume";
        private const string UISOUNDS_VOLUME = "UiSoundsVolume";

        #endregion


        #region Fields

        private float _minVolume = -80;
        private float _maxVolume = 0;
        private float _currentValue;
        private bool _isMuted;

        public string EffectsVolume => EFFECTS_VOLUME;
        public string MusicVolume => MUSIC_VOLUME;
        public string MasterVolume => MASTER_VOLUME;
        public string VoiceVolume => VOICE_VOLUME;
        public string UiSoundsVolume => UISOUNDS_VOLUME;

        #endregion


        #region Methods

        public void SetCurrentValue(float value) =>
            _currentValue = value;

        public float Mute()
        {
            _isMuted = !_isMuted;
            if (_isMuted)
                return _minVolume;
            else
                return _currentValue;
        }

        public float CheckValue(float value)
        {
            var translatedNumber = (Mathf.Abs(_minVolume) + Mathf.Abs(_maxVolume)) * value + _minVolume;
            if (translatedNumber < _minVolume)
                return _minVolume;
            if (translatedNumber > _maxVolume)
                return _maxVolume;

            return translatedNumber;
        } 

        #endregion
    }
}
