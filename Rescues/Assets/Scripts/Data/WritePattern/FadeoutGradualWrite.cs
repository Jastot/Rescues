using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace Rescues
{
    [CreateAssetMenu(fileName = "FadeoutGradualWrite", menuName = "Data/WritePatterns/FadeoutGradualWrite")]
    public sealed class FadeoutGradualWrite : WritePattern
    {
        #region Fields

        public const int WRITE_SPEED_MAX_RANGE = 10;
        public const int WRITE_STEP_MAX_RANGE = 10;

        [Range(1, WRITE_STEP_MAX_RANGE)]
        [SerializeField] private int _writeStep;
        [Range(1, WRITE_SPEED_MAX_RANGE)]
        [SerializeField] private int _writeSpeed;
        [SerializeField] private float _timeToFadeout;
        [SerializeField] private float _timeBeforeFadeoutStarts;

        private readonly List<ITimeRemaining> _sequence = new List<ITimeRemaining>();

        #endregion


        #region Methods

        public override void DrawText(string inputText, TextMeshProUGUI outputTextContainer)
        {
            _sequence.Clear();
            DOTween.Clear();
            outputTextContainer.DOFade(1, 0);

            float _timeForWriteChar = Time.deltaTime * WRITE_SPEED_MAX_RANGE / _writeSpeed;
            int start = 0;
            int tempStep = _writeStep;
            while (start < inputText.Length)
            {
                if ((start + tempStep) >= inputText.Length)
                {
                    tempStep = inputText.Length - start;
                }

                string tempSubstring = inputText.Substring(start, tempStep);
                _sequence.Add(new TimeRemaining(() =>
                {
                    outputTextContainer.text += tempSubstring;
                },
                _timeForWriteChar));

                start += tempStep;
            }

            if (_sequence.Count > 0)
            {
                Fade(outputTextContainer);
                _sequence.AddSequentialTimeRemaining();
            }
        }

        public override void ClearText(TextMeshProUGUI textContainer)
        {
            _sequence.RemoveSequentialTimeRemaining();
            textContainer.text = string.Empty;
        }

        private void Fade(TextMeshProUGUI outputTextContainer)
        {
            _sequence.Add(new TimeRemaining(() =>
            {
                outputTextContainer.DOFade(0, _timeToFadeout).
                OnComplete(() => outputTextContainer.text = string.Empty);
            },
            _timeBeforeFadeoutStarts));
        }

        #endregion
    }
}