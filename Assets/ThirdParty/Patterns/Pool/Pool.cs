using System.Collections.Generic;
using UnityEngine;

namespace NoClearGames.Patterns.Pool
{
    [System.Serializable]
    public class Pool<T> where T : Component
    {
        [SerializeField] private Transform parent;
        [SerializeField] private T prefab;
        private List<T> _items = new List<T>();
        private int _maxSize = 50;

        public T[] ActiveItems
        {
            get
            {
                List<T> activeItems = new List<T>();

                foreach (T item in _items)
                    if (item.gameObject.activeSelf)
                        activeItems.Add(item);

                return activeItems.ToArray();
            }
        }

        public T Get
        {
            get
            {
                // Review Objects Count

                foreach (T item in _items)
                    if (!item.gameObject.activeSelf)
                        return item;


                return AddNewItem();
            }
        }

        public T GetActive
        {
            get
            {
                T item = Get;
                item.gameObject.SetActive(true);
                return item;
            }
        }

        private T AddNewItem()
        {
            T item = GameObject.Instantiate(prefab, parent);
            Init(item);
            item.transform.localPosition = Vector3.zero;
            item.transform.localRotation = Quaternion.identity;
            _items.Add(item);
            item.name = typeof(T).Name + "_" + _items.Count;
            return item;
        }

        public void TurnOffItems()
        {
            foreach (T item in _items)
                if (item.gameObject.activeSelf)
                    item.gameObject.SetActive(false);
        }

        protected virtual void Init(T item)
        {
        }

        public void SetMaxSize(int maxSize) => this._maxSize = maxSize;

        public void SetPrefab(T prefab)
        {
            this.prefab = prefab;
        }
    }
}