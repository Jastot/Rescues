using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataSavingSystem;
using UnityEngine;

namespace Rescues
{
    [Serializable]
    public sealed class WorldGameData
    {
        #region Fields

        private SavingPacked _savingPacked;
        private List<SavingElementBehaviour> _listOfSavingElementBehaviour;
        public bool needUnPack = false;
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
        
        public void SetListOfInteractable(List<SavingElementBehaviour> listOfSavingElementBehaviour)
        {
            UnSubOnSavingBeh();
            _listOfSavingElementBehaviour = listOfSavingElementBehaviour;
            if (needUnPack)
                UnPackagingByLevels();
            SubOnSavingBeh();
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

        public void SaveItem(ItemData itemData,ItemCondition itemCondition)
        {
            _savingPacked.ItemBehaviours.Insert(_savingPacked.ItemBehaviours.
                FindIndex(s => s.SavingStruct.Id == itemData.itemID), new ItemListData()
                {
                    SavingStruct = new SavingStruct()
                    {
                        Id = itemData.itemID,
                        Name = itemData.Name
                    },
                    ItemCondition = itemCondition
                });
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

        #region SavingElementBeh

        private void SubOnSavingBeh()
        {
            foreach (var savingElementBehaviour in _listOfSavingElementBehaviour)
            {
                savingElementBehaviour.SaveSequence += PackagingByLevel;
            }
        }
        private void UnSubOnSavingBeh()
        {
            if (_listOfSavingElementBehaviour!=null)
            {
                foreach (var savingElementBehaviour in _listOfSavingElementBehaviour)
                {
                    savingElementBehaviour.SaveSequence -= PackagingByLevel;
                } 
            }
        }
       
        private void PackagingByLevel(EventSequence obj,string levelsName,int indexInSequence)
        {
            var LevelProgress = _savingPacked.LevelsProgress.FirstOrDefault(i => i.levelsName == levelsName);
            if (!LevelProgress.eventSequenceData.Any(n=> n.savingStruct.Id == obj.savingStruct.Id))
                LevelProgress.eventSequenceData.Add(obj);
            
        }
        
        private void UnPackagingByLevels()
        {
            foreach (var levelProgress in _savingPacked.LevelsProgress)
            {
                foreach (var eventSequence in levelProgress.eventSequenceData)
                {
                    var setting = _listOfSavingElementBehaviour.
                        FirstOrDefault(s=>
                        {
                            s.GetPartInfo(eventSequence.indexInList,out var id, out var name);
                            if (id == eventSequence.savingStruct.Id)
                                return true;
                            else
                                return false;
                        });
                    if (setting != null)
                        setting.SetPartStateFalse(eventSequence.indexInList);
                }
            }

            needUnPack = false;
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
            foreach (Transform transform in location.items.transform)
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
            
            foreach (Transform transform in location.puzzles.transform)
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

        public void OpenCurrentLocation(Location location,List<IInteractable> interactables,InventoryBehaviour inventoryBehaviour)
        {
            LoadStart?.Invoke();
            var locationIndex = LookForLevelByNameInt(location.name);
            
            foreach (var item in _savingPacked.ItemBehaviours)
            {
                if ((item.ItemCondition == (ItemCondition)0))
                    foreach (Transform realItem in location.items)
                        if (item.SavingStruct.Id==realItem.gameObject.GetComponent<ItemBehaviour>().Id)
                            realItem.gameObject.SetActive(false);
                if (item.ItemCondition==(ItemCondition)2)
                {
                    foreach (Transform realItem in location.items)
                    {
                        var itemBehaviour = realItem.gameObject.GetComponent<ItemBehaviour>();
                        if (item.SavingStruct.Id == itemBehaviour.Id)
                        {
                            inventoryBehaviour.AddItem(itemBehaviour.ItemData);
                            realItem.gameObject.SetActive(false);
                        }
                    }
                }
            }
            foreach (var puzzle in _savingPacked.LevelsProgress[locationIndex].puzzleListData)
            {
                if ((puzzle.PuzzleCondition == (PuzzleCondition)1))
                    foreach (Transform realPuzzle in location.puzzles)
                        if (puzzle.SavingStruct.Id == realPuzzle.gameObject.GetComponent<PuzzleBehaviour>().Id)
                            realPuzzle.gameObject.SetActive(false);
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
    
    