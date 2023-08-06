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
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _transform = GetComponent<Transform>();
        _rigidbody.useGravity = false;
        
    }


    public void Run()
    {
        _rigidbody.AddForce(_transform.forward * speed);
    }
    
}
