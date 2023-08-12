using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

public class StationController : MonoBehaviour
{

    [SerializeField] private GameObject exitPoint;
    [SerializeField] private GameObject enterPoint;
    [SerializeField] private float cartsOffset;
    [SerializeField] private float frontCartOffsetFromExitPoint;
    
    
    
    [Range(1, 4)] [SerializeField] private int cartCount;


    public Vector3 EnterPoint => enterPoint.transform.position;
    public Vector3 ExitPoint => exitPoint.transform.position;

    private List<Vector3> _cartPositions;
    private bool _positionsInitialized;
    

    private void SetCartPositions()
    {
        Vector3 forward = this.gameObject.transform.forward;
        
        _cartPositions = new List<Vector3>();

        for (int i = 0; i < cartCount; i++)
        {
            Vector3 p = ExitPoint - forward * (frontCartOffsetFromExitPoint + cartsOffset * i);
            _cartPositions.Add(p);
        }

        _positionsInitialized = true;
    }

    public void PlaceTrainInStation(TrainController train)
    {
        for (int i = 0; i < train.Carts.Length; i++)
        {
            if (i >= cartCount)
            {
                break;
            }
            
            if(GetCartPosition(i, out Vector3 p))
            {
                Transform t = train.Carts[i].transform;
                t.position = p;
                t.forward = this.transform.forward;
            }
        }
    }

    public bool GetCartPosition(int index, out Vector3 pos)
    {

        if (!_positionsInitialized)
        {
            SetCartPositions();
        }
        
        if (index >= _cartPositions.Count)
        {
            pos = Vector3.zero;
            return false;
        }

        pos = _cartPositions[index];
        return true;
    }
    
}
