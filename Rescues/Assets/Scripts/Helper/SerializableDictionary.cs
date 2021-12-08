using System;
using System.Collections.Generic;
using UnityEngine;


namespace Rescues
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField]
        private List<TKey> keys = new List<TKey>();

        [SerializeField]
        private List<TValue> values = new List<TValue>();

        // save the dictionary to lists
        public void OnBeforeSerialize()
        {
            if (keys.Count == values.Count)
            {
                keys.Clear();
                values.Clear();
                foreach (KeyValuePair<TKey, TValue> pair in this)
                {
                    keys.Add(pair.Key);
                    values.Add(pair.Value);
                }
            }
        }

        // load dictionary from lists
        public void OnAfterDeserialize()
        {
            if (keys.Count == values.Count)
            {
                Clear();
                for (int i = 0; i < keys.Count; i++)
                {
                    Add(keys[i], values[i]);
                }
            }
        }
    }
}