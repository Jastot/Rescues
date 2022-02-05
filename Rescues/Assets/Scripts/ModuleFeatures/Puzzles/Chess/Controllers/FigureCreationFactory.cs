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
        private const int _offsetParameter = 1;
        
        #endregion


        #region Methods

        public FigureCreationFactory(Dictionary<ChessPuzzleFiguresTypes, GameObject>availableGameObjects,Transform parent)
                {
                    _availableGameObjectsDictionary = availableGameObjects;
                    _parent = parent;
                }
                
                
                public Figure CreateAFigure(ChessPuzzleFiguresTypes figure,
                    int posX,int posY, List<EndPosition> endPositions)
                {
                    var newFigure =Object.Instantiate(_availableGameObjectsDictionary[figure],_parent);
                    newFigure.gameObject.transform.localPosition = new Vector2(posX,posY);
                    var parameters = newFigure.GetComponent<Figure>();
                    
                    parameters.SetFigureStartInfo(
                        posX+_offsetParameter,
                        posY+_offsetParameter,
                        endPositions);
                    return parameters;
                }

        #endregion
    }
}