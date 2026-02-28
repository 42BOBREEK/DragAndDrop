using UnityEngine;
using TMPro;
using System;

public class CopyPaster : ActionButton
{
    [SerializeField] private Fruit _copyPastObject;
    [SerializeField] private ObjectsSpawner _spawner;
    [SerializeField] private DragAndDrop _dragNDrop;
    [SerializeField] private GameManager _manager;

    private void Start()
    {
        UpdateChargesText();
    }

    public void SpawnCopyPastObject()
    {
        if(_dragNDrop.CanDrag == false || _chargesLeft <= 0)
            return;

        Fruit copyPastObject = _spawner.SpawnFruit(_copyPastObject);
        copyPastObject.OnStartDrag();

        Rigidbody2D copyPastRigidbody = copyPastObject.GetComponent<Rigidbody2D>();

        _manager.SubscribeToCollided(copyPastObject);

        _dragNDrop.DeleteActiveObject();
        _dragNDrop.SetActiveObject(copyPastRigidbody);
        _chargesLeft--;
        InvokeChargesChanged();
        UpdateChargesText();
    }
}
