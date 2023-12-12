using System;
using DG.Tweening;
using NoClearGames.Manager;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace NoClearGames.UI
{
    public class HUDPage : BasePage
    {
        [SerializeField] private Button pauseBtn;
        [SerializeField] private Button tutorialBtn;
        [SerializeField] private Button startTrainBtn;
        [FormerlySerializedAs("proffesorBtn")] [SerializeField] private Button professorBtn;
        [SerializeField] private Button hintBtn;


        [SerializeField] private LevelManager levelManager;

        public event Action HintButtonClickAction;

        public override void Awake()
        {
            base.Awake();
            pauseBtn.onClick.AddListener(ShowPause);
            tutorialBtn.onClick.AddListener(ShowTutorial);
            startTrainBtn.onClick.AddListener(StartTrain);
            professorBtn.onClick.AddListener(OnProfessorBtnClick);
            hintBtn.onClick.AddListener(OnHintButtonClicked);
            SetHintBtnActive(false);
        }

        public void StartBtnStartBlinking()
        {
            startTrainBtn.transform.DOScale(Vector3.one * 1.2f, 0.5f).SetLoops(-1, LoopType.Yoyo);
        }
        
        public void StartBtnStopBlinking()
        {
            startTrainBtn.transform.DOKill();
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
            professorBtn.gameObject.SetActive(active);
        }
        
        public void SetHintBtnActive(bool active)
        {
            if (hintBtn.gameObject.activeSelf == active)
            {
                return;
            }
            
            hintBtn.gameObject.SetActive(active);

            if (active)
            {
                hintBtn.transform.DOScale(Vector3.one * 1.2f, 0.5f).SetLoops(-1, LoopType.Yoyo);
            }
            else
            {
                Transform transform1;
                (transform1 = hintBtn.transform).DOKill();
                transform1.localScale = Vector3.one;
            }
        }

        private void StartTrain()
        {
            if (levelManager.PathIsValid)
            {
                startTrainBtn.interactable = false;
                levelManager.StartTrain();
                StartBtnStopBlinking();
            }
            else
            {
                levelManager.ShowInvalidStartTutorial();
            }
        }

        private void OnHintButtonClicked()
        {
            HintButtonClickAction?.Invoke();
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

        private void OnDestroy()
        {
            
        }
    }
}