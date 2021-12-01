using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Rescues
{
    [Serializable]
    public sealed class WorldGameData
    {
        #region Fields

        private SavingPacked _savingPacked;
        private List<IInteractable> _listOfInteractable;
        
        public Action SaveStart = delegate {};
        public Action LoadStart = delegate {};
        public Action RestartLevel = delegate {};
        
        #endregion

        
        #region ClassLifeCycles

        public WorldGameData()
        {
            _savingPacked = new SavingPacked
            {
                PlayerPosition = null,
                ItemBehaviours = new List<ItemListData>(),
                PlayersProgress = new PlayersProgress(),
                LevelsProgress = new List<LevelProgress>()
            };
        }

        #endregion
        
        
        #region Methods

        public void SavePlayersPosition(Transform playersPosition)
        {
            _savingPacked.PlayerPosition = ConvertVector3ToString(playersPosition.position);
        }

        public Vector3 LoadPlayerPosition()
        {
            return ConvertStringToVector3(_savingPacked.PlayerPosition);
        }
        
        public void SetListOfInteractable(List<IInteractable> listOfInteractable)
        {
            _listOfInteractable = listOfInteractable;
            PackagingByLevels();
        }
        
        public void SavePlayersProgress(int currentLevel)
        {
            _savingPacked.PlayersProgress.PlayerCurrentPositionInProgress = currentLevel;
        }

        private void AddNewLevelInfoToLevelsProgress(LevelProgress levelProgress)
        {
            _savingPacked.LevelsProgress.Add(levelProgress);
        }

        #region Items

        public void AddItem(ItemListData itemListData)
        {
            _savingPacked.ItemBehaviours.Add(itemListData);
        }

        public void SaveItem(ItemListData itemListData)
        {
            //TODO: переделать. 
            var index = _savingPacked.ItemBehaviours.FindIndex(s => s.SavingStruct.Name == itemListData.SavingStruct.Name);
            if (index != -1)
                _savingPacked.ItemBehaviours.Insert(index, itemListData);
            else
                AddItem(itemListData);
        }

        public void DeleteItem(ItemListData itemListData)
        {
            _savingPacked.ItemBehaviours.Remove(itemListData);
        }


        #endregion

        
        #region Quests

        public void AddInLevelProgressQuest(int levelsIndex, QuestListData itemListData)
        {
            _savingPacked.LevelsProgress[levelsIndex].questListData.Add(itemListData);
        }

        public void SaveInfoInLevelProgressQuest(int levelsIndex, string name, QuestListData itemListData)
        {
            var index = _savingPacked.LevelsProgress[levelsIndex].questListData.FindIndex(s => s.SavingStruct.Name == name);
            _savingPacked.LevelsProgress[levelsIndex].questListData.Insert(index, itemListData);
        }

        public void DeleteInfoInLevelProgressQuest(int levelsIndex, QuestListData itemListData)
        {
            _savingPacked.LevelsProgress[levelsIndex].questListData.Remove(itemListData);
        }


        #endregion

        #region IInteractable

        private void PackagingByLevels()
        {
            foreach (var interactable in _listOfInteractable)
            {
                var beh = interactable as InteractableObjectBehavior;
                var levelsName = beh.GameObject.transform.parent.parent.name;
                _savingPacked.LevelsProgress.FirstOrDefault(i => i.levelsName == levelsName)?.listOfInteractable.Add(
                    new InteractiveCondition()
                    {
                        SavingStruct = new SavingStruct()
                        {
                            Id = beh.Id,
                            Name = beh.name
                        },
                        IsInteractable = beh.IsInteractable,
                        IsInteractionLocked = beh.IsInteractionLocked
                    });
            }
        }

        private void UnPackagingByLevels()
        {
            foreach (var levelProgress in _savingPacked.LevelsProgress)
            {
                foreach (var interactive in levelProgress.listOfInteractable)
                {
                    var setting = _listOfInteractable.FirstOrDefault(i=> i.Id == interactive.SavingStruct.Id);
                    if (setting != null)
                    {
                        setting.IsInteractable = interactive.IsInteractable;
                        setting.IsInteractionLocked = interactive.IsInteractionLocked;
                    }
                }
            }
        }

        #endregion
        
        #region Puzzles

        public void AddInLevelProgressPuzzle(int levelsIndex, PuzzleListData itemListData)
        {
            _savingPacked.LevelsProgress[levelsIndex].puzzleListData.Add(itemListData);
        }

        public void SaveInfoInLevelProgressPuzzle(int levelsIndex, string name, PuzzleListData itemListData)
        {
            var index = _savingPacked.LevelsProgress[levelsIndex].puzzleListData.FindIndex(s => s.SavingStruct.Name == name);
            _savingPacked.LevelsProgress[levelsIndex].puzzleListData.Insert(index, itemListData);
        }

        public void DeleteInfoInLevelProgressPuzzle(int levelsIndex, PuzzleListData itemListData)
        {
            _savingPacked.LevelsProgress[levelsIndex].puzzleListData.Remove(itemListData);
        }
        
        #endregion

        public byte[] Serialize()
        {
            SaveStart.Invoke();
            IEnumerable<byte> total = ByteConverter.AddToStreamAllPacked(_savingPacked);
            byte[] result = total.ToArray();
            return result;
        }

        public void Deserialize(byte[] data)
        {
            LoadStart.Invoke();
            var dataString = Encoding.ASCII.GetString(data);
            ByteConverter.DataReader(dataString, out _savingPacked);
        }

        public bool LookForLevelByNameBool(string name)
        {
            foreach (var levelProgress in _savingPacked.LevelsProgress)
                if (levelProgress.levelsName == name)
                    return true;
            return false;
        }

        public int LookForLevelByNameInt(string name)
        {
            int counter = 0;
            foreach (var levelProgress in _savingPacked.LevelsProgress)
            {
                if (levelProgress.levelsName == name)
                {
                    return counter;
                }

                counter++;
            }

            return -1;
        }

        public void SetLastGate(IGate gate)
        {
            _savingPacked.LastGate = new GateStruct()
            {
                goToLevelName = gate.GoToLevelName,
                goToLocationName = gate.GoToLocationName,
                goToGateId = gate.GoToGateId
            };
        }
        
        public void AddNewLocation(Location location)
        {
            AddNewLevelInfoToLevelsProgress(new LevelProgress()
            {
                levelsName = location.name
            });
            int correctIndex = _savingPacked.LevelsProgress.Count - 1;
            foreach (Transform transform in location._items.transform)
            {
                var itemBehaviour = transform.GetComponentInChildren<ItemBehaviour>();
                AddItem(new ItemListData()
                {
                    SavingStruct = new SavingStruct()
                    {
                        Id = itemBehaviour.Id,
                        Name = itemBehaviour.name
                    },
                    ItemCondition = (ItemCondition) 1
                });
            }

            foreach (Transform transform in location._puzzles.transform)
            {
                var itemBehaviour = transform.GetComponentInChildren<PuzzleBehaviour>();
                AddInLevelProgressPuzzle(correctIndex, new PuzzleListData()
                {
                    SavingStruct = new SavingStruct()
                    {
                        Id = itemBehaviour.Id,
                        Name = itemBehaviour.name
                    },
                    PuzzleCondition = (PuzzleCondition) 1
                });
            }
        }

        public void OpenCurrentLocation(Location location,List<IInteractable> interactables)
        {
            var locationIndex = LookForLevelByNameInt(location.name);
            foreach (var item in _savingPacked.ItemBehaviours)
            {
                if ((item.ItemCondition == (ItemCondition)0)||(item.ItemCondition==(ItemCondition)2))
                    foreach (Transform realItem in location._items)
                        if (item.SavingStruct.Name==realItem.gameObject.name)//TODO: ID
                            realItem.gameObject.SetActive(false);
            }
            foreach (var puzzle in _savingPacked.LevelsProgress[locationIndex].puzzleListData)
            {
                if ((puzzle.PuzzleCondition == (PuzzleCondition)1))
                    foreach (Transform realPuzzle in location._puzzles)
                        if (puzzle.SavingStruct.Name == realPuzzle.gameObject.name)//TODO: ID
                            realPuzzle.gameObject.SetActive(false);
            }
            foreach (var interactable in _savingPacked.LevelsProgress[locationIndex].listOfInteractable)
            {
                //TODO: Need Test!!!
                var r = interactables.Where((p =>
                {
                    var a = p as InteractableObjectBehavior;
                    a.Id = interactable.SavingStruct.Id;
                    a.name = interactable.SavingStruct.Name;
                    return a;
                }))?.FirstOrDefault();
                if (r != null)
                {
                    r.IsInteractable = interactable.IsInteractable;
                    r.IsInteractionLocked = interactable.IsInteractionLocked;
                }
            }
        }

        public IGate GetLastGate()
        {
            return GateDataMock.GetMock(
                _savingPacked.LastGate.goToLevelName,
                _savingPacked.LastGate.goToLocationName,
                _savingPacked.LastGate.goToGateId);
        }
        
        
        public string ConvertVector3ToString(Vector3 vector3)
        {
            return vector3.x + "," + vector3.y + "," + vector3.z;
        }
        public Vector3 ConvertStringToVector3(string str)
        {
            var mass = str.Split(',');
            return new Vector3(Convert.ToInt32(mass[0]),Convert.ToInt32(mass[1]),0);
        }
        #endregion

        
    }
}
    
    