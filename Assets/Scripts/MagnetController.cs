using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetController : MonoBehaviour
{
    public enum PoleSign
    {
        N,
        S
    }

    [SerializeField] private PoleSign sign;
    [SerializeField] private float power = 0.1f;

    [SerializeField] private Material nPoleMaterial;
    [SerializeField] private Material sPoleMaterial;
    [SerializeField] private new Renderer renderer;


    private void OnValidate()
    {
        if (sign == PoleSign.N)
        {
            renderer.material = nPoleMaterial;
        }
        else
        {
            renderer.material = sPoleMaterial;
        }
    }

    public PoleSign Sign => sign;

    public float Power => power;


    private int CalculateForceType(PoleSign first, PoleSign second)
    {
        return first == second ? -1 : 1;
    }


    private void ImpactOnMagnet(MagnetController other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb.isKinematic)
        {
            return;
        }

        Vector3 distanceTmp = this.transform.position - other.transform.position ;
        Vector3 forceDirection = distanceTmp.normalized;
        float distance = distanceTmp.magnitude;
        int forceSign = CalculateForceType(other.Sign, this.Sign);
        rb.AddForce((this.Power * other.power * forceDirection * forceSign) / Mathf.Pow(distance, 2));
    } 
    
    
    private void ImpactOnNeutral(GameObject other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb.isKinematic)
        {
            return;
        }

        Vector3 distanceTmp = other.transform.position - this.transform.position;
        Vector3 forceDirection = distanceTmp.normalized;
        float distance = distanceTmp.magnitude;
        rb.AddForce(-(this.Power * forceDirection) / Mathf.Pow(distance, 2));
        
    } 
    



    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Magnet"))
        {
            ImpactOnMagnet(other.GetComponent<MagnetController>());
        }
        else if(other.CompareTag("Neutral"))
        {
            Debug.Log("a");
            ImpactOnNeutral(other.gameObject);
        }
    }
}
