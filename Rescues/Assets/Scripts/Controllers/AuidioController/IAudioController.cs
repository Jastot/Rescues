namespace Rescues
{
    public interface IAudioController : IMusicVolumeSetter, IEffectsVolumeSetter, IVoiceVolumeSetter
    {
        void SetMasterVolume(float value);

        void SetUiSoundsVolume(float value);

        void SetStartVolumeLevel(float value);

        void MuteAllSounds();
    }
}
