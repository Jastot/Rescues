using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Rescues
{
    public class Figure: MonoBehaviour,IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
    {
        #region Fields

        [SerializeField] private FigureStruct _figureStruct;

        private Vector2 _inWhichCellItIs;
        public event Action<Figure> OnBeginDragEvent;
        public event Action<Figure> OnEndDragEvent;
        public event Action<Figure> OnDragEvent;
        public event Action<Figure> OnDropEvent;

        #endregion
        

        #region Methods

        public void OnDrop(PointerEventData eventData)
        {
            OnDropEvent?.Invoke(this);
        }

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
        }
        
        public void SetFigureStartInfo(
            int ID,
         int CurrentPositionX,
         int CurrentPositionY
        )
        {
            _figureStruct.UnicSequenceID = ID;
            _figureStruct.CurrentPositionX = CurrentPositionX;
            _figureStruct.CurrentPositionY = CurrentPositionY;
            _figureStruct.EndPositionX = CurrentPositionX;
            _figureStruct.EndPositionY = CurrentPositionY;
        }
        
        #endregion

    }
}