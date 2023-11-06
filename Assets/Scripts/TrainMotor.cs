using System;
using System.Collections;
using System.Collections.Generic;
using NoClearGames;
using UnityEngine;

public enum TrainModes
{
    Neutral,
    N,
    S,
}

[RequireComponent(typeof(Rigidbody))]
public class TrainMotor : MonoBehaviour, IMagnet
{
    
    [SerializeField] private float speed = 1;
    [SerializeField] private Material sMaterial;
    [SerializeField] private Material nMaterial;
    [SerializeField] private Material neutralMaterial;
    [Range(0.1f, 100)] [SerializeField] private float magnetPower;
    
    
    private Rigidbody _rigidbody;
    private Transform _transform;
    private PoleSign _poleSign;
    private TrainModes _modes;

    public TrainModes Modes => _modes;


    public void UpdateMode(TrainModes modes)
    {
        _modes = modes;
        switch (modes)
        {
            case TrainModes.Neutral:
            {
                gameObject.tag = "Neutral";
                gameObject.GetComponent<Renderer>().material = neutralMaterial;
                break;
            }
            case TrainModes.N:
            {
                gameObject.tag = "Magnet";
                _poleSign = PoleSign.N;
                gameObject.GetComponent<Renderer>().material = nMaterial;
                break;
            }
            case TrainModes.S:
            {
                gameObject.tag = "Magnet";
                _poleSign = PoleSign.S;
                gameObject.GetComponent<Renderer>().material = sMaterial;
                break;
            }
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _transform = GetComponent<Transform>();
        _rigidbody.useGravity = false;
        
    }


    public void Run()
    {
        if (_rigidbody == null)
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
        _rigidbody.velocity = _transform.forward * speed;
    }

    public void MakeKinematic(bool isKinematic)
    {
        _rigidbody.isKinematic = isKinematic;
    }

    public PoleSign GetSign()
    {
        return _poleSign;
    }

    public float GetPower()
    {
        return magnetPower;
    }
}
