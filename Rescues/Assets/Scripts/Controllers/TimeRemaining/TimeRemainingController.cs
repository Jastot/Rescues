using System.Collections.Generic;


namespace Rescues
{
    public sealed class TimeRemainingController : IExecuteController
    {
        #region Fields

        private readonly List<ITimeRemaining> _timeRemainings;
        private readonly SequentialTimeRemainingsContainer _timeRemainingSequences;
        private readonly UnityTimeServices _timeService;

        #endregion


        #region ClassLifeCycles

        public TimeRemainingController()
        {
            _timeRemainings = TimeRemainingExtensions.TimeRemainings;
            _timeRemainingSequences = TimeRemainingExtensions.SequencesContainer;
            _timeService = Services.SharedInstance.UnityTimeServices;
        }

        #endregion


        #region IExecuteController

        public void Execute()
        {
            var time = _timeService.DeltaTime();
            for (var i = 0; i < _timeRemainings.Count; i++)
            {
                var obj = _timeRemainings[i];
                obj.CurrentTime -= time;
                if (obj.CurrentTime <= 0.0f)
                {
                    obj?.Method?.Invoke();
                    if (!obj.IsRepeating)
                    {
                        obj.RemoveTimeRemaining();
                    }
                    else
                    {
                        obj.CurrentTime = obj.Time;
                    }
                }
            }

            if (_timeRemainingSequences.sequentialTimeRemainings.Count > 0)
            {
                var sequences = _timeRemainingSequences.sequentialTimeRemainings;
                var sequenceElement = sequences[0][_timeRemainingSequences.currentSeqElementIndex];

                sequenceElement.CurrentTime -= time;
                if (sequenceElement.CurrentTime <= 0.0f)
                {
                    sequenceElement?.Method?.Invoke();
                    if (++_timeRemainingSequences.currentSeqElementIndex >= sequences[0].Count)
                    {
                        sequences[0].RemoveSequentialTimeRemaining();
                    }
                }               
            }
        }

        #endregion
    }
}
