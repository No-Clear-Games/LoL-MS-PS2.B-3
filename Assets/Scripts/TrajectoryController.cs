using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(LineRenderer))]
public class TrajectoryController : MonoBehaviour
{
    [SerializeField] [Range(4, 100)] private int frameCount = 4;
    [SerializeField] [Range(1, 5)] private int frameOffset = 1;
   


    public event Action<GameObject> NewObjectAddedToSimulationScene;
    public event Action<GameObject> ObjectInSimulationSceneMoved;
    public event Action<GameObject> ProjectileChanged;
    
    
    private GameObject _projectile;
    private Scene _simulationScene;
    private PhysicsScene _physicsScene;
    private LineRenderer _lineRenderer;

    private Dictionary<Transform, Transform> _scenesGameObjectsMap;
    
    // Start is called before the first frame update
    void Awake()
    {
        _scenesGameObjectsMap = new Dictionary<Transform, Transform>();
        _lineRenderer = GetComponent<LineRenderer>();
        
        CreatePhysicsScene();
        
        

        NewObjectAddedToSimulationScene += o => SimulateTrajectory();
        ObjectInSimulationSceneMoved += o => SimulateTrajectory();
        ProjectileChanged += o => SimulateTrajectory();
    }

    public void AddObjectToSimulationScene(Transform t)
    {
        if (_scenesGameObjectsMap.ContainsKey(t))
        {
            return ;
        }
        
        GameObject go = Instantiate(t.gameObject, t.position, t.rotation);
        DisableRenderer(go);
        SceneManager.MoveGameObjectToScene(go, _simulationScene);
        _scenesGameObjectsMap[t] = go.transform;
        NewObjectAddedToSimulationScene?.Invoke(go);
    }

    public void MoveObjectInSimulationScene(Transform t)
    {
        if (!_scenesGameObjectsMap.ContainsKey(t))
        {
            return ;
        }
        
        _scenesGameObjectsMap[t] = t;
        ObjectInSimulationSceneMoved?.Invoke(t.gameObject);
    }

    public void SetProjectile(GameObject projectile)
    {
        AddObjectToSimulationScene(projectile.transform);
        
        _projectile = _scenesGameObjectsMap[projectile.transform].gameObject;
        ProjectileChanged?.Invoke(_projectile);
    }
    
    private void CreatePhysicsScene()
    {
        _simulationScene = SceneManager.CreateScene("Physics Simulation", new CreateSceneParameters(LocalPhysicsMode.Physics3D));
        _physicsScene = _simulationScene.GetPhysicsScene();
    }


    private void DisableRenderer(GameObject go)
    {

        Renderer[] renderers = go.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renderers)
        {
            r.enabled = false;
        }
    }


    public void SimulateTrajectory()
    {
        if (_projectile == null)
        {
            return;
        }

        SceneManager.SetActiveScene(_simulationScene);
        
        Debug.Log("Sim");
        
        _lineRenderer.positionCount = frameCount;

        for (int i = 0; i < frameCount; i++)
        {
            
            _lineRenderer.SetPosition(i, _projectile.transform.position);
            
            _physicsScene.Simulate(Time.fixedDeltaTime * frameOffset);
            // for (int j = 0; j < frameOffset; j++)
            // {
            //     tr.FixedUpdate();
            // }
        }
    }
}
