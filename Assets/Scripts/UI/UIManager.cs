using System;
using NoClearGames.DialogueSystem;
using NoClearGames.Patterns.Singleton;
using UnityEngine;

namespace NoClearGames.UI
{
    public class UIManager : SingletonMonoBehaviour<UIManager>
    {
        public HUDPage hudPage;
        public ResultPopUp resultPop;
        public PausePopUp pausePopUp;

        //public GameOverPopUp gameOverPopUp;
        public DialoguePopUp tutorialDialoguePopUp;

        public LevelDialogueData tutorialDialogueData;

        public void ShowTutorialDialogue(string id, Action endAction)
        {
            DialogueMessage dialogue = tutorialDialogueData.GetDialogue(id);

            if (dialogue == null)
            {
                Debug.Log($"Dialogue with id = {id} not found");
                return;
            }

            tutorialDialoguePopUp.Show(dialogue, endAction);
        }
    }
}