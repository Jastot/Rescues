using TMPro;
using UnityEngine;


namespace Rescues
{
    public abstract class WritePattern : ScriptableObject
    {
        #region Methods

        public abstract void DrawText(string inputText, TextMeshProUGUI outputTextContainer);
        public abstract void ClearText(TextMeshProUGUI textContainer); 

        #endregion
    }
}
