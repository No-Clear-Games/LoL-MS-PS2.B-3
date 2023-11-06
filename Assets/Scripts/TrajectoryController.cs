using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(LineRenderer))]
public class  TrajectoryController : MonoBehaviour
{
    [SerializeField] [Range(4, 400)] private int frameCount = 4;
    [SerializeField] [Range(1, 3)] private int frameOffset = 1;


    [SerializeField] private float distanceLimit;

    [SerializeField] private Material errorMaterial;
    [SerializeField] private Material defaultMaterial;

    
    public event Action<GameObject> NewObjectAddedToSimulationScene;
    public event Action<GameObject> ObjectRemovedFromSimulation;
    public event Action<GameObject> ObjectInSimulationSceneMoved;
    public event Action<GameObject> ProjectileChanged;

    public event Action StartSimulation;
    public event Action EndSimulation;
    
    
    private GameObject _projectile;
    private GameObject _originalProjectile;
    private Scene _simulationScene;
    private PhysicsScene _physicsScene;
    private LineRenderer _lineRenderer;
    private bool _hasProjectile;
    private bool _forceFinish;
    private WaitForEndOfFrame _waitForEndOfFrame = new WaitForEndOfFrame();

    public GameObject Projectile => _projectile;
    

    public bool HasProjectile => _hasProjectile;

    private Dictionary<Transform, Transform> _scenesGameObjectsMap;
    
    // Start is called before the first frame update
    void Awake()
    {
        _scenesGameObjectsMap = new Dictionary<Transform, Transform>();
        _lineRenderer = GetComponent<LineRenderer>();
        
        CreatePhysicsScene();
        
        

        // NewObjectAddedToSimulationScene += o => SimulateTrajectory();
        // ObjectInSimulationSceneMoved += o => SimulateTrajectory();
        // ProjectileChanged += o => SimulateTrajectory();
    }

    public void ShowErrorMode()
    {
        _lineRenderer.material = errorMaterial;
    }
    
    public void ShowDefaultMode()
    {
        _lineRenderer.material = defaultMaterial;
    }

    public GameObject AddObjectToSimulationScene(Transform t)
    {
        if (_scenesGameObjectsMap.ContainsKey(t))
        {
            return _scenesGameObjectsMap[t].gameObject;
        }
        
        GameObject go = Instantiate(t.gameObject, t.position, t.rotation);
        DisableRenderer(go);
        SceneManager.MoveGameObjectToScene(go, _simulationScene);
        _scenesGameObjectsMap[t] = go.transform;
        NewObjectAddedToSimulationScene?.Invoke(go);
        return go;
    }

    public void RemoveObjectFromSimulation(Transform t)
    {
        StartCoroutine(RemoveObjectRoutine(t));
    }

    private IEnumerator RemoveObjectRoutine(Transform t)
    {
        if (_scenesGameObjectsMap.ContainsKey(t))
        {
            _scenesGameObjectsMap[t].gameObject.SetActive(false);
            Destroy(_scenesGameObjectsMap[t].gameObject);
            _scenesGameObjectsMap.Remove(t);
            ObjectRemovedFromSimulation?.Invoke(t.gameObject);
            yield return _waitForEndOfFrame;
        }
    }

    public Vector3[] GetPath()
    {
        Vector3[] arr = new Vector3[_lineRenderer.positionCount];
        _lineRenderer.GetPositions(arr);
        return arr;
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
        _originalProjectile = projectile;
        _hasProjectile = true;
        ProjectileChanged?.Invoke(_projectile);
    }

    public void ResetProjectilePosition()
    {
        _projectile.transform.position = _originalProjectile.transform.position;
        
        Rigidbody rb = _projectile.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.AddForce(-rb.GetAccumulatedForce());
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

    public void ForceFinishSimulation()
    {
        _forceFinish = true;
    }


    public void SimulateTrajectory()
    {
        if (_projectile == null)
        {
            return;
        }

        _forceFinish = false;
        StartSimulation?.Invoke();

        Scene mainScene = SceneManager.GetActiveScene();
        SceneManager.SetActiveScene(_simulationScene);
        
        
        _lineRenderer.positionCount = frameCount;

        Vector3 firstPos = _projectile.transform.position;
        Vector3 forward = _projectile.transform.forward;

        for (int i = 0; i < frameCount; i++)
        {
            Vector3 pos = _projectile.transform.position;

            if (_forceFinish || distanceLimit > 0 && Vector3.Dot(pos - firstPos, forward) > distanceLimit)
            {
                _lineRenderer.positionCount = i;
                break;
            }
            
            _lineRenderer.SetPosition(i, pos);
            
            _physicsScene.Simulate(Time.fixedDeltaTime * frameOffset);
        }

        SceneManager.SetActiveScene(mainScene);
        
        EndSimulation?.Invoke();
    }
}
