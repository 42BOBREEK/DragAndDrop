using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ObjectsSpawner _spawner;
    [SerializeField] private DragAndDrop _dragNDrop;
    [SerializeField] private bool _isPlaying = true;
    [SerializeField] private ObjectsMerger _merger;

    private void Start()
    {
        StartCoroutine(SpawnObjectIfNeeded());
    }

    private void OnEnable()
    {
        _spawner.ObjectSpawned += SubscribeToObject;
    }

    private void OnDisable()
    {
        _spawner.ObjectSpawned -= SubscribeToObject;
    }

    private IEnumerator SpawnObjectIfNeeded()
    {
        while(_isPlaying)
        {
            if(_dragNDrop.HasActiveObject == false)
            {
                DragableObject newObject = _spawner.Spawn();
                newObject.OnStartDrag();

                _dragNDrop.SetActiveObject(newObject.gameObject.GetComponent<Rigidbody2D>());

                newObject.Collided += SetActiveObjectNull;
                SubscribeToObject(newObject);
            }
            else
                yield return null;
        }
    }

    private void SetActiveObjectNull(DragableObject newObject) 
    {
        _dragNDrop.SetActiveObjectNull();
        _dragNDrop.SetCanDrag(true);

        newObject.Collided -= SetActiveObjectNull;
        newObject.OnEndDrag();
    }
    
    private void MergeObjects(DragableObject obj1, DragableObject obj2)
    {
        _merger.MergeObjects(obj1, obj2);
    }

    private void SubscribeToObject(DragableObject obj)
    {
        obj.CollidedWithDragableObject += MergeObjects;
    }
}
