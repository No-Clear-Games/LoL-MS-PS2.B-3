using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

[Serializable]
public class DragAndDropController
{


    [SerializeField] private float distanceFromCamera = 10;
    [SerializeField] private float physicsDragSpeed = 10;
    [SerializeField] private float mouseDragSpeed = 0.1f;
    
    enum State
    {
        NotDragging,
        Dragging
    }

    private bool _isDragging;
    private GameObject _draggedObject;
    private Camera _mainCamera;
    private WaitForFixedUpdate _waitForFixedUpdate;
    private Vector3 _velocity = Vector3.zero;
    private GameObject _currentHoveredSlot;
    private bool _hoveringOverSlot;
    private bool _gotCurrentSlot;

    public event Action<GameObject> OccupySlotAction;
    public event Action<GameObject> ReleaseSlotAction;
    public event Action<GameObject> StartDraggingAction;
    public event Action<GameObject> DropAction;
    
    // Start is called before the first frame update
    public void Initialize()
    {
        _isDragging = false;
        _mainCamera = Camera.main;
        _waitForFixedUpdate = new WaitForFixedUpdate();
    }

    public IEnumerator Drag(GameObject draggedObject)
    {
        _isDragging = true;
        _draggedObject = draggedObject;
        StartDraggingAction?.Invoke(draggedObject);
        
        draggedObject.transform.position = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue())
            .GetPoint(distanceFromCamera);
        while (_isDragging)
        {
            yield return _waitForFixedUpdate;
            
            
            Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (CheckForSlots(ray, out GameObject slot))
            {
                _hoveringOverSlot = true;
                if (slot != _currentHoveredSlot)
                {
                    _gotCurrentSlot = false;
                    MagnetSlot mSlot = slot.GetComponent<MagnetSlot>();
                    _currentHoveredSlot = slot;
                    if (!mSlot.Occupied)
                    {
                        mSlot.Occupy(draggedObject);
                        OccupySlotAction?.Invoke(draggedObject);
                        _gotCurrentSlot = true;
                    }
                }

                if (_gotCurrentSlot)
                {
                    continue;
                }
                
            }
            else if(_currentHoveredSlot != null && _gotCurrentSlot)
            {
                GameObject obj = _currentHoveredSlot.GetComponent<MagnetSlot>().Release();
                _currentHoveredSlot = null;
                _hoveringOverSlot = false;
                _gotCurrentSlot = false;
                ReleaseSlotAction?.Invoke(obj);
            }
            
            
            draggedObject.transform.position = Vector3.SmoothDamp(draggedObject.transform.position, ray.GetPoint(distanceFromCamera),
                ref _velocity, mouseDragSpeed);
        }

        DropAction?.Invoke(draggedObject);
        _draggedObject = null;
        _hoveringOverSlot = false;
        _currentHoveredSlot = null;
        _gotCurrentSlot = false;
    }

    private bool CheckForSlots(Ray ray, out GameObject slot)
    {
        slot = null;
        LayerMask layerMask = LayerMask.GetMask("Slots");
        bool found = Physics.Raycast(ray, out RaycastHit hit, 100,layerMask);
        if(found)
        {
            slot = hit.collider.gameObject;
        }

        return found;
    }
    


    public void Drop()
    {
        if (!_gotCurrentSlot)
        {
            return;
        }
        
        _isDragging = false;
    }

}
