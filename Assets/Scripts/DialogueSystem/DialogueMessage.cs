using System;
using UnityEngine;

namespace NoClearGames.DialogueSystem
{
    [Serializable]
    public class DialogueMessage
    {
        public string id;
        public Dialogues[] messages;
        public Action onEndAction;
        public bool claimed;
    }

    [Serializable]
    public class Dialogues
    {
        public string title;
        [TextArea] public string message;
        public Sprite spr;
        public string btnMessage;
    }
}