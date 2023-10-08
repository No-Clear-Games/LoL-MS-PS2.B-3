using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetSlot : MonoBehaviour
{

    private bool _occupied;
    private GameObject _obj;
    private bool _highigted;
    private int _objDefaultLayer;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
