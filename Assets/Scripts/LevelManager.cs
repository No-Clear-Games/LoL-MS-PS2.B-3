using System.Collections;
using System.Collections.Generic;
using Inventory;
using Inventory.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelManager : MonoBehaviour
{

    [SerializeField] private StationController startStation;
    [SerializeField] private StationController endStation;
    [SerializeField] private TrainController train;

    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private InventoryConfig inventoryConfig;

    [SerializeField] private TrajectoryController trajectoryController;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private DragAndDropController dragAndDropController;

    private GameInput _gameInput;
    
    // Start is called before the first frame update
    void Start()
    {
        SetupInputs();
        SetupTrain();
        SetupTrajectory();
        SetupInventory();
        SetupCameras();
        SetupDragAndDrop();
    }

    private void SetupTrain()
    {
        startStation.PlaceTrainInStation(train);
    }

    private void SetupInventory()
    {
        inventoryManager.InitInventory(inventoryConfig);
        inventoryManager.GetItemAction += InventoryManagerOnGetItemAction;
    }

    private void SetupTrajectory()
    {
        trajectoryController.ProjectileChanged += TrajectoryControllerOnProjectileChanged ;
        trajectoryController.NewObjectAddedToSimulationScene += TrajectoryControllerOnNewObjectAddedToSimulationScene;
        trajectoryController.ObjectRemovedFromSimulation += TrajectoryControllerOnObjectRemovedFromSimulation;
        
        trajectoryController.SetProjectile(train.Motor.gameObject);
    }

    private void TrajectoryControllerOnProjectileChanged(GameObject obj)
    {
        TrajectoryControllerSimulateProjectileMove();
    }

    private void TrajectoryControllerOnObjectRemovedFromSimulation(GameObject obj)
    {
        TrajectoryControllerSimulateProjectileMove();
    }

    private void TrajectoryControllerOnNewObjectAddedToSimulationScene(GameObject obj)
    {
        TrajectoryControllerSimulateProjectileMove();
    }

    private void TrajectoryControllerSimulateProjectileMove()
    {
        if (trajectoryController.HasProjectile)
        {
            trajectoryController.ResetProjectilePosition();
            TrainMotor motor = trajectoryController.Projectile.GetComponent<TrainMotor>();
            motor.Run();
            trajectoryController.SimulateTrajectory();
        }
    }

    private void SetupCameras()
    {
        cameraController.Initialize();
        _gameInput.Gameplay.ChangeCamera.performed += ChangeCameraOnPerformed;
    }

    private void SetupDragAndDrop()
    {
        dragAndDropController.Initialize();
        dragAndDropController.OccupySlotAction += DragAndDropControllerOnOccupySlotAction;
        dragAndDropController.ReleaseSlotAction += DragAndDropControllerOnReleaseSlotAction;
        dragAndDropController.StartDraggingAction += DragAndDropControllerOnStartDraggingAction;
        dragAndDropController.DropAction += DragAndDropControllerOnDropAction;
    }

    private void DragAndDropControllerOnDropAction(GameObject obj)
    {
        _gameInput.Gameplay.Click.performed -= ClickOnperformed;
    }

    private void DragAndDropControllerOnStartDraggingAction(GameObject obj)
    {
        _gameInput.Gameplay.Click.performed += ClickOnperformed;
    }

    private void ClickOnperformed(InputAction.CallbackContext obj)
    {
        dragAndDropController.Drop();
        inventoryManager.Unlock();
    }

    private void DragAndDropControllerOnReleaseSlotAction(GameObject obj)
    {
        trajectoryController.RemoveObjectFromSimulation(obj.transform);
    }

    private void DragAndDropControllerOnOccupySlotAction(GameObject obj)
    {
        trajectoryController.AddObjectToSimulationScene(obj.transform);
    }

    private void InventoryManagerOnGetItemAction(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab);
        
        StartCoroutine(dragAndDropController.Drag(obj));
        
        inventoryManager.Lock();
    }

    private void ChangeCameraOnPerformed(InputAction.CallbackContext context)
    {
        int value = (int)context.ReadValue<float>();
        cameraController.SwitchCamera(cameraController.ActiveIndex + value);
    }

    private void SetupInputs()
    {
        _gameInput = new GameInput();
        _gameInput.Gameplay.Enable();
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
