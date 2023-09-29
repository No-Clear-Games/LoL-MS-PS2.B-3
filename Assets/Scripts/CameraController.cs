using System;
using System.Collections;
using System.Collections.Generic;
// using Cinemachine;
using UnityEngine;

[Serializable]
public class CameraController
{
    // [SerializeField] private CinemachineVirtualCamera[] virtualCameras;
    //
    // private int _activeIndex;
    //
    // public int ActiveIndex => _activeIndex;
    //
    public void Initialize()
    {
        // foreach (CinemachineVirtualCamera virtualCamera in virtualCameras)
        // {
        //     virtualCamera.enabled = false;
        // }
        //
        // ActivateCamera(0);
    }
    //
    public void SwitchCamera(int index)
    {
        // DeactivateCamera(_activeIndex);
    }
        // ActivateCamera((index + virtualCameras.Length) % virtualCameras.Length);
    //
    private void ActivateCamera(int index)
    {
    //     virtualCameras[index].enabled = true;
    //     _activeIndex = index;
    }
    //
    private void DeactivateCamera(int index)
    {
    //     virtualCameras[index].enabled = false;
    }
}
