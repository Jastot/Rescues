using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Rescues
{
    public class Cell: MonoBehaviour
    {
        #region Flieds

        private ChessPuzzleFiguresTypes _cellType = ChessPuzzleFiguresTypes.None;
        private int _idOfFigurePlacedOnCell = 0;
        private bool _isCellOccupied;
        private Vector2 CorrectiveVector = new Vector2(1, 1);
        private Image _image;
        private Color _color;
        
        public int IndexX;
        public int IndexY;
        
        #endregion

        #region UnityMethods

        private void Awake()
        {
            _image = GetComponent<Image>();
            _color = _image.color;
        }

        #endregion
        
        #region Methods

        public void SetCellOccupied(bool x)
        {
            _isCellOccupied = x;
            if (_isCellOccupied)
            {
                _image.color = Color.red;
            }
            else
            {
                _image.color = _color;
            }
        }
        public bool GetCellOccupied()
        {
            return _isCellOccupied;
        }
        public ChessPuzzleFiguresTypes GetTypeOfCell()
        {
            return _cellType;
        }
        
        public void SetTypeOfCell(ChessPuzzleFiguresTypes type)
        {
            _cellType = type;
        }

        public int CalculateZone(float pointX, float pointY)
        {
            if ((pointX >= IndexX-1.5f && pointX <= IndexX-0.5f) && (pointY >= IndexY-1.5f && pointY <= IndexY-0.5f))
                if (!_isCellOccupied)
                    return 0;
                else
                   return 1;
            else
                return 2;
        }

        #endregion
        
    }
}