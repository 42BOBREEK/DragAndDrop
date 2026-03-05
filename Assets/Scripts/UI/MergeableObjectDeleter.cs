using System.Collections;
using UnityEngine;
using System;

public class MergeableObjectDeleter : ActionButton
{
    [SerializeField] private InputReader _input;
    [SerializeField] private DragAndDrop _dragNDrop;
    [SerializeField] private bool _canDelete;
    [SerializeField] private float _canDragDelay;
    [SerializeField] private DeleterAnimation _animation;
        
    public event Action<MergeableObject> MergeableObjectDeleted;

    private void OnEnable()
    {
        _input.Pressed += OnPressed;
        _animation.AnimationEnded += DeleteMergeableObject;
        UpdateChargesText();
    }

    private void OnDisable()
    {
        _input.Pressed -= OnPressed;
        _animation.AnimationEnded -= DeleteMergeableObject;
    }

    private void OnPressed()
    {
        if(_canDelete == false || _chargesLeft <= 0)
            return;

        MergeableObject objectToDelete = _input.GetMergeableObjectUnderPointer();

        if(objectToDelete == null)
            return;

        DeleteObject(objectToDelete);

        _canDelete = false;
        
        StartCoroutine(SetCanDragWithDelay());
    }

    private void DeleteMergeableObject(MergeableObject obj)
    {
        Destroy(obj.gameObject);
        _chargesLeft--;
        InvokeChargesChanged();
        UpdateChargesText();
    }

    private void DeleteObject(MergeableObject obj)
    {
        MergeableObjectDeleted?.Invoke(obj);
    }

    private IEnumerator SetCanDragWithDelay()
    {
        yield return new WaitForSeconds(_canDragDelay);

        _dragNDrop.SetCanDrag(true);
    }

    public void ToggleCanDelete() 
    {
        if(_chargesLeft <= 0)
        {
            _canDelete = false;
            _dragNDrop.SetCanDrag(true);
            return;
        }

        _canDelete = !_canDelete;
        _dragNDrop.SetCanDrag(!_canDelete);
    }
}