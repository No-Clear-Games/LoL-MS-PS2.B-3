using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

public class TtrajectoryTest : MonoBehaviour
{
    [SerializeField] private TrajectoryController trajectoryController;


    private void Start()
    {
        foreach (GameObject o in GameObject.FindGameObjectsWithTag("Simulate"))
        {
            trajectoryController.AddObjectToSimulationScene(o.transform);
        }
        
        trajectoryController.SetProjectile(GameObject.FindWithTag("Projectile"));
    }
}
