using System;
using System.Collections.Generic;
using Rescues;
using UnityEditor;
using UnityEngine;

    [CustomEditor(typeof(ChessPuzzleData))]
    public class ChessPuzzleDataEditor : Editor
    {
        #region Fields

        private List<string[,]> _board;
        private SerializedProperty Figures;
        private int _globalIndexOfArray;
        private ChessPuzzleData _chessPuzzleData;
        
        #endregion

        
        #region EditorMethods
        
        void OnEnable()
        {
            Figures = serializedObject.FindProperty("ElemntsOnBoard");
            _board = new List<string[,]>();
            _board.Add(new string[8, 8]);
            _globalIndexOfArray = 0;
        }
        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            _globalIndexOfArray = 0;
           
            serializedObject.Update();
            foreach (var obj in Figures.serializedObject.targetObjects)
            {
                _chessPuzzleData = obj as ChessPuzzleData;
                SetNullableTable(0);
                var countOfEndPoints = 0;
                foreach (var figureStruct in _chessPuzzleData.ElemntsOnBoard)
                {
                    countOfEndPoints += figureStruct.EndPositions.Count;
                }
                if (_board.Count != countOfEndPoints+1)
                {
                    for (int i = 0; i < countOfEndPoints; i++)
                    {
                        _board.Add(new string[8, 8]);
                    }
                }
                foreach (var figureStruct in _chessPuzzleData.ElemntsOnBoard)
                {
                    foreach (var endPositions in figureStruct.EndPositions)
                    {
                        if (endPositions==null)
                        {
                            return;
                        }
                        _globalIndexOfArray++;
                        SetNullableTable(_globalIndexOfArray);
                        _board[_globalIndexOfArray][figureStruct.CurrentPositionX-1,
                            figureStruct.CurrentPositionY-1] = "__";
                        SetArrayParameters(endPositions.EndPositionX-1,endPositions.EndPositionY-1,figureStruct);
                    }
                }
            }

            int indexForDraw = 0;
            foreach (var elementToDraw in _board)
            {
                DrawTable(indexForDraw);
                indexForDraw++;
            }
            
            int indexForTest=0;
            foreach (var whatToTest in _board)
            {
                if (CheckBoardForRetardAlert(indexForTest))
                {
                    EditorGUILayout.BeginHorizontal("Error");
                    EditorGUILayout.LabelField("RETARD ALERT");
                    EditorGUILayout.EndHorizontal();
                }
                indexForTest++;
            }
            EditorGUILayout.PropertyField(serializedObject.FindProperty("ElemntsOnBoard"), true);
            serializedObject.ApplyModifiedProperties();
        }

        private void OnDisable()
        {
            _board.Clear();
        }

        #endregion

        #region Methods

        private void DrawTable(int indexOfDraw)
        {
            GUILayout.BeginVertical();
            for (int i = 7; i >= 0; i--)
            {
                GUILayout.Label(
                    $"|_{_board[indexOfDraw][0,i]}_||_{_board[indexOfDraw][1, i]}_||_{_board[indexOfDraw][2, i]}_||_{_board[indexOfDraw][3, i]}_||_{_board[indexOfDraw][4, i]}_||_{_board[indexOfDraw][5, i]}_||_{_board[indexOfDraw][6, i]}_||_{_board[indexOfDraw][7, i]}_|"
                    );
                GUILayout.FlexibleSpace();
            }
            EditorGUILayout.Separator();
            GUILayout.EndVertical();
        }

        private bool CheckBoardForRetardAlert(int index)
        {
            var countOfFigures = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (_board[index][i,j] != "__")
                    {
                        countOfFigures++;
                    }
                }
            }

            if (countOfFigures == _chessPuzzleData.ElemntsOnBoard.Count)
            {
                return false;
            }
            return true;
        }

        private void SetNullableTable(int index)
        {
            if (index<1)
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        _board[index][i, j] = "__";
                    }
                }
                foreach (var figureStruct in _chessPuzzleData.ElemntsOnBoard)
                {
                    SetArrayParameters(figureStruct.CurrentPositionX-1,
                        figureStruct.CurrentPositionY-1, figureStruct);
                }
            }
            else
            {
                SetFromPreBoard(index);
            }
           
        }

        private void SetFromPreBoard(int index)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    _board[index][i, j] = _board[index-1][i,j];
                }
            }
        }
        
        private void SetArrayParameters(int whereX, int whereY, FigureStruct what)
        {
            if (_board != null)
                _board[_globalIndexOfArray][whereX, whereY] =
                    what.Color.ToString().Substring(0,1) +
                    what.IndexOfFigure.ToString().Substring(0,1);
        }
        
        #endregion
    }