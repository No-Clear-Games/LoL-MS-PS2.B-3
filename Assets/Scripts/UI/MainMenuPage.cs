using System;
using DG.Tweening;
using NoClearGames.Manager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SimpleJSON;
using LoLSDK;

namespace NoClearGames.UI
{
    public class MainMenuPage : BasePage
    {
        [SerializeField] private Button startBtn;
        [SerializeField] private Button continueBtn;

        private Sequence sequence;

        public override void Awake()
        {
            base.Awake();
            Show();
            startBtn.onClick.AddListener(() => { StartGame("Level 1"); });

            sequence = DOTween.Sequence();

            sequence.Append(startBtn.transform.DOScale(1.1f, 1f).SetEase(Ease.InBack).SetLoops(-1, LoopType.Yoyo));
            sequence.Append(continueBtn.transform.DOScale(1.1f, 1f).SetEase(Ease.InBack).SetLoops(-1, LoopType.Yoyo));
        }

        public override void Show(Action doneAction = null)
        {
            base.Show(doneAction);
            AudioManager.Instance.PlayMusic(AudioManager.Instance.Music.menu);
        }

        public override void Hide(Action doneAction = null)
        {
            base.Hide(doneAction);
            sequence?.Kill();
        }

        private void Start()
        {
            Helper.StateButtonInitialize<PlayerState>(startBtn, continueBtn, OnLoad);
        }

        private void OnLoad(PlayerState loadedPlayerState)
        {
            if (loadedPlayerState == null)
            {
                Debug.Log("no data is loaded!");
                return;
            }

            continueBtn.onClick.RemoveAllListeners();
            continueBtn.onClick.AddListener(() =>
            {
                Hide();
                SceneManager.LoadScene(loadedPlayerState.lastSceneName, LoadSceneMode.Single);
            });
        }

        private void StartGame(string level)
        {
            Hide();
            SceneManager.LoadScene(level, LoadSceneMode.Single);
            AudioManager.Instance.PlaySFX(AudioManager.Instance.SFX.clickSfx);
        }

        public class PlayerState
        {
            public string lastSceneName;
        }
    }
}