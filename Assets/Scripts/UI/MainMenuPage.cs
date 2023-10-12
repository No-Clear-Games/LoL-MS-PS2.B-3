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
            startBtn.onClick.AddListener(() => { StartGame(2); });

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
            LOLSDK.Instance.LoadState<PlayerState>(callback =>
            {
                _playerState = callback.data;

                continueBtn.gameObject.SetActive(_playerState.lastSceneName > 2);

                continueBtn.onClick.RemoveAllListeners();
                continueBtn.onClick.AddListener(() =>
                {
                    Debug.Log(_playerState.lastSceneName);
                    StartGame(_playerState.lastSceneName);
                    Hide();
                });
            });
            //Helper.StateButtonInitialize<PlayerState>(startBtn, continueBtn, OnLoad);
        }

        private PlayerState _playerState;

        private void OnLoad(PlayerState loadedPlayerState)
        {
            if (loadedPlayerState == null)
            {
                Debug.Log("no data is loaded!");
                return;
            }

            _playerState = loadedPlayerState;
        }

        private void StartGame(int level)
        {
            Hide();
            SceneManager.LoadScene(level, LoadSceneMode.Single);
            AudioManager.Instance.PlaySFX(AudioManager.Instance.SFX.clickSfx);
        }

        public class PlayerState
        {
            public int lastSceneName;
        }
    }
}