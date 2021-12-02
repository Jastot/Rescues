using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Rescues
{
    public class DialogueUI : MonoBehaviour
    {
        #region Fields

        [Space(10)]
        [Header("Npc")]
        public GameObject npcContainer;
        public TextMeshProUGUI npcLabel;
        public TextMeshProUGUI npcText;
        public Color npcLabelColor;
        public Image npcImage;
        public Color npcImageNormalColor;
        [Range(1, 10)]
        public int writeStep;
        [Range(1, 10)]
        public int writeSpeed;
        public float timeBeforeNextNode;
        public Image npcBackGround;

        [Space(10)]
        [Header("Player")]
        public GameObject playerContainer;
        public TextMeshProUGUI playerLabel;
        public PossibleAnswer[] playerTextChoices;
        public Color playerLabelColor;
        public Image playerImage;
        public Color playerImageNormalColor;
        public Image playerBackground;

        [Space(10)]
        [Header("Other")]
        public GameObject dialogContainer;
        public Image background;
        public NodeSoundContainer nodeSoundContainer;

        #endregion
    }
}
