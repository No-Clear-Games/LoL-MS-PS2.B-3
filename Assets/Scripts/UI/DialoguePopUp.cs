using System;
using System.Text;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using NoClearGames.DialogueSystem;
using NoClearGames.Manager;
using NoClearGames.UI;
using TMPro;
using UnityEngine;
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

        private DialogueMessage _dialogueMessage;
        private int _messageId;

        [SerializeField] private float delayBetweenTyping = 0.02f;

        public void Show(DialogueMessage dialogueMessage, Action endAction)
        {
            _messageId = 0;
            this._dialogueMessage = dialogueMessage;

            dialogueMessage.onEndAction += () =>
            {
                Hide();
                endAction?.Invoke();
            };

            Show(TypingMessage().Forget);
        }

        private async UniTaskVoid TypingMessage()
        {
            titleText.text = _dialogueMessage.messages[_messageId].title;
            btnText.text = _dialogueMessage.messages[_messageId].btnMessage;
            img.sprite = _dialogueMessage.messages[_messageId].spr;

            img.gameObject.SetActive(img.sprite != null);

            nextBtn.transform.localScale = Vector3.zero;

            StringBuilder msg = new StringBuilder();

            foreach (char ctx in _dialogueMessage.messages[_messageId].message)
            {
                msg.Append(ctx.ToString());
                messageText.text = msg.ToString();
                await UniTask.Delay(TimeSpan.FromSeconds(delayBetweenTyping), ignoreTimeScale: false);
            }

            nextBtn.onClick.RemoveAllListeners();

            nextBtn.onClick.AddListener(() =>
            {
                _messageId++;

                AudioManager.Instance.PlaySFX(AudioManager.Instance.SFX.clickSfx);

                if (_messageId > _dialogueMessage.messages.Length - 1)
                {
                    _dialogueMessage.onEndAction?.Invoke();
                    return;
                }

                TypingMessage().Forget();
            });

            nextBtn.transform.DOScale(Vector3.one, .5f).SetEase(Ease.OutBounce);
        }
    }
}