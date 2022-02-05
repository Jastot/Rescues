using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Rescues
{
    public class Figure: MonoBehaviour,IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        #region Fields

        [SerializeField] private FigureStruct _figureStruct;

        private Vector2 _inWhichCellItIs;
        public event Action<Figure> OnBeginDragEvent;
        public event Action<Figure> OnEndDragEvent;
        public event Action<Figure> OnDragEvent;

        #endregion
        

        #region Methods

        public void OnDrag(PointerEventData eventData)
        {
            OnDragEvent?.Invoke(this);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnEndDragEvent?.Invoke(this);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            OnBeginDragEvent?.Invoke(this);
        }

        public void SetCell(int x, int y)
        {
            _inWhichCellItIs = new Vector2(x, y);
        }
        public Vector2 GetCell()
        {
            return _inWhichCellItIs;
        }

        public FigureStruct GetFigureStruct()
        {
            return _figureStruct;
        }
        
        public void SetFigurePosition(int x,int y)
        {
            gameObject.transform.localPosition = new Vector3(x, y, 0);
            _figureStruct.CurrentPositionX = x+1;
            _figureStruct.CurrentPositionY = y+1;
        }
        
        public void SetFigureStartInfo(
            int CurrentPositionX,
         int CurrentPositionY,
            List<EndPosition> endPositions
        )
        {
            _figureStruct.CurrentPositionX = CurrentPositionX;
            _figureStruct.CurrentPositionY = CurrentPositionY;
            _figureStruct.indexOfCurrentPosition = 0;
            _figureStruct.EndPositions = endPositions;
        }
        
        #endregion

    }
}