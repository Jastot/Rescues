using System.Collections.Generic;


namespace Rescues
{
    public static partial class TimeRemainingExtensions
    {
        #region Fields

        private static readonly List<ITimeRemaining> _timeRemainings = new List<ITimeRemaining>(10);
        private static readonly SequentialTimeRemainingsContainer _sequencesContainer = 
            new SequentialTimeRemainingsContainer();

        #endregion


        #region Properties

        public static List<ITimeRemaining> TimeRemainings => _timeRemainings;
        public static SequentialTimeRemainingsContainer SequencesContainer => _sequencesContainer;

        #endregion


        #region Methods

        public static void AddTimeRemaining(this ITimeRemaining value, float newTime = -1.0f)
        {
            if (_timeRemainings.Contains(value))
            {
                return;
            }

            if (newTime >= 0)
            {
                value.Time = newTime;
            }
            value.CurrentTime = value.Time;
            _timeRemainings.Add(value);
        }

        public static void AddSequentialTimeRemaining(this List<ITimeRemaining> values, float newTime = -1.0f)
        {
            foreach (var value in values)
            {
                if (newTime >= 0)
                {
                    value.Time = newTime;
                }
                value.CurrentTime = value.Time;
            }
            _sequencesContainer.sequentialTimeRemainings.Add(values);
        }

        public static void RemoveTimeRemaining(this ITimeRemaining value)
        {
            if (!_timeRemainings.Contains(value))
            {
                return;
            }
            _timeRemainings.Remove(value);
        }

        public static void RemoveSequentialTimeRemaining(this List<ITimeRemaining> values)
        {
            if (!_sequencesContainer.sequentialTimeRemainings.Contains(values))
            {
                return;
            }
            _sequencesContainer.sequentialTimeRemainings.Remove(values);
            _sequencesContainer.currentSeqElementIndex = 0;
        }

        #endregion
    }
}
