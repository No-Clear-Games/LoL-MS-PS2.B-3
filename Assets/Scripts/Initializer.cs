using System;
using LoLSDK;
using NoClearGames.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NoClearGames
{
    public class Initializer : Singleton<Initializer>
    {
        private Action<int> progressData;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            Application.runInBackground = false;
        }

        private void LoadProgress()
        {
        }

        public void GoToNextLevel()
        {
            var index = SceneManager.GetActiveScene().buildIndex;
            index++;

            Save(new MainMenuPage.PlayerState() {lastSceneName = index});
            if (index >= SceneManager.sceneCountInBuildSettings)
            {
                UIManager.Instance.resultPop.Hide();
                UIManager.Instance.endGamePopUp.Show();
                Debug.Log("CompleteGame");
                LOLSDK.Instance.CompleteGame();
                return;
            }

            SceneManager.LoadScene(index, LoadSceneMode.Single);
        }

        public void ResetLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        }

        public void Save(MainMenuPage.PlayerState playerState)
        {
            if (PlayerPrefs.GetInt("playerState", 2) >= playerState.lastSceneName)
            {
                return;
            }

            PlayerPrefs.SetInt("playerState", playerState.lastSceneName);

            var levelManager = FindObjectOfType<LevelManager>();

            PlayerPrefs.SetInt("Score", (int) levelManager.Score);
            LOLSDK.Instance.SubmitProgress((int) levelManager.Score, SceneManager.GetActiveScene().buildIndex - 1, SceneManager.sceneCountInBuildSettings - 2);
            LOLSDK.Instance.SaveState(playerState);
        }
    }
}