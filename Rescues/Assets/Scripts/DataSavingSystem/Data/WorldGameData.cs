using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;


namespace Rescues
{
    [Serializable]
    public sealed class WorldGameData
    {
        #region Fields
        
        //TODO: set player's position
        private static string _playerPosition;
        private static PlayersProgress _playersProgress;
        private static List<ItemListData> _itemBehaviours;
        private static List<LevelProgress> _levelsProgress;
        public Action RestartLevel = delegate {};
        
        #endregion

        
        #region ClassLifeCycles

        public WorldGameData()
        {
            _playerPosition = null;
            _itemBehaviours = new List<ItemListData>();
            _playersProgress = new PlayersProgress();
            _levelsProgress = new List<LevelProgress>();
        }

        #endregion
        
        
        #region Methods

        public void SavePlayersPosition(Transform playersPosition)
        {
            _playerPosition = ConvertVector3ToString(playersPosition.position);
        }

        public Vector3 LoadPlayerPosition()
        {
            return ConvertStringToVector3(_playerPosition);
        }
        
        public void SavePlayersProgress(int currentLevel)
        {
            _playersProgress.PlayerCurrentPositionInProgress = currentLevel;
        }

        private void AddNewLevelInfoToLevelsProgress(LevelProgress levelProgress)
        {
            _levelsProgress.Add(levelProgress);
        }

        #region Items

        public void AddItem(ItemListData itemListData)
        {
            _itemBehaviours.Add(itemListData);
        }

        public void SaveItem(ItemListData itemListData)
        {
            //TODO: переделать. 
            var index = _itemBehaviours.FindIndex(s => s.Name == itemListData.Name);
            if (index != -1)
                _itemBehaviours.Insert(index, itemListData);
            else
                AddItem(itemListData);
        }

        public void DeleteItem(ItemListData itemListData)
        {
            _itemBehaviours.Remove(itemListData);
        }


        #endregion

        
        #region Quests

        public void AddInLevelProgressQuest(int levelsIndex, QuestListData itemListData)
        {
            _levelsProgress[levelsIndex].QuestListData.Add(itemListData);
        }

        public void SaveInfoInLevelProgressQuest(int levelsIndex, string name, QuestListData itemListData)
        {
            var index = _levelsProgress[levelsIndex].QuestListData.FindIndex(s => s.Name == name);
            _levelsProgress[levelsIndex].QuestListData.Insert(index, itemListData);
        }

        public void DeleteInfoInLevelProgressQuest(int levelsIndex, QuestListData itemListData)
        {
            _levelsProgress[levelsIndex].QuestListData.Remove(itemListData);
        }


        #endregion

        
        #region Puzzles

        public void AddInLevelProgressPuzzle(int levelsIndex, PuzzleListData itemListData)
        {
            _levelsProgress[levelsIndex].PuzzleListData.Add(itemListData);
        }

        public void SaveInfoInLevelProgressPuzzle(int levelsIndex, string name, PuzzleListData itemListData)
        {
            var index = _levelsProgress[levelsIndex].PuzzleListData.FindIndex(s => s.Name == name);
            _levelsProgress[levelsIndex].PuzzleListData.Insert(index, itemListData);
        }

        public void DeleteInfoInLevelProgressPuzzle(int levelsIndex, PuzzleListData itemListData)
        {
            _levelsProgress[levelsIndex].PuzzleListData.Remove(itemListData);
        }
        
        #endregion

        public byte[] Serialize()
        {
            IEnumerable<byte> total = Encoding.ASCII.GetBytes("[").Concat((IEnumerable<byte>)Encoding.ASCII.GetBytes(_playerPosition)).Concat(Encoding.ASCII.GetBytes("]"))
                .Concat(Encoding.ASCII.GetBytes("[")).Concat(ByteConverter.AddToStreamPlayersProgress(_playersProgress)).Concat(Encoding.ASCII.GetBytes("]")).
                Concat(Encoding.ASCII.GetBytes("[")).Concat(ByteConverter.AddToStreamItems(_itemBehaviours)).Concat(Encoding.ASCII.GetBytes("]"))
                .Concat(ByteConverter.AddToStreamLevelProgress(_levelsProgress));
            byte[] result = total.ToArray();
            return result;
        }

        public static void Deserialize(byte[] data)
        {
            var dataString = Encoding.ASCII.GetString(data);
            ByteConverter.DataReader(dataString, out _playerPosition,out _itemBehaviours, out _playersProgress, out _levelsProgress);
        }

        public bool LookForLevelByNameBool(string name)
        {
            foreach (var levelProgress in _levelsProgress)
                if (levelProgress.LevelsName == name)
                    return true;
            return false;
        }

        public int LookForLevelByNameInt(string name)
        {
            int counter = 0;
            foreach (var levelProgress in _levelsProgress)
            {
                if (levelProgress.LevelsName == name)
                {
                    return counter;
                }

                counter++;
            }

            return -1;
        }

        public void AddNewLocation(Location location,IGate gate)
        {
            AddNewLevelInfoToLevelsProgress(new LevelProgress()
            {
                LevelsName = location.name,
                LastGate = gate
            });
            int correctIndex = _levelsProgress.Count - 1;
            foreach (Transform transform in location._items.transform)
                AddItem(new ItemListData()
                {
                    Name = transform.name,
                    ItemCondition = (ItemCondition) 1
                });
            foreach (Transform transform in location._puzzles.transform)
                AddInLevelProgressPuzzle(correctIndex, new PuzzleListData()
                {
                    Name = transform.name,
                    PuzzleCondition = (PuzzleCondition) 1
                });
        }

        public void OpenCurrentLocation(Location location)
        {
            var locationIndex = LookForLevelByNameInt(location.name);
            foreach (var item in _itemBehaviours)
            {
                if ((item.ItemCondition == (ItemCondition)0)||(item.ItemCondition==(ItemCondition)2))
                    foreach (Transform realItem in location._items)
                        if (item.Name==realItem.gameObject.name)
                            realItem.gameObject.SetActive(false);
            }
            foreach (var puzzle in _levelsProgress[locationIndex].PuzzleListData)
            {
                if ((puzzle.PuzzleCondition == (PuzzleCondition)1))
                    foreach (Transform realPuzzle in location._puzzles)
                        if (puzzle.Name == realPuzzle.gameObject.name)
                            realPuzzle.gameObject.SetActive(false);
            }
        }

        public IGate GetLastGate()
        {
            return _levelsProgress[_playersProgress.PlayerCurrentPositionInProgress].LastGate;
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
    
    