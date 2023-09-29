using System;
using NoClearGames.Manager;
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

        [SerializeField] private Image btnImg;
        [SerializeField] private Sprite retry, next;

        public void Win(string score)
        {
            Show();
            winTxt.SetActive(true);
            loseTxt.SetActive(false);
            scoreTxt.text = score;

            btnImg.sprite = next;

            nextBtn.onClick.RemoveAllListeners();
            nextBtn.onClick.AddListener(() =>
            {
                Initializer.Instance.GoToNextLevel();
                AudioManager.Instance.PlaySFX(AudioManager.Instance.SFX.clickSfx);
            });
        }

        public void Lose(string score)
        {
            Show();

            winTxt.SetActive(false);
            loseTxt.SetActive(true);
            scoreTxt.text = score;

            btnImg.sprite = retry;

            nextBtn.onClick.RemoveAllListeners();
            nextBtn.onClick.AddListener(() =>
            {
                Initializer.Instance.ResetLevel();
                AudioManager.Instance.PlaySFX(AudioManager.Instance.SFX.clickSfx);
            });
        }
    }
}