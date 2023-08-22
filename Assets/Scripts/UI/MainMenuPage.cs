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

        public override void Awake()
        {
            base.Awake();
            Show();
            startBtn.onClick.AddListener(StartGame);
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
            continueBtn.onClick.AddListener(() => { SceneManager.LoadScene(loadedPlayerState.lastSceneName, LoadSceneMode.Single); });
        }

        private void StartGame()
        {
            SceneManager.LoadScene("Level1", LoadSceneMode.Single);
            AudioManager.Instance.PlaySFX(AudioManager.Instance.SFX.clickSfx);
        }

        public class PlayerState
        {
            public string lastSceneName;
        }
    }
}