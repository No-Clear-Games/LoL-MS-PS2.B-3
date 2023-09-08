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
        [Space(4)] [SerializeField] private Button musicBtn;
        [SerializeField] private Sprite musicOnSpr, musicOffSpr;
        [SerializeField] private Image musicImg;
        [Space(4)] [SerializeField] private Button sfxBtn;
        [SerializeField] private Image sfxImg;
        [SerializeField] private Sprite sfxOnSpr, sfxOffSpr;

        private void Start()
        {
            resumeBtn.onClick.AddListener(ResumeGame);
            resetBtn.onClick.AddListener(ResetLevel);
            musicBtn.onClick.AddListener(ChangeMusicState);
            sfxBtn.onClick.AddListener(ChangeSfxState);
        }

        public override void Show(Action doneAction = null)
        {
            base.Show(doneAction);
            Application.runInBackground = false;
            musicImg.sprite = AudioManager.Instance.MuteSFX ? musicOffSpr : musicOnSpr;
        }

        public override void Hide(Action doneAction = null)
        {
            base.Hide(doneAction);
            Application.runInBackground = true;
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

        private void ChangeMusicState()
        {
            AudioManager.Instance.Mute_Music();
            musicImg.sprite = AudioManager.Instance.MuteMusic ? musicOffSpr : musicOnSpr;
            AudioManager.Instance.PlaySFX(AudioManager.Instance.SFX.clickSfx);
        }

        private void ChangeSfxState()
        {
            AudioManager.Instance.Mute_SFX();
            sfxImg.sprite = AudioManager.Instance.MuteSFX ? sfxOffSpr : sfxOnSpr;
            AudioManager.Instance.PlaySFX(AudioManager.Instance.SFX.clickSfx);
        }
    }
}