using System;
using NoClearGames.DialogueSystem;
using NoClearGames.Patterns.Singleton;
using UnityEngine;

namespace NoClearGames.UI
{
    public class UIManager : SingletonMonoBehaviour<UIManager>
    {
        //public HudPage hudPage;
        //public PausePopUp pausePopUp;
        //public GameOverPopUp gameOverPopUp;
        //public DialoguePopUp dialoguePopUp;
        //public HelpPopUp helpPopUp;

        public LevelDialogueData dialogueData;

        public void ShowDialogue(string id, Action endAction)
        {
            DialogueMessage dialogue = dialogueData.GetDialogue(id);

            if (dialogue == null)
            {
                Debug.Log($"Dialogue with id = {id} not found");
                return;
            }
        }
    }
}