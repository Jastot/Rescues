using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace Rescues
{
    [CustomEditor(typeof(ChessPuzzleData))]
    public class ChessPuzzleDataEditor : Editor
    {
        private char[,] _board;
        
        private SerializedProperty Figures;
        void OnEnable()
        {
            // Fetch the objects from the GameObject script to display in the inspector
            Figures = serializedObject.FindProperty("ElemntsOnBoard");
            _board = new char[8, 8];
            
        }
        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    _board[i, j] = '_';
                }
            }
            serializedObject.Update();
            foreach (var VARIABLE in Figures.serializedObject.targetObjects)
            {
                var data = VARIABLE as ChessPuzzleData;
                foreach (var figureStruct in data.ElemntsOnBoard)
                {
                    if (_board != null)
                        _board[figureStruct.CurrentPositionX-1, figureStruct.CurrentPositionY-1] =
                            (figureStruct.IndexOfFigure.ToString())[0];
                }
            }
            var style= GUI.skin.GetStyle("label");
            style.margin.left = 400;
            for (int i = 0; i < 8; i++)
            {
                GUILayout.Label(
                        $"|_{_board[i, 0]}_||_{_board[i, 1]}_||_{_board[i, 2]}_||_{_board[i, 3]}_||_{_board[i, 4]}_||_{_board[i, 5]}_||_{_board[i, 6]}_||_{_board[i, 7]}_|",
                        style);
                GUILayout.FlexibleSpace();
            }
            
            
            EditorGUILayout.PropertyField(serializedObject.FindProperty("ElemntsOnBoard"), true);
            serializedObject.ApplyModifiedProperties();
            
        }
    }
    
    
    [Serializable]
    [CreateAssetMenu(fileName = "ChessPuzzle", menuName = "Data/Puzzles/ChessPuzzle")]
    public class ChessPuzzleData: ScriptableObject
    {
        #region Fields
        
        [Header("Пример последовательности: 1 0 2")]
        [Header("Последовательность активных элементов")] [SerializeField]
        public string Sequence;
        [Header("Элементы на доске")]
        public List<FigureStruct> ElemntsOnBoard;

        #endregion
    }
}