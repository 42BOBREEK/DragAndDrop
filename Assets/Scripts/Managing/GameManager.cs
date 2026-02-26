using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ObjectsSpawner _spawner;
    [SerializeField] private DragAndDrop _dragNDrop;
    [SerializeField] private bool _isPlaying = true;
    [SerializeField] private ObjectsMerger _merger;
    [SerializeField] private ScoreCounter _scoreCounter;
    [SerializeField] private DeadLine _deadLine;
    [SerializeField] private RectTransform _gameOverPanel;
    [SerializeField] private bool _isPaused;
    [SerializeField] private AudioSource _audio;

    private void Start()
    {
        Time.timeScale = 1f;
        StartCoroutine(SpawnObjectIfNeeded());
    }

    private void OnEnable()
    {
        _spawner.MergedObjectSpawned += SubscribeToCollidedWithDragableObject;
        _spawner.MergedObjectSpawned += PlaySound;
        _merger.BountyMerged += ChangeScore;
        _merger.WatermelonMerged += OnWatermelonMerged;
        _deadLine.CollidedWithFruit += SetGameOver;
    }

    private void OnDisable()
    {
        _spawner.MergedObjectSpawned -= SubscribeToCollidedWithDragableObject;
        _spawner.MergedObjectSpawned -= PlaySound;
        _merger.BountyMerged -= ChangeScore;
        _merger.WatermelonMerged -= OnWatermelonMerged;
        _deadLine.CollidedWithFruit -= SetGameOver;
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

    private void SetGameOver()
    {
        SetIsPaused(true);
        _dragNDrop.SetCanDrag(false);
        _gameOverPanel.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    private void OnWatermelonMerged(Vector2 watermelonPos)
    {
        _spawner.SpawnChest(watermelonPos);
        _scoreCounter.AddWatermelonScore();
    }

    private void PlaySound(DragableObject obj)
    {
        _audio.Play();
    }

    private void ChangeScore(int score)
    {
        _scoreCounter.AddScore(score);
    }

    public void SetIsPaused(bool isPaused)
    {
        _isPaused = isPaused;
    }

    private void SubscribeToCollidedWithDragableObject(DragableObject obj)
    {
        obj.CollidedWithDragableObject += MergeObjects;
    }

    public void SubscribeToCollided(DragableObject obj)
    {
        obj.Collided += SetActiveObjectNull;
    }
}
