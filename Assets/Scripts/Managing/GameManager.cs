using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ObjectsSpawner _spawner;
    [SerializeField] private DragAndDrop _dragNDrop;
    [SerializeField] private bool _isPlaying = true;
    [SerializeField] private ObjectsMerger _merger;
    [SerializeField] private ScoreCounter _scoreCounter;

    [SerializeField] private bool _isPaused;

    private void Start()
    {
        StartCoroutine(SpawnObjectIfNeeded());
    }

    private void OnEnable()
    {
        _spawner.ObjectSpawned += SubscribeToCollidedWithDragableObject;
        _merger.BountyMerged += ChangeScore;
    }

    private void OnDisable()
    {
        _spawner.ObjectSpawned -= SubscribeToCollidedWithDragableObject;
        _merger.BountyMerged -= ChangeScore;
    }

    private IEnumerator SpawnObjectIfNeeded()
    {
        while(_isPlaying)
        {
            if(_isPaused == true)
                yield return null;

            if(_dragNDrop.HasActiveObject == false)
            {
                DragableObject newObject = _spawner.SpawnRandomObject();
                newObject.OnStartDrag();
                newObject.ChangeCanCollideWithDragableObjects(true);

                _dragNDrop.SetActiveObject(newObject.gameObject.GetComponent<Rigidbody2D>());

                newObject.Collided += SetActiveObjectNull;
                SubscribeToCollidedWithDragableObject(newObject);
            }
            else
                yield return null;
        }
    }

    private void SetActiveObjectNull(DragableObject newObject) 
    {
        _dragNDrop.SetActiveObjectNull();

        if(_isPaused == false)
            _dragNDrop.SetCanDrag(true);

        newObject.Collided -= SetActiveObjectNull;
        newObject.OnEndDrag();
    }
    
    private void MergeObjects(DragableObject obj1, DragableObject obj2)
    {
        obj1.TryGetComponent<Fruit>(out Fruit fruit1);
        obj2.TryGetComponent<Fruit>(out Fruit fruit2);

        if(fruit1 != null && fruit2 != null)
            _merger.MergeObjects(fruit1, fruit2);
    }

    public void SubscribeToCollidedWithDragableObject(DragableObject obj)
    {
        obj.CollidedWithDragableObject += MergeObjects;
    }

    public void SubscribeToCollided(DragableObject obj)
    {
        obj.Collided += SetActiveObjectNull;
    }

    private void ChangeScore(int score)
    {
        _scoreCounter.ChangeScore(score);
    }

    public void SetIsPaused(bool isPaused)
    {
        _isPaused = isPaused;
    }
}
