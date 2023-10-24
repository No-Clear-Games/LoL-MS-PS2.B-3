using System;
using DG.Tweening;
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
        [SerializeField] private Button proffesorBtn;

        [SerializeField] private LevelManager levelManager;

        public override void Awake()
        {
            base.Awake();
            pauseBtn.onClick.AddListener(ShowPause);
            tutorialBtn.onClick.AddListener(ShowTutorial);
            startTrainBtn.onClick.AddListener(StartTrain);
            proffesorBtn.onClick.AddListener(OnProfessorBtnClick);
        }

        private void Start()
        {
        }

        private void OnProfessorBtnClick()
        {
            levelManager.ShowDialoguePage(null);
        }

        public void SetProfessorBtnActive(bool active)
        {
            proffesorBtn.gameObject.SetActive(active);
        }

        private void StartTrain()
        {
            if (levelManager.PathIsValid)
            {
                startTrainBtn.interactable = false;
                levelManager.StartTrain();
            }
            else
            {
                levelManager.ShowInvalidStartTutorial();
            }
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

        public void StartInfoButtonAnimation()
        {
            tutorialBtn.transform.DOScale(1.2f, .5f).SetEase(Ease.InOutBack).SetLoops(10, LoopType.Yoyo).onComplete +=
                () => { tutorialBtn.transform.localScale = Vector3.one; };
        }
    }
}