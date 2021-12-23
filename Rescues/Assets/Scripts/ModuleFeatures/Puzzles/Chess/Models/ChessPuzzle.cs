using System;
using System.Collections.Generic;
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
        
        public List<string> _playersSequence;
        
        #endregion
        
        
        #region  Propeties

        public ChessBoard ChessBoard => _chessBoard;
        
        #endregion


        #region UnityMethods

        private void Start()
        {
            _playersSequence = new List<string>();
            _chessBoard = gameObject.GetComponentInChildren<ChessBoard>();
            _chessBoard._chessPuzzleData = _chessPuzzleData;
            Initiate();
        }

        #endregion

        #region Methods

        public void Initiate()
        {
            _chessBoard.Loaded += BoardLoading;
            _chessBoard.FigurePlacedOnNewPosition += LookingAtSequence; 
        }

        public void CleanData()
        {
            _playersSequence.Clear();
            _chessBoard.Loaded -= BoardLoading;
            _chessBoard.FigurePlacedOnNewPosition -= LookingAtSequence;
        }
        
        private void BoardLoading()
        {
            _chessBoard.SetPuzzledFigures();
        }

        private void LookingAtSequence(FigureStruct _figureStruct)
        {
            
            if (CheckFigurePosition(_figureStruct))
                _playersSequence.Add(_figureStruct.UnicSequenceID+"");
            else
                _playersSequence.Add("-1");
            Debug.Log(_playersSequence);
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