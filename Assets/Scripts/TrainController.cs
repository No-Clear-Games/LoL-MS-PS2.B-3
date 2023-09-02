using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using UnityEngine.Serialization;

public class TrainController : MonoBehaviour
{
    
    
    private Vector3 _headPrevPosition;
    private Transform _headTransform;


    [SerializeField] private LineRenderer trackLine;
    [SerializeField] private GameObject[] carts;

    [SerializeField] private float startSpeed;
    
    [SerializeField] private TrainMotor motor;

    public TrainMotor Motor => motor;

    public float TrainTimeScale => _tweens[0].timeScale;
    

    public GameObject[] Carts => carts;

    private List<TweenerCore<Vector3, Path, PathOptions>> _tweens;

    public event Action<float> SpeedChanged;

    public event Action<float> TimeScaleChanged;
    public event Action TrainStarted;



    private float CalculateActualSpeed()
    {
        var position = _headTransform.position;
        float speed = (position - _headPrevPosition).magnitude;
        _headPrevPosition = position;
        SpeedChanged?.Invoke(speed);
        return speed;
    }

    public void StartTrainMove(StationController from, StationController to, Vector3[] path)
    {
        // Vector3[] positions = new Vector3[trackLine.positionCount];
        // trackLine.GetPositions(positions);

        _headTransform = carts[0].transform;
        _headPrevPosition = _headTransform.position;
        motor.MakeKinematic(true);
        _tweens = new List<TweenerCore<Vector3, Path, PathOptions>>();
        int i = 0;
        foreach (GameObject wagon in Carts)
        {
            from.GetCartPosition(i, out Vector3 start);
            to.GetCartPosition(i, out Vector3 end);
            List<Vector3> pathList = new List<Vector3> {start};
            pathList.AddRange(path);
            pathList.Add(to.EnterPoint);
            pathList.Add(end);
            
            
            TweenerCore<Vector3, Path, PathOptions> t = wagon.transform.DOPath(pathList.ToArray(), startSpeed, PathType.CatmullRom).SetSpeedBased().SetLookAt(0f).SetEase(Ease.InOutCubic);
            
            _tweens.Add(t);
            i++;
        }

        _tweens[0].onUpdate += () => CalculateActualSpeed();
        TrainStarted?.Invoke();
    }


    public void ChangeSpeed(float changePercent)
    {
            foreach (TweenerCore<Vector3,Path,PathOptions> tween in _tweens)
            {
                tween.timeScale += tween.timeScale * changePercent;
            }
            TimeScaleChanged?.Invoke(TrainTimeScale);
    }
}
