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
        Sequence sequence = DOTween.Sequence();
        Vector3 firstPosition = trackLine.GetPosition(0);
        sequence.Append(trainHead.transform.DOMove(firstPosition, 1).SetSpeedBased());

        for (int i = 1; i < trackLine.positionCount; i++)
        {
            
            Vector3 position = trackLine.GetPosition(i);
            sequence.Append(trainHead.transform.DOMove(position, 1).SetEase(Ease.Linear ));
        }

        sequence.Play();
    }
}
