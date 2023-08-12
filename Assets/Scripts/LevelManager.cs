using System.Collections;
using System.Collections.Generic;
using Inventory;
using Inventory.Scripts;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    [SerializeField] private StationController startStation;
    [SerializeField] private StationController endStation;
    [SerializeField] private TrainController train;

    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private InventoryConfig inventoryConfig;

    [SerializeField] private TrajectoryController trajectoryController;
    
    // Start is called before the first frame update
    void Start()
    {
        SetupTrain();
        SetupTrajectory();
        SetupInventory();
    }

    private void SetupTrain()
    {
        startStation.PlaceTrainInStation(train);
    }

    private void SetupInventory()
    {
        inventoryManager.InitInventory(inventoryConfig);
    }

    private void SetupTrajectory()
    {
        trajectoryController.ProjectileChanged += TrajectoryControllerOnProjectileChanged;
        
        trajectoryController.SetProjectile(train.Motor.gameObject);
    }

    private void TrajectoryControllerOnProjectileChanged(GameObject projectile)
    {
        TrainMotor motor = projectile.GetComponent<TrainMotor>();
        motor.Run();
        trajectoryController.SimulateTrajectory();
    }

    private void SimulatePath()
    {
        
    }

    private void StartTrain()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
