using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoClearGames
{
    [Serializable]
    public class HintOnWrong
    {
        [SerializeField] private MagnetController wrongMagnet;
        [SerializeField] [TextArea] private string hint;
        [SerializeField] private string hintLanguageKey;

        public MagnetController WrongMagnet => wrongMagnet;
        public string Hint => hint;
        public string HintLanguageKey => hintLanguageKey;
    }
}
