using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rescues
{
    [Serializable]
    public class FigureStruct
    {
        public enum ColorOfFigure
        {
            Black = 0,
            White = 1
        }
        
        public ChessPuzzleFiguresTypes IndexOfFigure;
        [Range(1, 8)]
        public int CurrentPositionX;
        [Range(1, 8)]
        public int CurrentPositionY;
        public List<EndPosition> EndPositions;
        [Header("Only for chart")]
        public ColorOfFigure Color;
        [NonSerialized]
        public int indexOfCurrentPosition;
        
    }

    [Serializable]
    public class EndPosition
    {
        public int UnicSequenceID = -1;
        [Range(1, 8)]
        public int EndPositionX = 1;
        [Range(1, 8)]
        public int EndPositionY = 1;
    }
    
   
}