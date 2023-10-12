using NoClearGames.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace NoClearGames.UI
{
    public class HUDPage : BasePage
    {
        [SerializeField] private Button pauseBtn;
        [SerializeField] private Button tutorialBtn;
        [SerializeField] private Button startTrainBtn;

        [SerializeField] private LevelManager levelManager;

        public override void Awake()
        {
            base.Awake();
            pauseBtn.onClick.AddListener(ShowPause);
            tutorialBtn.onClick.AddListener(ShowTutorial);
            startTrainBtn.onClick.AddListener(StartTrain);
        }

        private void StartTrain()
        {
            startTrainBtn.interactable = false;
            levelManager.StartTrain();
        }

        private void ShowTutorial()
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.SFX.clickSfx);
            levelManager.ShowTutorialPage();
            //UIManager.Instance.ShowTutorialDialogue("Tutorial", () => UIManager.Instance.tutorialDialoguePopUp.Hide());
        }

        private void ShowPause()
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.SFX.clickSfx);
            UIManager.Instance.pausePopUp.Show();
        }
    }
}