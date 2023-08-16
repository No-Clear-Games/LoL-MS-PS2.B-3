using System;
using System.Collections.Generic;
using UnityEngine;

namespace NoClearGames.UI
{
    public class BackManager : MonoBehaviour
    {
        private static BackManager _instance;

        public static BackManager Instance
        {
            get
            {
                if (!_instance) _instance = new GameObject("BackManager").AddComponent<BackManager>();
                return _instance;
            }
        }

        private Stack<Action> _actions = new Stack<Action>();
        public Action setAppExitPopUp;
        public bool HaveAction => _actions.Count > 0;
        public int ActionCount => _actions.Count;
        public bool allowBack = true;

        public Action AddBack
        {
            set => _actions.Push(value);
        }

        public void RemoveAllBacks() => _actions.Clear();

        public void RemoveLastBack()
        {
            if (_actions.Count > 0)
            {
                _actions.Pop();
            }
        }

        public Action ReplaceLastBack
        {
            set
            {
                RemoveLastBack();
                AddBack = value;
            }
        }

        public void ApplyBack()
        {
            if (!allowBack) return;

            if (_actions.Count == 0)
            {
                return;
            }

            if (_actions.Count <= 0) return;
            var removedItem = _actions.Pop();
            removedItem?.Invoke();
        }

        public void ApplyBackAll()
        {
            if (_actions.Count == 0) return;

            while (_actions.Count > 0)
                ApplyBack();
        }
    }
}