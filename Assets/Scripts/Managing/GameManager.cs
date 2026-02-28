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
    [SerializeField] private CopyPaster _copyPaster;
    [SerializeField] private FruitsDeleter _deleter;
    [SerializeField] private MusicManager _musicManager;
    [SerializeField] private ProgressSaver _progressSaver;

    private void Awake()
    {
        ValidateFields();
    }

    private void ValidateFields()
    {
        if (_spawner == null) Debug.Log($"{nameof(GameManager)}: _spawner is null", this);
        if (_dragNDrop == null) Debug.Log($"{nameof(GameManager)}: _dragNDrop is null", this);
        if (_merger == null) Debug.Log($"{nameof(GameManager)}: _merger is null", this);
        if (_scoreCounter == null) Debug.Log($"{nameof(GameManager)}: _scoreCounter is null", this);
        if (_deadLine == null) Debug.Log($"{nameof(GameManager)}: _deadLine is null", this);
        if (_gameOverPanel == null) Debug.Log($"{nameof(GameManager)}: _gameOverPanel is null", this);
        if (_audio == null) Debug.Log($"{nameof(GameManager)}: _audio is null", this);
        if (_copyPaster == null) Debug.Log($"{nameof(GameManager)}: _copyPaster is null", this);
        if (_deleter == null) Debug.Log($"{nameof(GameManager)}: _deleter is null", this);
        if (_musicManager == null) Debug.Log($"{nameof(GameManager)}: _musicManager is null", this);
        if (_progressSaver == null) Debug.Log($"{nameof(GameManager)}: _progressSaver is null", this);
    }

    private void Start()
    {
        Time.timeScale = 1f;
        StartCoroutine(SpawnObjectIfNeeded());
    }

    private void OnEnable()
    {
        if (_spawner != null)
        {
            _spawner.MergedObjectSpawned += SubscribeToCollidedWithDragableObject;
            _spawner.MergedObjectSpawned += PlaySound;
        }
        if (_merger != null)
        {
            _merger.BountyMerged += ChangeScore;
            _merger.WatermelonMerged += OnWatermelonMerged;
        }
        if (_deadLine != null)
            _deadLine.CollidedWithFruit += SetGameOver;
        if (_copyPaster != null)
            _copyPaster.ChargesChanged += SaveCopyPasterCharges;
        if (_deleter != null)
            _deleter.ChargesChanged += SaveDeleterCharges;
        if (_musicManager != null)
            _musicManager.MusicToggled += SaveMusicState;
    }

    private void OnDisable()
    {
        if (_spawner != null)
        {
            _spawner.MergedObjectSpawned -= SubscribeToCollidedWithDragableObject;
            _spawner.MergedObjectSpawned -= PlaySound;
        }
        if (_merger != null)
        {
            _merger.BountyMerged -= ChangeScore;
            _merger.WatermelonMerged -= OnWatermelonMerged;
        }
        if (_deadLine != null)
            _deadLine.CollidedWithFruit -= SetGameOver;
        if (_copyPaster != null)
            _copyPaster.ChargesChanged -= SaveCopyPasterCharges;
        if (_deleter != null)
            _deleter.ChargesChanged -= SaveDeleterCharges;
        if (_musicManager != null)
            _musicManager.MusicToggled -= SaveMusicState;
    }

    private IEnumerator SpawnObjectIfNeeded()
    {
        while (_isPlaying)
        {
            if (_isPaused)
            {
                yield return null;
                continue;
            }

            if (_dragNDrop != null && !_dragNDrop.HasActiveObject)
            {
                var newObject = _spawner?.SpawnRandomObject();
                if (newObject != null)
                {
                    newObject.OnStartDrag();
                    newObject.ChangeCanCollideWithDragableObjects(true);

                    _dragNDrop.SetActiveObject(newObject.gameObject.GetComponent<Rigidbody2D>());

                    newObject.Collided += SetActiveObjectNull;
                    SubscribeToCollidedWithDragableObject(newObject);
                }
            }
            else
            {
                yield return null;
            }
        }
    }

    private void SetActiveObjectNull(DragableObject newObject)
    {
        if (_dragNDrop != null)
        {
            _dragNDrop.SetActiveObjectNull();
            if (!_isPaused)
                _dragNDrop.SetCanDrag(true);
        }

        newObject.Collided -= SetActiveObjectNull;
        newObject.OnEndDrag();
    }
    
    private void MergeObjects(DragableObject obj1, DragableObject obj2)
    {
        obj1.TryGetComponent<Fruit>(out Fruit fruit1);
        obj2.TryGetComponent<Fruit>(out Fruit fruit2);

        if (fruit1 != null && fruit2 != null && _merger != null)
            _merger.MergeObjects(fruit1, fruit2);
    }

    private void SetGameOver()
    {
        SetIsPaused(true);
        if (_dragNDrop != null)
            _dragNDrop.SetCanDrag(false);
        if (_gameOverPanel != null)
            _gameOverPanel.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    private void OnWatermelonMerged(Vector2 watermelonPos)
    {
        if (_spawner != null)
            _spawner.SpawnChest(watermelonPos);
        if (_scoreCounter != null)
            _scoreCounter.AddWatermelonScore();
        if (_progressSaver != null && _scoreCounter != null)
            _progressSaver.SaveWatermelonScore(_scoreCounter.WatermelonScore);
    }

    private void PlaySound(DragableObject obj)
    {
        if (_audio != null)
            _audio.Play();
    }

    private void ChangeScore(int score)
    {
        if (_scoreCounter != null)
            _scoreCounter.AddScore(score);
        if (_progressSaver != null && _scoreCounter != null)
            _progressSaver.SaveScore(_scoreCounter.Score);
    }

    private void SaveCopyPasterCharges()
    {
        if (_progressSaver != null && _copyPaster != null)
            _progressSaver.SaveCopyPasterCharges(_copyPaster.ChargesLeft);
    }

    private void SaveDeleterCharges()
    {
        if (_progressSaver != null && _deleter != null)
            _progressSaver.SaveDeleterCharges(_deleter.ChargesLeft);
    }

    private void SaveMusicState()
    {
        if (_progressSaver != null && _musicManager != null)
            _progressSaver.SaveMusicState(_musicManager.IsOn);
        PlayerPrefs.Save();
    }

    public void SetIsPaused(bool isPaused)
    {
        _isPaused = isPaused;
    }

    private void SubscribeToCollidedWithDragableObject(DragableObject obj)
    {
        if (obj != null)
            obj.CollidedWithDragableObject += MergeObjects;
    }

    public void SubscribeToCollided(DragableObject obj)
    {
        if (obj != null)
            obj.Collided += SetActiveObjectNull;
    }
}
