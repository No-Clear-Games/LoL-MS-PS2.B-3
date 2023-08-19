using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TrainMotor : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    
    
    private Rigidbody _rigidbody;
    private Transform _transform;
    
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
    
}
