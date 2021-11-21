using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Rescues
{
    public sealed class InitializeSaveLoadController : IInitializeController
    {
        #region Fields

        private readonly GameContext _context;

        private GameSavingSerializer _gameSavingSerializer;

        #endregion

        
        #region ClassLifeCycles

        public InitializeSaveLoadController(GameContext context, Services services)
        {
            _context = context;
            _context.WorldGameData = new WorldGameData();
            _gameSavingSerializer = new GameSavingSerializer();
        }

        #endregion

        
        #region IInitializeController

        public void Initialize()
        {
            _context.saveLoadBehaviour = Object.FindObjectOfType<SaveLoadBehaviour>(true);
            _context.saveLoadBehaviour.ReEnable += UpdateListOfSaves;
            _context.gameMenu.CalledSaveLoad += _context.saveLoadBehaviour.SetSaveLoad;
            _context.gameMenu.CalledSaveLoad += _context.saveLoadBehaviour.SwitchState;
            _context.saveLoadBehaviour.BackAction += _context.saveLoadBehaviour.SwitchState;
            _context.saveLoadBehaviour.Saving += Saving;
            _context.saveLoadBehaviour.Loading += Loading;
            _context.saveLoadBehaviour.gameObject.SetActive(false);
        }
        
        #endregion

        
        #region Methods

        private void Loading(string obj)
        {
            _gameSavingSerializer.Load(obj);
            _context.WorldGameData.RestartLevel.Invoke();
        }

        private void Saving(string name)
        {
            _context.WorldGameData.SavePlayersPosition(_context.character.Transform);
            _gameSavingSerializer.Save(_context.WorldGameData,name);
        }

        private void UpdateListOfSaves()
        {
            _context.saveLoadBehaviour.FileContexts = _gameSavingSerializer.GetAllSaves();
        }

        #endregion
    }
}