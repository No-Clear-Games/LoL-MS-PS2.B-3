using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
// using Cinemachine;
using UnityEngine;

[Serializable]
public class CameraController
{
    [SerializeField] private Transform[] virtualCameras;
    private int _activeIndex;
    [SerializeField] private Camera camera;

    public int ActiveIndex => _activeIndex;

    public void Initialize()
    {
        ActivateCamera(0);
    }

    public void SwitchCamera(int index)
    {
        ActivateCamera((index + virtualCameras.Length) % virtualCameras.Length);
    }

    private void ActivateCamera(int index)
    {
        Transform target = virtualCameras[index];
        _activeIndex = index;

        if (camera != null)
        {
            camera.transform.DOMove(target.position, 1);
            camera.transform.DORotate(target.rotation.eulerAngles, 1);
        }
    }
}