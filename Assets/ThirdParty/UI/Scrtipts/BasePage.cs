using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace NoClearGames.UI
{
    public abstract class BasePage : MonoBehaviour
    {
        public enum HideType
        {
            Vertical,
            Horizontal,
        }

        [HideInInspector] public bool show;
        public GameObject root;
        public float durationTime = .6f;
        public Ease ease = Ease.InOutQuart;
        [SerializeField] protected Button backBtn;
        public GameObject[] items;
        public event Action OnShow;
        public event Action OnHide;

        private Sequence sequence;

        public virtual void Awake()
        {
            if (backBtn != null)
                backBtn.onClick.AddListener(BackManager.Instance.ApplyBack);
        }

        public virtual void Show(Action doneAction = null)
        {
            transform.SetAsLastSibling();
            if (show)
            {
                return;
            }

            if (sequence != null)
            {
                sequence.Kill();
            }

            show = true;

            root.gameObject.SetActive(true);
            foreach (GameObject item in items)
            {
                item.transform.localScale = Vector3.zero;
            }

            root.transform.localScale = Vector3.zero;
            root.transform.DOScale(Vector3.one, durationTime).SetEase(ease).onComplete += () =>
            {
                doneAction?.Invoke();
                OnShow?.Invoke();
                StartCoroutine(ScaleUp());
            };
        }

        private System.Collections.IEnumerator ScaleUp()
        {
            sequence = DOTween.Sequence();
            foreach (GameObject item in items)
            {
                sequence.Append(item.transform.DOScale(1f, .2f).SetEase(Ease.Linear));
            }

            // sequence.Play();
            yield break;
        }

        public virtual void Hide(Action doneAction = null)
        {
            if (!show)
            {
                return;
            }

            show = false;

            root.transform.DOScale(Vector3.zero, durationTime).SetEase(ease).onComplete += () =>
            {
                root.SetActive(show);
                doneAction?.Invoke();
                OnHide?.Invoke();
            };
        }

        protected virtual void ForceHide()
        {
            if (!show)
            {
                return;
            }

            show = false;

            root.SetActive(show);
        }
    }
}