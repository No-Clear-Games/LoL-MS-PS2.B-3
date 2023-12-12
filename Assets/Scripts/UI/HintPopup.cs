using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NoClearGames.Manager;
using NoClearGames.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using LoLSDK;
using TMPro;
using UnityEngine.UI;

namespace NoClearGames
{
    public class HintPopup : BasePage
    {
        [SerializeField] private Image CorrectIcon;
        [SerializeField] private Image WrongIcon;
        [SerializeField] private Image HintIcon;

        public TextMeshProUGUI messageText;

        public float hidePositionX;
        public float moveDuration = 0.1f;
        public float showDuration = 5;

        private Coroutine _waitForHideCoroutine;
        private Vector3 _defaultPosition;
        private bool _defaultPosSet = false;
        private Tween tweener;

        public void MoveRight()
        {
            var transform1 = transform;
            if (!_defaultPosSet)
            {
                _defaultPosSet = true;
                _defaultPosition = transform1.position;
            }

            Vector3 hidePos = _defaultPosition;
            hidePos.x = hidePositionX;

            transform1.position = hidePos;

            transform1.DOMove(_defaultPosition, moveDuration);
        }

        public Tween MoveLeft(bool forceDuration = false)
        {
            var transform1 = transform;
            var defaultPosition = transform1.position;

            Vector3 hidePos = defaultPosition;
            hidePos.x = hidePositionX;
            return transform1.DOMove(hidePos, forceDuration ? 0 : moveDuration);
        }

        public void Show(HintItem hintItem, HintData.SlotState hintType, MagnetController wrongMagnet)
        {
            MoveLeft(true);
            tweener?.Kill();
            ForceHide();

            if (!_defaultPosSet)
            {
                _defaultPosSet = true;
                var transform1 = transform;
                _defaultPosition = transform1.position;
                Vector3 hidePos = _defaultPosition;
                hidePos.x = hidePositionX;

                transform1.position = hidePos;
            }


            CorrectIcon.gameObject.SetActive(hintType == HintData.SlotState.Correct);
            WrongIcon.gameObject.SetActive(hintType == HintData.SlotState.Wrong);
            HintIcon.gameObject.SetActive(hintType == HintData.SlotState.Blank);


            if (!SetMessage(hintItem, hintType, wrongMagnet))
            {
                return;
            }

            AudioManager.Instance.StopMusic();
            // AudioManager.Instance.PauseSounds();
            if (_waitForHideCoroutine != null)
            {
                StopCoroutine(_waitForHideCoroutine);
                _waitForHideCoroutine = null;
                    
            }

            Show(doneAction: (() =>
            {

                

                MoveRight();
                StartCoroutine(WaitForHide());
            }));
        }

        private IEnumerator WaitForHide()
        {
            yield return new WaitForSeconds(showDuration);
            Hide();
            _waitForHideCoroutine = null;
        }

        public override void Hide(Action doneAction = null)
        {
            tweener = MoveLeft().OnComplete(() =>
            {
                AudioManager.Instance.UnPauseSounds();
                AudioManager.Instance.PlaySFX(AudioManager.Instance.SFX.clickSfx);
                AudioManager.Instance.PlayMusic(AudioManager.Instance.Music.inGame);
                base.Hide(doneAction);
            });
        }

        private bool SetMessage(HintItem hintItem, HintData.SlotState hintType, MagnetController wrongMagnet)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            string hintLanguageKey = hintItem.GetHintLanguageKey(hintType, wrongMagnet);
            LOLSDK.Instance.SpeakText(hintLanguageKey);
#endif
            if (!hintItem.TryGetHintString(hintType, wrongMagnet, out string hintText))
            {
                return false;
            }

            messageText.text = hintText;
            return true;
        }
    }
}