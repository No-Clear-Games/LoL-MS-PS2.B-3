using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoClearGames
{
    
    [Serializable]
    public class HintItem
    {
        [SerializeField] private MagnetSlot magnetSlot;
        [SerializeField] private MagnetController correctMagnet;
        [SerializeField] private string hintOnBlank;
        [SerializeField] private string hintOnWrong;
        [SerializeField] private string detailsOnCorrect;
        
        
        public MagnetSlot MagnetSlot => magnetSlot;
        public MagnetController CorrectMagnet => correctMagnet;
        public string HintOnBlank => hintOnBlank;
        public string HintOnWrong => hintOnWrong;
        public string DetailsOnCorrect => detailsOnCorrect;
    }
}
