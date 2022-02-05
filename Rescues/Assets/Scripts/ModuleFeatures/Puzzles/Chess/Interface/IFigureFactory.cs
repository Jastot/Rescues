using System.Collections.Generic;
using Rescues;
using UnityEngine;

namespace ModuleFeatures.Puzzles.Chess.Interface
{
    public interface IFigureFactory
    {
        Figure CreateAFigure(ChessPuzzleFiguresTypes figure,
            int posX,int posY, List<EndPosition> endPositions);
    }
}