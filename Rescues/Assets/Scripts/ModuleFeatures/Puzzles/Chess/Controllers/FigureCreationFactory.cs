using System;
using System.Collections.Generic;
using ModuleFeatures.Puzzles.Chess.Interface;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Rescues
{
    public class FigureCreationFactory: IFigureFactory
    {
        #region Fields

        private Dictionary<ChessPuzzleFiguresTypes, GameObject> _availableGameObjectsDictionary;
        private readonly Transform _parent;
        
        #endregion
        
        public FigureCreationFactory(Dictionary<ChessPuzzleFiguresTypes, GameObject>availableGameObjects,Transform parent)
        {
            _availableGameObjectsDictionary = availableGameObjects;
            _parent = parent;
        }
        
        
        public Figure CreateAFigure(int id,ChessPuzzleFiguresTypes figure,Vector2 pos)
        {
            var newFigure =Object.Instantiate(_availableGameObjectsDictionary[figure],_parent);
            newFigure.gameObject.transform.localPosition = pos;
            var parameters = newFigure.GetComponent<Figure>();
            parameters.SetFigureStartInfo(id,Convert.ToInt32(pos.x),
                Convert.ToInt32(pos.y));
            return parameters;
        }
    }
}