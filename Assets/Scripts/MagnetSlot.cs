using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MagnetSlot : MonoBehaviour
{

    private bool _occupied;
    private GameObject _obj;
    private bool _highigted;
    private int _objDefaultLayer;
    private GameObject _silhouette;
    private Tween _silhouetteTween;

    private static Dictionary<string, GameObject> _silhouetteCache = new Dictionary<string, GameObject>();

    public bool Occupied => _occupied;
    

    public void Occupy(GameObject obj)
    {
        _obj = obj;
        obj.transform.parent = this.transform;
        obj.transform.localPosition = Vector3.zero;
        _highigted = obj.layer == LayerMask.NameToLayer("Highlighted");
        _occupied = true;
    }

    public GameObject Release()
    {
        _obj.transform.parent = null;
        _occupied = false;
        _highigted = false;
        GameObject tmp = _obj;
        _obj = null;
        return tmp;
    }

    public void HighlightObject(bool highlight)
    {
        if (_highigted == highlight || _obj == null)
        {
            return;
        }

        if(highlight)
        {
            _objDefaultLayer = _obj.layer;
            foreach (Transform child in _obj.GetComponentsInChildren<Transform>())
            {
                child.gameObject.layer = LayerMask.NameToLayer("Highlighted");
            }
        }
        else
        {
            foreach (Transform child in _obj.GetComponentsInChildren<Transform>())
            {
                child.gameObject.layer = _objDefaultLayer;
            }
        }

        _highigted = highlight;
    }

    private GameObject GetSilhouette(GameObject obj)
    {
        if (_silhouetteCache.ContainsKey(obj.name) && _silhouetteCache[obj.name] != null)
        {
            return _silhouetteCache[obj.name];
        }
        
        GameObject sCache = Instantiate(obj);
        sCache.SetActive(false);
        sCache.transform.localPosition = Vector3.zero;
        foreach (Component component in sCache.GetComponents<Component>())
        {
            if (!(component is Renderer || component is MeshFilter || component is Transform))
            {
                Destroy(component);
            }
        }
        foreach (Transform child in sCache.GetComponentsInChildren<Transform>())
        {
            child.gameObject.layer = LayerMask.NameToLayer("SlotHL");
        }

        _silhouetteCache[obj.name] = sCache;
        return sCache;
    }

    public void BlinkSilhouetteStart(GameObject obj)
    {
        if (_occupied)
        {
            return;
        }
        
        GameObject s = GetSilhouette(obj);
        _silhouette = Instantiate(s, transform, false);
        _silhouette.SetActive(true);
        _silhouette.transform.localScale = Vector3.one / transform.lossyScale.x;
        _silhouetteTween = _silhouette.transform.DOScale(Vector3.one * 1.5f, 1f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

    public void BlinkSilhouetteStop()
    {
        _silhouetteTween.Complete();
        Destroy(_silhouette);
        _silhouette = null;
    }

}
