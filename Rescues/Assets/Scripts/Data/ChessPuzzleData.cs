using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace Rescues
{
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