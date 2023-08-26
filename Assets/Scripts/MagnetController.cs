using System;
using System.Collections;
using System.Collections.Generic;
using Inventory.Scripts;
using UnityEngine;

public class MagnetController : MonoBehaviour, ICollectible
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

    private string _inventoryId = "";
    public string GetInventoryId()
    {
        return _inventoryId;
    }

    public bool HasInventoryId()
    {
        return _inventoryId != "";
    }

    public void SetInventoryId(string id)
    {
        _inventoryId = id;
    }

    private void OnValidate()
    {
        renderer.material = sign == PoleSign.N ? nPoleMaterial : sPoleMaterial;
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
            ImpactOnNeutral(other.gameObject);
        }
    }
}
