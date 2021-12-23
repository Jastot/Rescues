using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rescues
{
    public class ChessBoard: MonoBehaviour
    {
        #region Fields

        [SerializeField] private GameObject[] _availableFigures;
        [SerializeField] private Transform _parentBoard;
        private Dictionary<ChessPuzzleFiguresTypes, GameObject> _availablePrefabsDictionary;
        private Cell[,] _board;
        private FigureCreationFactory _figureCreationFactory;
        private List<Figure> _figureStructs;
        private Figure _pickedUpFigure;
        private const int _indexOfMassiveI = 1;
        public event Action Loaded;
        public event Action<FigureStruct> FigurePlacedOnNewPosition;
        
        public ChessPuzzleData _chessPuzzleData;
        
        #endregion
        
        #region UnityMethods
        
        private void Start()
        {
            _availablePrefabsDictionary = new Dictionary<ChessPuzzleFiguresTypes, GameObject>();
            _figureStructs = new List<Figure>();
            MakeADictionary();
            BoardInitialization();
            SetNullableBoard();
            _figureCreationFactory = new FigureCreationFactory(
                _availablePrefabsDictionary,_parentBoard);
            Loaded.Invoke();
        }

        #endregion

        #region Methods
        
        private void MakeADictionary()
        {
            var index = 1;
            foreach (var prefab in _availableFigures)
            {
                _availablePrefabsDictionary.Add((ChessPuzzleFiguresTypes)index,prefab);
                index++;
            }
        }
        
        public void SetNullableBoard()
        {
            #region Set all Cells NONE

            for (int i = 0; i <= 7; i++)
            {
                for (int j = 0; j <= 7; j++)
                {
                    _board[i,j].SetTypeOfCell(ChessPuzzleFiguresTypes.None);
                }
            }

            #endregion

            #region Clean a board and logic

            foreach (var figure in _figureStructs)
            {
                figure.OnDragEvent -= OnDragFigure;
                figure.OnBeginDragEvent -= OnBeginDragFigure;
                figure.OnEndDragEvent -= OnEndDragFigure;
                figure.OnDropEvent -= OnDropFigure;
                Destroy(figure.gameObject);
            }
            _figureStructs.Clear();
            
            #endregion
        }
        
        private void BoardInitialization()
        {
            var realBoard = gameObject.GetComponentsInChildren<Cell>();
            _board = new Cell[8, 8];
            for (int i = 0; i <= 7; i++)
            {
                for (int j = 0; j <= 7; j++)
                {
                    var indexOfCell = i * 8 + j;
                    _board[j,i] = realBoard[indexOfCell];
                    realBoard[indexOfCell].IndexX = j+_indexOfMassiveI;
                    realBoard[indexOfCell].IndexY = i+_indexOfMassiveI;
                }
            }
        }

        public void SetPuzzledFigures()
        {
            foreach (var figureStruct in _chessPuzzleData.ElemntsOnBoard)
            {
                _board[figureStruct.CurrentPositionX-_indexOfMassiveI,figureStruct.CurrentPositionY-_indexOfMassiveI].
                    SetTypeOfCell(figureStruct.IndexOfFigure);
                _board[figureStruct.CurrentPositionX - _indexOfMassiveI, figureStruct.CurrentPositionY - _indexOfMassiveI]
                    .SetCellOccupied(true);
                var currentFigure = _figureCreationFactory.CreateAFigure(figureStruct.UnicSequenceID,figureStruct.IndexOfFigure,
                     new Vector2(figureStruct.CurrentPositionX-_indexOfMassiveI, figureStruct.CurrentPositionY-_indexOfMassiveI));
                currentFigure.OnDragEvent += OnDragFigure;
                currentFigure.OnBeginDragEvent += OnBeginDragFigure;
                currentFigure.OnEndDragEvent += OnEndDragFigure;
                currentFigure.OnDropEvent += OnDropFigure;
                currentFigure.SetCell(
                    figureStruct.CurrentPositionX-_indexOfMassiveI,
                    figureStruct.CurrentPositionY-_indexOfMassiveI);
                _figureStructs.Add(currentFigure);
            }
        }
        
        private void OnDragFigure(Figure obj)
        {
            obj.gameObject.transform.position = Input.mousePosition;
        }
        private void OnBeginDragFigure(Figure obj)
        {
            _pickedUpFigure = obj;
        }
        
        private void OnDropFigure(Figure obj)
        {
            if (_pickedUpFigure==null)
                return;
            //write to seq
            
        }
        
        private void OnEndDragFigure(Figure obj)
        {
            bool needToStopIt;
            Vector2 cellsCoordinates;
            Cell back;
            for (int i = 0; i <= 7; i++)
            {
                for (int j = 0; j <= 7; j++)
                {
                    needToStopIt = false;
                    switch (_board[i,j].CalculateZone(
                            obj.gameObject.transform.localPosition.x,
                            obj.gameObject.transform.localPosition.y))
                        {
                            case 0:
                                obj.SetFigurePosition(
                                    _board[i,j].IndexX-_indexOfMassiveI,
                                    _board[i,j].IndexY-_indexOfMassiveI);
                                cellsCoordinates = obj.GetCell();
                                var from = _board[Convert.ToInt32(cellsCoordinates.x),
                                    Convert.ToInt32(cellsCoordinates.y)];
                                from.SetCellOccupied(false);
                                obj.SetCell(i, j);
                                _board[i,j].SetCellOccupied(true);
                                needToStopIt = true;
                                break;
                            case 1:
                                cellsCoordinates = obj.GetCell();
                                back = _board[
                                    Convert.ToInt32(cellsCoordinates.x),
                                    Convert.ToInt32(cellsCoordinates.y)];
                                obj.SetFigurePosition(
                                    back.IndexX-_indexOfMassiveI,
                                    back.IndexY-_indexOfMassiveI);
                                break;
                            case 2:
                                break;
                        }
                    if (needToStopIt)
                        break;
                }
            }
            _pickedUpFigure = null;
            FigurePlacedOnNewPosition?.Invoke(obj.GetFigureStruct());
        }

        public Cell[,] GetBoard()
        {
            return _board;
        }
        
        #endregion
    }
}