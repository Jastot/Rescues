using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Rescues
{
    public sealed class GameSavingSerializer
    {
        #region Methods

        public IEnumerable<FileContext> GetAllSaves()
        {
            var path = $"{Serialization.path}/{Serialization.SAVING_PATH}";
            if (Directory.Exists(path) == false)
            {
                yield break;
            }       
            foreach (var file in Directory.GetFiles(path))
            {
                if(Path.HasExtension(file) == false || Path.GetExtension(file) != Serialization.DEFEND_EXTENSION)
                    continue;
                yield return new FileContext(new FileInfo(file));
            }
        }
        
        public void Delete(string fileName)
        {
            var savePath = $"{Serialization.path}/{Serialization.SAVING_PATH}/{fileName}{Serialization.DEFEND_EXTENSION}";
            if(File.Exists(savePath))
                File.Delete(savePath);
        }
        
        public void Save(WorldGameData data, string fileName)
        {
            Directory.CreateDirectory(Serialization.path+"/"+$"{Serialization.SAVING_PATH}");
            var savePath = $"{Serialization.path}/{Serialization.SAVING_PATH}/{fileName}{Serialization.DEFEND_EXTENSION}";
            File.WriteAllBytes(savePath, data.Serialize());
        }
        
        public void Load(string fileName)
        {
            var savePath = $"{Serialization.path}/{Serialization.SAVING_PATH}/{fileName}{Serialization.DEFEND_EXTENSION}";
            if (!File.Exists(savePath) == false)
                WorldGameData.Deserialize(File.ReadAllBytes(savePath));
            else
                throw new Exception("Loading failed");
            
        }

        #endregion
    }
}