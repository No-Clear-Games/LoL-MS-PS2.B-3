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
    
    public enum State
    {
        NotDragging,
        Dragging,
        Canceling,
        Dropping
    }

    private State _state;
    private GameObject _draggedObject;
    private Camera _mainCamera;
    private WaitForFixedUpdate _waitForFixedUpdate;
    private Vector3 _velocity = Vector3.zero;
    private GameObject _currentHoveredSlot;
    private bool _hoveringOverSlot;
    private bool _gotCurrentSlot;
    private int _itemDefaultLayer;

    public State DraggingState => _state;

    public event Action<GameObject> OccupySlotAction;
    public event Action<GameObject> ReleaseSlotAction;
    public event Action<GameObject> StartDraggingAction;
    public event Action<GameObject> DropAction;
    public event Action<GameObject> CancelAction; 
    
    // Start is called before the first frame update
    public void Initialize()
    {
        _state = State.NotDragging;
        _mainCamera = Camera.main;
        _waitForFixedUpdate = new WaitForFixedUpdate();
    }

    public IEnumerator Drag(GameObject draggedObject)
    {
        _state = State.Dragging;
        _draggedObject = draggedObject;
        StartDraggingAction?.Invoke(draggedObject);
        _itemDefaultLayer = draggedObject.layer;
        draggedObject.layer = LayerMask.NameToLayer("Highlighted");
        
        draggedObject.transform.position = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue())
            .GetPoint(distanceFromCamera);
        while (_state == State.Dragging)
        {
            yield return _waitForFixedUpdate;
            
            
            Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (CheckForSlots(ray, out GameObject slot))
            {
                _hoveringOverSlot = true;
                if (slot != _currentHoveredSlot)
                {
                    if (_gotCurrentSlot)
                    {
                        ReleaseSlot(_currentHoveredSlot.GetComponent<MagnetSlot>());
                    }
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
            else 
            {
                if (_currentHoveredSlot != null && _gotCurrentSlot)
                {
                    ReleaseSlot(_currentHoveredSlot.GetComponent<MagnetSlot>());
                }
                _currentHoveredSlot = null;
                _hoveringOverSlot = false;
                _gotCurrentSlot = false;
            }


            if (draggedObject != null)
            {
                draggedObject.transform.position = Vector3.SmoothDamp(draggedObject.transform.position,
                    ray.GetPoint(distanceFromCamera),
                    ref _velocity, mouseDragSpeed);
            }
        }

        _draggedObject.layer = _itemDefaultLayer;
        _hoveringOverSlot = false;
        _gotCurrentSlot = false;

        if (_state == State.Canceling)
        {
            if (_gotCurrentSlot && _currentHoveredSlot != null)
            {
                ReleaseSlot(_currentHoveredSlot.GetComponent<MagnetSlot>());
            }
            
            CancelAction?.Invoke(_draggedObject);
        }

        if (_state == State.Dropping)
        {
            DropAction?.Invoke(_draggedObject);
        }
        
        _currentHoveredSlot = null;
        _draggedObject = null;
        
    }

    private void ReleaseSlot(MagnetSlot magnetSlot)
    {
        GameObject obj = magnetSlot.Release();
        ReleaseSlotAction?.Invoke(obj);
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


    public void CancelDrag()
    {
        if (_state != State.Dragging)
        {
            return;
        }

        _state = State.Canceling;
    }

    public void TryDrop()
    {
        if (!_gotCurrentSlot || _state != State.Dragging)
        {
            return;
        }
        _state = State.Dropping;
    }

}
