using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TrainController : MonoBehaviour
{

    [SerializeField] private LineRenderer trackLine;
    [SerializeField] private GameObject trainHead;

    // Start is called before the first frame update
    void Start()
    {
        StartTrainMove();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void StartTrainMove()
    {
        Vector3[] positions = new Vector3[trackLine.positionCount];
        trackLine.GetPositions(positions);
        Sequence sequence = DOTween.Sequence();
        Vector3 firstPosition = trackLine.GetPosition(0);
        sequence.Append(trainHead.transform.DOMove(firstPosition, 1).SetSpeedBased());
        sequence.Append(trainHead.transform.DOPath(positions, 10, PathType.CatmullRom).SetLookAt(0.1f));

        // for (int i = 1; i < trackLine.positionCount; i++)
        // {
        //     
        //     Vector3 position = trackLine.GetPosition(i);
        //     sequence.Append(trainHead.transform.DoPa)
        // }

        sequence.Play();
    }
}
