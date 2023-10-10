using System;
using LoLSDK;
using NoClearGames.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NoClearGames
{
    public class Initializer : Singleton<Initializer>
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            Application.runInBackground = false;
        }

        public void GoToNextLevel()
        {
            var index = SceneManager.GetActiveScene().buildIndex;
            index++;
            Save(new MainMenuPage.PlayerState() {lastSceneName = SceneManager.GetActiveScene().buildIndex});
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

            LOLSDK.Instance.SaveState(playerState);
        }
    }
}