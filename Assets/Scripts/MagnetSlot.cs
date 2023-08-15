using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetSlot : MonoBehaviour
{

    private bool _occupied;
    private GameObject _obj;

    public bool Occupied => _occupied;

    public void Occupy(GameObject obj)
    {
        _obj = obj;
        obj.transform.parent = this.transform;
        obj.transform.localPosition = Vector3.zero;
        _occupied = true;
    }

    public GameObject Release()
    {
        _obj.transform.parent = null;
        _occupied = false;
        GameObject tmp = _obj;
        _obj = null;
        return tmp;
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
