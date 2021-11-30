using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Rescues
{
    public static class ByteConverter
    {
        #region Methods

        public static IEnumerable<byte> AddToStreamAllPacked(SavingPacked savingPacked)
        {
            return Encoding.ASCII.GetBytes(JsonUtility.ToJson(savingPacked));
        }
        

        public static void DataReader(string dataString,out SavingPacked savingPacked)
        {
            savingPacked = JsonUtility.FromJson<SavingPacked>(dataString);
        }
        
        #endregion
    }
}