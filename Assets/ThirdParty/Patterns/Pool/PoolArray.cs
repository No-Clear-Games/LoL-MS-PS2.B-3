using System.Collections.Generic;
using UnityEngine;

namespace NoClearGames.Patterns.Pool
{
    [System.Serializable]
    public class PoolArray<T> where T : Component
    {
        [SerializeField] private Transform parent;
        [SerializeField] private T[] prefab;
        private List<T> _items = new List<T>();

        public T[] ActiveItems
        {
            get
            {
                List<T> activeItems = new List<T>();

                for (int i = 0; i < _items.Count; i++)
                    if (_items[i].gameObject.activeSelf)
                        activeItems.Add(_items[i]);

                return activeItems.ToArray();
            }
        }

        public T Get
        {
            get
            {
                for (int i = 0; i < _items.Count; i++)
                    if (!_items[i].gameObject.activeSelf)
                        return _items[i];


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
            if (_items.Count == 0)
                InitialNewItems();

            T item = GameObject.Instantiate(prefab[Random.Range(0, prefab.Length)], parent);
            Init(item);
            item.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            _items.Add(item);
            item.name = _items.Count + "_" + item.name;
            return item;
        }

        private void InitialNewItems()
        {
            for (int i = 0; i < prefab.Length; i++)
                AddNewItem(i).gameObject.SetActive(false);
        }

        private T AddNewItem(int prefabIndex)
        {
            T item = GameObject.Instantiate(prefab[Random.Range(0, prefab.Length)], parent);
            Init(item);
            item.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            _items.Add(item);
            item.name = _items.Count + "_" + item.name;
            return item;
        }

        public void DeactiveItems()
        {
            for (int i = 0; i < _items.Count; i++)
                if (_items[i].gameObject.activeSelf)
                    _items[i].gameObject.SetActive(false);
        }

        protected virtual void Init(T item)
        {
        }

        protected virtual void InitializeOnGet(T item)
        {
        }
    }
}