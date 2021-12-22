using System;
using Rescues;
using UnityEngine;

namespace Rescues
{
    public class ChessPuzzle: Puzzle
    {
        #region Fileds

        [SerializeField] private ChessPuzzleData _chessPuzzleData;
        private ChessBoard _chessBoard;
        private bool _isPlayerRight;
        
        public string _playersSequence;
        #endregion
        
        
        #region  Propeties

        public ChessBoard ChessBoard => _chessBoard;
        
        
        #endregion


        #region UnityMethods

        private void Start()
        {
            _chessBoard = gameObject.GetComponentInChildren<ChessBoard>();
            _chessBoard._chessPuzzleData = _chessPuzzleData;
            _chessBoard.Loaded += BoardLoading;
            _chessBoard.FigurePlacedOnNewPosition += LookingAtSequence;
        }

        #endregion

        #region Methods

        private void BoardLoading()
        {
            _chessBoard.SetPuzzledFigures();
        }

        private void LookingAtSequence(FigureStruct _figureStruct)
        {
            
            if (CheckFigurePosition(_figureStruct))
                _playersSequence += _figureStruct.UnicSequenceID + " ";
            else
                _playersSequence += "-1 ";
            CheckComplete();
        }

        private bool CheckFigurePosition(FigureStruct figureStruct)
        {
            if (figureStruct.EndPositionX == figureStruct.CurrentPositionX
                && figureStruct.EndPositionY == figureStruct.CurrentPositionY)
                return true;
            else
                return false;
        }
        #endregion
        
    }
}