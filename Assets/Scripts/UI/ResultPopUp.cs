using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NoClearGames.UI
{
    public class ResultPopUp : BasePage
    {
        [SerializeField] private GameObject winTxt;
        [SerializeField] private GameObject loseTxt;
        [SerializeField] private TextMeshProUGUI scoreTxt;
        [SerializeField] private Button nextBtn;


        public void Win(string score)
        {
            Show();
            winTxt.SetActive(true);
            loseTxt.SetActive(false);
            scoreTxt.text = score;

            nextBtn.onClick.RemoveAllListeners();
            nextBtn.onClick.AddListener(() => { Initializer.Instance.GoToNextLevel(); });
        }

        public void Lose(string score)
        {
            Show();

            winTxt.SetActive(false);
            loseTxt.SetActive(true);
            scoreTxt.text = score;


            nextBtn.onClick.RemoveAllListeners();
            nextBtn.onClick.AddListener(() => { Initializer.Instance.ResetLevel(); });
        }
    }
}