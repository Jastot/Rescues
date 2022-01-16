using Rescues;
using UnityEngine;

namespace ModuleFeatures.Puzzles.Chess.Interface
{
    public interface IFigureFactory
    {
        Figure CreateAFigure(int id,ChessPuzzleFiguresTypes figure,
            int posX,int posY, int endPosX, int endPosY);
    }
}