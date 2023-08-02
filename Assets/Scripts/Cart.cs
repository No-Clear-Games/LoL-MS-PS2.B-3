using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Cart : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("StationEnter"))
        {
            transform.DORotate(other.transform.rotation.eulerAngles, 1f, RotateMode.Fast);
        }
    }
}
