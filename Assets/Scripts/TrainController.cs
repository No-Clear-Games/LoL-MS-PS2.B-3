using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;

public class TrainController : MonoBehaviour
{

    [SerializeField] private LineRenderer trackLine;
    [SerializeField] private GameObject[] carts;

    [SerializeField] private float speed;
    
    [SerializeField] private TrainMotor motor;

    public TrainMotor Motor => motor;


    public GameObject[] Carts => carts;


    public void StartTrainMove(StationController from, StationController to, Vector3[] path)
    {
        // Vector3[] positions = new Vector3[trackLine.positionCount];
        // trackLine.GetPositions(positions);

        int i = 0;
        foreach (GameObject wagon in Carts)
        {
            from.GetCartPosition(i, out Vector3 start);
            to.GetCartPosition(i, out Vector3 end);
            List<Vector3> pathList = new List<Vector3> {start, from.ExitPoint};
            pathList.AddRange(path);
            pathList.Add(to.EnterPoint);
            pathList.Add(end);
            
            
            wagon.transform.DOPath(pathList.ToArray(), speed, PathType.CatmullRom).SetSpeedBased().SetLookAt(0f).SetEase(Ease.InOutCubic);
            
            i++;
        }
        
    }


  
}
