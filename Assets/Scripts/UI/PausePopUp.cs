using System;
using NoClearGames.Manager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace NoClearGames.UI
{
    public class PausePopUp : BasePage
    {
        [SerializeField] private Button resumeBtn;
        [SerializeField] private Button resetBtn;

        [Space(4)] [SerializeField] private Button sfxBtn;
        [SerializeField] private Image sfxImg;
        [SerializeField] private Sprite sfxOnSpr, sfxOffSpr;

        private void Start()
        {
            resumeBtn.onClick.AddListener(ResumeGame);
            resetBtn.onClick.AddListener(ResetLevel);
            sfxBtn.onClick.AddListener(ChangeSfxState);
        }

        public override void Show(Action doneAction = null)
        {
            base.Show(doneAction);
            sfxImg.sprite = AudioManager.Instance.MuteSFX ? sfxOffSpr : sfxOnSpr;
        }

        private void ResetLevel()
        {
            //TODO RESET LEVEL
            AudioManager.Instance.PlaySFX(AudioManager.Instance.SFX.clickSfx);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        }

        private void ResumeGame()
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.SFX.clickSfx);
            Hide();
        }

        private void ChangeSfxState()
        {
            AudioManager.Instance.Mute_SFX();
            AudioManager.Instance.Mute_Music();

            sfxImg.sprite = AudioManager.Instance.MuteSFX ? sfxOffSpr : sfxOnSpr;
            AudioManager.Instance.PlaySFX(AudioManager.Instance.SFX.clickSfx);
        }
    }
}