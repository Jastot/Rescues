using System;
using System.Collections.Generic;
using DataSavingSystem;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Rescues
{
    public sealed class InitializeSaveLoadController : IInitializeController,ITearDownController
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
            ReSetInteractable();
            _context.saveLoadBehaviour = Object.FindObjectOfType<SaveLoadBehaviour>(true);
            _context.saveLoadBehaviour.ReEnable += UpdateListOfSaves;
            _context.gameMenu.CalledSaveLoad += _context.saveLoadBehaviour.SetSaveLoad;
            _context.gameMenu.CalledSaveLoad += _context.saveLoadBehaviour.SwitchState;
            _context.saveLoadBehaviour.BackAction += _context.saveLoadBehaviour.SwitchState;
            _context.saveLoadBehaviour.Saving += Saving;
            _context.saveLoadBehaviour.Loading += Loading;
            _context.WorldGameData.SaveStart += ReSetInteractable;
            _context.WorldGameData.LoadStart += ReSetInteractable;
        }
        
        
        public void TearDown()
        {
            _context.saveLoadBehaviour.ReEnable -= UpdateListOfSaves;
            _context.gameMenu.CalledSaveLoad -= _context.saveLoadBehaviour.SetSaveLoad;
            _context.gameMenu.CalledSaveLoad -= _context.saveLoadBehaviour.SwitchState;
            _context.saveLoadBehaviour.BackAction -= _context.saveLoadBehaviour.SwitchState;
            _context.saveLoadBehaviour.Saving -= Saving;
            _context.saveLoadBehaviour.Loading -= Loading;
            _context.WorldGameData.SaveStart -= ReSetInteractable;
            _context.WorldGameData.LoadStart -= ReSetInteractable;
        }
        #endregion

        
        #region Methods

        private void Loading(string obj)
        {
            _context.WorldGameData.needUnPack = true;
            _gameSavingSerializer.Load(_context.WorldGameData,obj);
            _context.WorldGameData.RestartLevel.Invoke();
        }

        private void Saving(string name)
        {
            _context.WorldGameData.needUnPack = false;
            _context.WorldGameData.SavePlayersPosition(_context.character.Transform);
            _gameSavingSerializer.Save(_context.WorldGameData,name);
        }

        private void UpdateListOfSaves()
        {
            _context.saveLoadBehaviour.FileContexts = _gameSavingSerializer.GetAllSaves();
        }

        private void ReSetInteractable()
        {
            var listOfInter = _context.GetTriggers(InteractableObjectType.EventSystem);
            List<SavingElementBehaviour> savingElementBehaviours = new List<SavingElementBehaviour>();
            foreach (var interactable in listOfInter)
            {
                var beh = interactable as InteractableObjectBehavior;
                var savingElementBehaviour = beh.GetComponent<SavingElementBehaviour>();
                if (savingElementBehaviour!=null)
                    savingElementBehaviours.Add(savingElementBehaviour);
            }
            _context.WorldGameData.SetListOfInteractable(savingElementBehaviours);
            savingElementBehaviours.Clear();
        }

        #endregion

    }
}