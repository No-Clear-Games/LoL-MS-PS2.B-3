using System;
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
        }

        public void ResetLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        }
    }
}