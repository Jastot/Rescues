using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rescues
{
    [Serializable]
    public class FigureStruct
    {
        [Range(-1,32)]
        public int UnicSequenceID = -1;
        public ChessPuzzleFiguresTypes IndexOfFigure;
        [Range(1, 8)]
        public int CurrentPositionX;
        [Range(1, 8)]
        public int CurrentPositionY;

        public int indexOfCurrentPosition;
        public List<EndPosition> EndPositions;
    }

    [Serializable]
    public class EndPosition
    {
        [Range(1, 8)]
        public int EndPositionX;
        [Range(1, 8)]
        public int EndPositionY;
    }
}