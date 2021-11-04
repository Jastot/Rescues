﻿using System.Collections.Generic;


namespace Rescues
{
    public static partial class TimeRemainingExtensions
    {
        #region Fields

        private static readonly List<ITimeRemaining> _timeRemainings = new List<ITimeRemaining>(10);
        private static readonly List<List<ITimeRemaining>> _sequentialTimeRemainings = new List<List<ITimeRemaining>>(10);

        #endregion


        #region Properties

        public static List<ITimeRemaining> TimeRemainings => _timeRemainings;
        public static List<List<ITimeRemaining>> SequentialTimeRemainings => _sequentialTimeRemainings;

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
            if (_sequentialTimeRemainings.Contains(values))
            {
                return;
            }

            foreach (var value in values)
            {
                if (newTime >= 0)
                {
                    value.Time = newTime;
                }
                value.CurrentTime = value.Time; 
            }
            _sequentialTimeRemainings.Add(values);
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
            if (!_sequentialTimeRemainings.Contains(values))
            {
                return;
            }
            _sequentialTimeRemainings.Remove(values);
        }

        #endregion
    }
}
