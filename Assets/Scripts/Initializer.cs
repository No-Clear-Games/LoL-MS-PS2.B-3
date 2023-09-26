﻿using System;
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
            SceneManager.LoadScene(index, LoadSceneMode.Single);
            Save(new MainMenuPage.PlayerState() {lastSceneName = SceneManager.GetActiveScene().name});
        }

        public void ResetLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        }

        public void Save(MainMenuPage.PlayerState playerState)
        {
            LOLSDK.Instance.SaveState(playerState);
        }
    }
}