using UnityEngine;


namespace Rescues
{
    public sealed class AudioController : IAudioController, IAudioSnapsHots
    {
        #region Fields

        private readonly AudioMixerData _mixer;
        private readonly AudioModel _audioModel;
        private readonly AudioControllerContext _context;

        #endregion


        #region ClassLifeCycles

        public AudioController()
        {
            _mixer = Resources.Load<AudioMixerData>(AssetsPathGameObject.AudioData[AudioDataType.AudioMixer]);
            _context = Resources.Load<AudioControllerContext>(AssetsPathGameObject.AudioData[AudioDataType.AudioControllerContext]);
            _audioModel = new AudioModel();
            _context.BindAuidoController(this);
            _mixer.Master.GetFloat(_audioModel.MasterVolume, out var currentValue);
            _audioModel.SetCurrentValue(currentValue);
        }

        #endregion


        #region Methods

        public void LoadMainMusicTheme(Transform levelParent)
        {
            var mainMusicTheme = Resources.Load(AssetsPathGameObject.AudioData[AudioDataType.HotelMusicTheme]);
            Object.Instantiate(mainMusicTheme, levelParent);
        }

        #endregion


        #region IAudioController

        public void SetMasterVolume(float value)
        {
            var checkedValue = _audioModel.CheckValue(value);
            _mixer.Master.SetFloat(
                _audioModel.MasterVolume,
                checkedValue);
        }

        public void MuteAllSounds() =>
            _mixer.Master.SetFloat(
                _audioModel.MasterVolume,
                _audioModel.Mute());

        public void SetStartVolumeLevel(float value) =>
            _mixer.Master.SetFloat(
                _audioModel.MasterVolume,
                _audioModel.CheckValue(value));

        public void SetEffectsVolume(float value) =>
            _mixer.Master.SetFloat(
                _audioModel.EffectsVolume,
                _audioModel.CheckValue(value));

        public void SetMusicVolume(float value) =>
             _mixer.Master.SetFloat(
                 _audioModel.MusicVolume,
                 _audioModel.CheckValue(value));

        public void SetUiSoundsVolume(float value) =>
             _mixer.Master.SetFloat(
                 _audioModel.UiSoundsVolume,
                 _audioModel.CheckValue(value));

        public void SetVoiceVolume(float value) =>
            _mixer.Master.SetFloat(
                 _audioModel.VoiceVolume,
                 _audioModel.CheckValue(value));

        #endregion


        #region IAudioSnapsHots

        public void MuffledMusic() =>
            _mixer.MuffledMusic.TransitionTo(Time.deltaTime);

        public void UnMuffledMusic() =>
            _mixer.UnMuffledMusic.TransitionTo(Time.deltaTime); 

        #endregion
    }
}
