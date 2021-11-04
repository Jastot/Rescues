﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rescues
{
    public class DialogueUI : MonoBehaviour
    {
        [Header("Npc")]
        public GameObject npcContainer;
        public TextMeshProUGUI npcLabel;
        public TextMeshProUGUI npcText;
        public Image npcImage;

        [Header("Player"), Space(10)]
        public GameObject playerContainer;
        public TextMeshProUGUI playerLabel;
        public PossibleAnswer[] playerTextChoices;
        public Color playerLabelColor;
        public Image playerImage;

        [Header("Other"), Space(10)]
        public GameObject dialogContainer;
        public Image background;
        public AudioSource dialogueSound;
        public DestroyClip destroyClip;
    }
}
