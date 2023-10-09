using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using DG.Tweening;
using Inventory;
using Inventory.Scripts;
using NoClearGames;
using NoClearGames.DialogueSystem;
using NoClearGames.Manager;
using NoClearGames.UI;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;

    [SerializeField] private StationController startStation;
    [SerializeField] private StationController endStation;
    [SerializeField] private TrainController train;

    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private InventoryConfig inventoryConfig;

    [SerializeField] private TrajectoryController trajectoryController;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private DragAndDropController dragAndDropController;
    [SerializeField] private AllowedArea allowedArea;
    [SerializeField] private Button startButton;
    [SerializeField] private Material obstacleMaterial;
    [SerializeField] private DialoguePopUp tutorialPopUp;
    [SerializeField] private DialoguePopUp dialoguePopUp;
    [SerializeField] private LevelDialogueData tutorialDialogueData;
    [SerializeField] private LevelDialogueData levelDialogueData;

    [Range(0, 1)] [SerializeField] private float minimumTimeScale;
    [SerializeField] private float scoreScale = 1;
    [SerializeField] private float obstaclesDissolveDuration = 1;
    [SerializeField] private TrainModes trainMode;

    private GameInput _gameInput;
    private Camera _mainCamera => Camera.main;
    private float _score;
    private bool _lost;
    private bool _pathIsValid;


    public static LevelManager GetCurrentLevelManager()
    {
        return GameObject.FindWithTag("LevelManager").GetComponent<LevelManager>();
    }

    public void ShowTutorialPage()
    {
        tutorialPopUp.Show(tutorialDialogueData.GetDialogue("tutorial"), null);
    }

    public void ShowDialoguePage()
    {
        dialoguePopUp.Show(levelDialogueData.GetDialogue("start"), null);
        dialoguePopUp.MoveUp();
    }

    public float Score => _score;

    public TrainModes TrainMode => trainMode;

    public event Action PlayerWon;
    public event Action PlayerLost;

    // Start is called before the first frame update
    void Start()
    {
        _lost = false;
        SetupObstacles();
        SetupInputs();
        SetupTrain();
        SetupTrajectory();
        SetupInventory();
        SetupCameras();
        SetupDragAndDrop();
        ShowDialoguePage();

        AudioManager.Instance.PlayMusic(AudioManager.Instance.Music.inGame);


        PlayerWon += () =>
        {
            UIManager.Instance.resultPop.Win(Score.ToString());
            AudioManager.Instance.PlaySFX(AudioManager.Instance.SFX.winSfx);
        };
        PlayerLost += () =>
        {
            UIManager.Instance.resultPop.Lose(Score.ToString());
            AudioManager.Instance.PlaySFX(AudioManager.Instance.SFX.loseSfx);
        };
    }

    private void OnValidate()
    {
        if (train != null) train.Motor.UpdateMode(trainMode);
    }

    private void SetupObstacles()
    {
        obstacleMaterial.SetFloat(DissolveProgress, 1);
        obstacleMaterial.DOFloat(0, DissolveProgress, obstaclesDissolveDuration);
    }

    private void SetupTrain()
    {
        startStation.PlaceTrainInStation(train);
        endStation.TrainEntered += EndStationOnTrainEntered;
        train.SpeedChanged += CalcAndSetScore;
        train.TimeScaleChanged += CheckLose;
        Debug.Log("Train setup succeeded");
    }

    private void EndStationOnTrainEntered()
    {
        Win();
    }

    private void CheckLose(float timeScale)
    {
        if (timeScale < minimumTimeScale && !_lost)
        {
            _lost = true;
            PlayerLost?.Invoke();
            ;
            Debug.Log("Lost");
        }
    }

    private void Win()
    {
        if (!_lost)
        {
            PlayerWon?.Invoke();
            Debug.Log("Won");
        }
    }


    private float _tmpScore;
    private static readonly int DissolveProgress = Shader.PropertyToID("_Dissolve_Progress");

    private void CalcAndSetScore(float trainVelocity)
    {
        _tmpScore = Mathf.Max(trainVelocity * scoreScale, _tmpScore);
        _score = _tmpScore * train.TrainTimeScale;
        scoreText.text = ((int) _score).ToString(CultureInfo.InvariantCulture);
    }


    private void SetupInventory()
    {
        inventoryManager.InitInventory(inventoryConfig);
        inventoryManager.GetItemAction += InventoryManagerOnGetItemAction;
        Debug.Log("Inventory setup succeeded");
    }

    private void SetupTrajectory()
    {
        trajectoryController.ProjectileChanged += TrajectoryControllerOnProjectileChanged;
        trajectoryController.NewObjectAddedToSimulationScene += TrajectoryControllerOnNewObjectAddedToSimulationScene;
        trajectoryController.ObjectRemovedFromSimulation += TrajectoryControllerOnObjectRemovedFromSimulation;
        AllowedArea aArea = trajectoryController.AddObjectToSimulationScene(allowedArea.transform).GetComponent<AllowedArea>();
        aArea.TrainTouched += AAreaOnTrainTouched;
        trajectoryController.StartSimulation += TrajectoryControllerOnStartSimulation;
        trajectoryController.EndSimulation += TrajectoryControllerOnEndSimulation;
        trajectoryController.SetProjectile(train.Motor.gameObject);

        Debug.Log("Trajectory setup succeeded");
    }

    private void TrajectoryControllerOnEndSimulation()
    {
        if (_pathIsValid)
        {
            EnableStartTrain();
        }
        else
        {
            DisableStartTrain();
        }
    }

    private void TrajectoryControllerOnStartSimulation()
    {
        _pathIsValid = false;
    }

    private void AAreaOnTrainTouched()
    {
        _pathIsValid = true;
        trajectoryController.ForceFinishSimulation();
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

    private void DisableStartTrain()
    {
        startButton.interactable = false;
        trajectoryController.ShowErrorMode();
    }

    private void EnableStartTrain()
    {
        startButton.interactable = true;
        trajectoryController.ShowDefaultMode();
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
        Debug.Log("Cam setup succeeded");
    }

    private void SetupDragAndDrop()
    {
        dragAndDropController.Initialize();
        dragAndDropController.OccupySlotAction += DragAndDropControllerOnOccupySlotAction;
        dragAndDropController.ReleaseSlotAction += DragAndDropControllerOnReleaseSlotAction;
        dragAndDropController.StartDraggingAction += DragAndDropControllerOnStartDraggingAction;
        dragAndDropController.DropAction += DragAndDropControllerOnDropAction;
        dragAndDropController.CancelAction += DragAndDropControllerOnCancelAction;
        _gameInput.Gameplay.Click.performed += DragOnClick;

        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnSceneUnloaded(Scene arg0)
    {
        dragAndDropController.OccupySlotAction -= DragAndDropControllerOnOccupySlotAction;
        dragAndDropController.ReleaseSlotAction -= DragAndDropControllerOnReleaseSlotAction;
        dragAndDropController.StartDraggingAction -= DragAndDropControllerOnStartDraggingAction;
        dragAndDropController.DropAction -= DragAndDropControllerOnDropAction;
        dragAndDropController.CancelAction -= DragAndDropControllerOnCancelAction;
        _gameInput.Gameplay.Click.performed -= DragOnClick;
    }

    private void DragAndDropControllerOnCancelAction(GameObject obj)
    {
        ICollectible collectible = obj.GetComponent<ICollectible>();
        if (inventoryConfig.GetInventorySupply(collectible.GetInventoryId(), out InventorySupply supply))
        {
            inventoryManager.AddItem(supply.item, 1);
        }
        else
        {
            Debug.LogError("Error Getting Inventory item");
        }

        Destroy(obj);

        inventoryManager.Unlock();
        _gameInput.Gameplay.Click.performed -= DropOnClick;
        _gameInput.Gameplay.RightClick.performed -= RightClickOnPerformed;
        _gameInput.Gameplay.Click.performed += DragOnClick;
        StartCoroutine(HighLightOnHoverSlot());
    }

    private void DragAndDropControllerOnDropAction(GameObject obj)
    {
        _gameInput.Gameplay.Click.performed -= DropOnClick;
        _gameInput.Gameplay.RightClick.performed -= RightClickOnPerformed;
        _gameInput.Gameplay.Click.performed += DragOnClick;
        StartCoroutine(HighLightOnHoverSlot());
    }

    private void DragAndDropControllerOnStartDraggingAction(GameObject obj)
    {
        _gameInput.Gameplay.Click.performed -= DragOnClick;

        _gameInput.Gameplay.Click.performed += DropOnClick;
        _gameInput.Gameplay.RightClick.performed += RightClickOnPerformed;
    }

    private void RightClickOnPerformed(InputAction.CallbackContext context)
    {
        dragAndDropController.CancelDrag();
    }


    private void DropOnClick(InputAction.CallbackContext obj)
    {
        dragAndDropController.TryDrop();
        inventoryManager.Unlock();
    }

    private void DragOnClick(InputAction.CallbackContext context)
    {
        Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit, 100, LayerMask.GetMask("Slots")))
        {
            MagnetSlot slot = hit.collider.gameObject.GetComponent<MagnetSlot>();
            if (!slot.Occupied)
            {
                return;
            }

            GameObject obj = slot.Release();

            StartCoroutine(dragAndDropController.Drag(obj));
            inventoryManager.Lock();
        }
    }

    private IEnumerator HighLightOnHoverSlot()
    {
        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
        GameObject lastHoveredSlot = null;

        while (dragAndDropController.DraggingState != DragAndDropController.State.Dragging)
        {
            yield return waitForEndOfFrame;
            Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit, 100, LayerMask.GetMask("Slots")))
            {
                if (lastHoveredSlot == hit.collider.gameObject)
                {
                    continue;
                }

                if (lastHoveredSlot != null)
                {
                    lastHoveredSlot.GetComponent<MagnetSlot>().HighlightObject(false);
                }


                MagnetSlot slot = hit.collider.gameObject.GetComponent<MagnetSlot>();
                if (!slot.Occupied)
                {
                    continue;
                }


                slot.HighlightObject(true);


                lastHoveredSlot = hit.collider.gameObject;
            }
            else
            {
                if (lastHoveredSlot != null)
                {
                    lastHoveredSlot.GetComponent<MagnetSlot>().HighlightObject(false);
                    lastHoveredSlot = null;
                }
            }
        }
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

        obj.GetComponent<ICollectible>().SetInventoryId(InventorySupply.GetItemId(prefab));


        StartCoroutine(dragAndDropController.Drag(obj));

        inventoryManager.Lock();
    }

    private void ChangeCameraOnPerformed(InputAction.CallbackContext context)
    {
        int value = (int) context.ReadValue<float>();
        cameraController.SwitchCamera(cameraController.ActiveIndex + value);
    }

    private void SetupInputs()
    {
        _gameInput = new GameInput();
        _gameInput.Gameplay.Enable();
        Debug.Log("Inputs setup succeeded");
    }


    public void StartTrain()
    {
        Vector3[] path = trajectoryController.GetPath();

        train.StartTrainMove(startStation, endStation, path);
    }
}