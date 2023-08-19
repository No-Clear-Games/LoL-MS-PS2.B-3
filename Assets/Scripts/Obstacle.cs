using System.Collections;
using System.Collections.Generic;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [Range(0, 1)] public float speedDecreasePercentage;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cart"))
        {
            Debug.Log("a");
            TrainController train = other.transform.parent.gameObject.GetComponent<TrainController>();
            train.ChangeSpeed(-speedDecreasePercentage);
            Destroy(this.gameObject);
        }
    }
}
