using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoClearGames
{
    
    [Serializable]
    public class HintItem
    {
        
        // public enum HintType
        // {
        //     Blank,
        //     Correct,
        //     Wrong,
        // }
        
        [SerializeField] private MagnetSlot magnetSlot;
        [SerializeField] private MagnetController correctMagnet;
        [TextArea] [SerializeField] private string hintOnBlank;
        [TextArea] [SerializeField] private string detailsOnCorrect;
        [SerializeField] private List<HintOnWrong> hintOnWrong;
        
        
         [SerializeField] private string hintOnBlankLanguageId;
         [SerializeField] private string detailsOnCorrectLanguageId;
        
        
        public MagnetSlot MagnetSlot => magnetSlot;
        public MagnetController CorrectMagnet => correctMagnet;
        public string HintOnBlank => hintOnBlank;
        public List<HintOnWrong> HintOnWrong => hintOnWrong;
        public string DetailsOnCorrect => detailsOnCorrect;
        public string HintOnBlankLanguageId => hintOnBlankLanguageId;
        public string DetailsOnCorrectLanguageId => detailsOnCorrectLanguageId;


        public static bool MagnetsEqual(MagnetController magnet1, MagnetController magnet2)
        {
            return Math.Abs(magnet1.Power - magnet2.Power) < 0.01f && magnet1.Sign == magnet2.Sign;
        }
        
        public HintOnWrong GetHintOnWrong(MagnetController wrongMagnet)
        {
            return HintOnWrong.Find(x => MagnetsEqual(x.WrongMagnet, wrongMagnet));
        }
        public bool TryGetHintString(HintData.SlotState hintType, MagnetController wrongMagnet, out string hintText)
        {
            
#if UNITY_EDITOR

            
            switch (hintType)   
            {
                case HintData.SlotState.Blank:
                {
                    hintText = hintOnBlank;
                    return true;
                }
                case HintData.SlotState.Correct:
                {
                    hintText = detailsOnCorrect;
                    return  true;
                    
                }
                case HintData.SlotState.Wrong:
                {
                    HintOnWrong how = GetHintOnWrong(wrongMagnet);
                    if (how == null)
                    {
                        hintText = "";
                        return false;
                    }

                    hintText = how.Hint;
                    return true;
                }
            }
#elif UNITY_WEBGL
            switch (hintType)   
            {
                case HintData.SlotState.Blank:
                {
                    hintText = SharedState.LanguageDefs[hintOnBlankLanguageId];
                    return true;
                }
                case HintData.SlotState.Correct:
                {
                    hintText = SharedState.LanguageDefs[detailsOnCorrectLanguageId];
                    return  true;
                }
                case HintData.SlotState.Wrong:
                {
                    HintOnWrong how = GetHintOnWrong(wrongMagnet);
                    if (how == null)
                    {
                        hintText = "";
                        return false;
                    }

                    hintText = SharedState.LanguageDefs[how.HintLanguageKey];
                    return true;
                }
            }

            
#endif
            hintText = "";
            return false;
        }
        
        public string GetHintLanguageKey(HintData.SlotState hintType, MagnetController wrongMagnet )
        {
            
            switch (hintType)   
            {
                case HintData.SlotState.Blank:
                    return hintOnBlankLanguageId;
                case HintData.SlotState.Correct:
                    return detailsOnCorrectLanguageId;
                case HintData.SlotState.Wrong:
                    return  GetHintOnWrong(wrongMagnet).HintLanguageKey;
                
            }

            return "";
        }
    }
}
