using UnityEngine;
using UnityEngine.Audio;


namespace Rescues
{
    [CreateAssetMenu(fileName = "AudioMixer", menuName = "Data/Audio/" + nameof(AudioMixerData))]
    public sealed class AudioMixerData : ScriptableObject
    {
        #region Fields

        [field: SerializeField] public AudioMixer Master { get; private set; }
        [field: SerializeField] public AudioMixerSnapshot MuffledMusic { get; private set; }
        [field: SerializeField] public AudioMixerSnapshot UnMuffledMusic { get; private set; }

        #endregion
    }
}

