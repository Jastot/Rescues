using System.Linq;
using UnityEngine;

namespace Rescues
{
    public class ChessController: IPuzzleController
    {
        #region Fields

        private string Sequence;
        private bool firstActive=true;
        #endregion
        
        #region IPuzzleController
        public void Initialize(Puzzle puzzle)
        {
            puzzle.Activated += Activate;
            puzzle.Closed += Close;
            puzzle.Finished += Finish;
            puzzle.CheckCompleted += CheckComplete;
            puzzle.ResetValuesToDefault += ResetValues;
            puzzle.ForceClose();
        }

        public void Activate(Puzzle puzzle)
        {            
            var puzzlePosition = Camera.main.transform.position;
            puzzlePosition.z = 0;
            puzzle.transform.position = puzzlePosition;
            puzzle.gameObject.SetActive(true);
        }

        public void Close(Puzzle puzzle)
        {
            
        }

        public void CheckComplete(Puzzle puzzle)
        {
            var specificPuzzle = puzzle as ChessPuzzle;
            if (firstActive)
            {
                Sequence = specificPuzzle.ChessBoard._chessPuzzleData.Sequence;
                firstActive=false;
            }
            var playersSequence = specificPuzzle._playersSequence;//.
            //     Remove(specificPuzzle._playersSequence.Length-1).Split(' ');
            if (specificPuzzle != null 
               // &&  Sequence.Take(Sequence.Length).SequenceEqual(playersSequence))
               && playersSequence.Contains(Sequence))
                Finish(specificPuzzle);
        }

        public void Finish(Puzzle puzzle)
        {
            puzzle.Finish();
        }

        public void ResetValues(Puzzle puzzle)
        {
            var specificPuzzle = puzzle as ChessPuzzle;
            if (specificPuzzle != null)
            {
                specificPuzzle.ChessBoard.SetNullableBoard();
                specificPuzzle._playersSequence = "";
                specificPuzzle.ChessBoard.SetPuzzledFigures();
            }
        }
        
        #endregion
    }
}