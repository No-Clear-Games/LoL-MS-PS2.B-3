using System;
using System.Collections;
using System.Collections.Generic;
using Inventory.Scripts;
using NoClearGames;
using UnityEngine;


public enum PoleSign
{
    N,
    S
}

public class MagnetController : MonoBehaviour, ICollectible, IMagnet
{
    

    [SerializeField] private PoleSign sign;
    [SerializeField] private float power = 0.1f;

    [SerializeField] private Material nPoleMaterial;
    [SerializeField] private Material sPoleMaterial;
    [SerializeField] private new Renderer renderer;
    [SerializeField] private GameObject forceField;

    private string _inventoryId = "";
    private static readonly int Speed = Shader.PropertyToID("_Speed");
    private static readonly int Invard = Shader.PropertyToID("_Invard");

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
        if(renderer)
            renderer.material = sign == PoleSign.N ? nPoleMaterial : sPoleMaterial;
    }

    private void Start()
    {
        Renderer fRenderer = forceField.GetComponent<Renderer>();
        fRenderer.material = new Material(fRenderer.material);
        fRenderer.material.SetFloat(Speed, power / 10);

        TrainModes trainMode = LevelManager.GetCurrentLevelManager().TrainMode;

        if (trainMode == TrainModes.Neutral || (trainMode == TrainModes.N && sign == PoleSign.S) ||
            (trainMode == TrainModes.S && sign == PoleSign.N))
        {
            fRenderer.material.SetFloat(Invard, 1f);
        }
        else
        {
            fRenderer.material.SetFloat(Invard, 0);
        }
    }

    public PoleSign Sign => sign;

    public float Power => power;


    private int CalculateForceType(PoleSign first, PoleSign second)
    {
        // Debug.Log($"{first} - {second}");
        return first == second ? -1 : 1;
    }


    private void ImpactOnMagnet(GameObject other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb.isKinematic)
        {
            return;
        }

        Vector3 distanceTmp = this.transform.position - other.transform.position ;
        Vector3 forceDirection = distanceTmp.normalized;
        float distance = distanceTmp.magnitude;

        IMagnet magnet = other.GetComponent<IMagnet>();
        int forceSign = CalculateForceType(magnet.GetSign(), this.Sign);
        rb.AddForce((this.Power * magnet.GetPower() * forceDirection * forceSign) / Mathf.Pow(distance, 2));
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
            ImpactOnMagnet(other.gameObject);
        }
        else if(other.CompareTag("Neutral"))
        {
            ImpactOnNeutral(other.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Magnet") || other.CompareTag("Neutral"))
        {
            Debug.Log(other.gameObject.transform.position);
        }
    }

    public PoleSign GetSign()
    {
        return sign;
    }

    public float GetPower()
    {
        return power;
    }
}
