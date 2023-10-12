using System;
using System.Text;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using LoLSDK;
using NoClearGames.DialogueSystem;
using NoClearGames.Manager;
using NoClearGames.UI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace NoClearGames
{
    public partial class DialoguePopUp : BasePage
    {
        [Space] public Image img;
        public TextMeshProUGUI titleText;
        public TextMeshProUGUI messageText;
        public TextMeshProUGUI btnText;
        public Button nextBtn;
        public Button closeBtn;

        private DialogueMessage _dialogueMessage;
        private int _messageId;

        [SerializeField] private float delayBetweenTyping = 0.02f;

        private Coroutine _coroutine;

        public void Show(DialogueMessage dialogueMessage, Action endAction)
        {
            _messageId = 0;
            this._dialogueMessage = dialogueMessage;

            AudioManager.Instance.StopMusic();

            closeBtn.gameObject.SetActive(PlayerPrefs.HasKey(SceneManager.GetActiveScene().name));
            closeBtn.onClick.RemoveAllListeners();
            closeBtn.onClick.AddListener(() =>
            {
                dialogueMessage.onEndAction?.Invoke();
                AudioManager.Instance.PlaySFX(AudioManager.Instance.SFX.clickSfx);
            });

            dialogueMessage.onEndAction += () =>
            {
                Hide();
                endAction?.Invoke();
            };

            Show(() =>
            {
                if (_coroutine != null)
                {
                    StopCoroutine(_coroutine);
                }

                _coroutine = StartCoroutine(TypingMessage());
            });
        }

        private System.Collections.IEnumerator TypingMessage()
        {
#if UNITY_EDITOR
            titleText.text = _dialogueMessage.messages[_messageId].title;
            if (btnText)
                btnText.text = _dialogueMessage.messages[_messageId].btnMessage;
            string translatedMessage =
                _dialogueMessage.messages[_messageId].message;

#elif UNITY_WEBGL
            titleText.text = SharedState.LanguageDefs[_dialogueMessage.messages[_messageId].titleLanguageId];
            if (btnText)
                btnText.text = SharedState.LanguageDefs[_dialogueMessage.messages[_messageId].btnLanguageId];
            string translatedMessage =
                SharedState.LanguageDefs[_dialogueMessage.messages[_messageId].messageLanguageId];
            TextToSpeech(_dialogueMessage.messages[_messageId].messageLanguageId);
#endif
            if (img)
            {
                img.sprite = _dialogueMessage.messages[_messageId].spr;

                img.gameObject.SetActive(img.sprite != null);
            }

            nextBtn.transform.localScale = Vector3.zero;

            StringBuilder msg = new StringBuilder();

            foreach (char ctx in translatedMessage)
            {
                msg.Append(ctx.ToString());
                messageText.text = msg.ToString();
                yield return new WaitForSeconds(delayBetweenTyping);
                //await UniTask.Delay(TimeSpan.FromSeconds(delayBetweenTyping), ignoreTimeScale: false);
            }

            nextBtn.onClick.RemoveAllListeners();

            nextBtn.onClick.AddListener(() =>
            {
                _messageId++;

                AudioManager.Instance.PlaySFX(AudioManager.Instance.SFX.clickSfx);

                if (_messageId > _dialogueMessage.messages.Length - 1)
                {
                    _dialogueMessage.onEndAction?.Invoke();
                    AudioManager.Instance.PlayMusic(AudioManager.Instance.Music.inGame);

                    PlayerPrefs.SetString(SceneManager.GetActiveScene().name, "Displayed");
                    return;
                }

                if (_coroutine != null)
                {
                    StopCoroutine(_coroutine);
                }

                StartCoroutine(TypingMessage());
                // TypingMessage().Forget();
            });

            nextBtn.transform.DOScale(Vector3.one, .5f).SetEase(Ease.OutBounce);
        }

        private void TextToSpeech(string key)
        {
            LOLSDK.Instance.SpeakText(key);
        }
    }
}