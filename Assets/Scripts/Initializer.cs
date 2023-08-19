using System;
using UnityEngine;

namespace NoClearGames
{
    public class Initializer : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            Application.runInBackground = false;
        }
    }
}