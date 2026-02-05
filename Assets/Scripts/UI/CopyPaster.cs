using UnityEngine;

public class CopyPaster : MonoBehaviour
{
    [SerializeField] private Fruit _copyPastObject;
    [SerializeField] private ObjectsSpawner _spawner;
    [SerializeField] private DragAndDrop _dragNDrop;
    [SerializeField] private GameManager _manager;

    public void SpawnCopyPastObject()
    {
        Fruit copyPastObject = _spawner.SpawnFruit(_copyPastObject);
        copyPastObject.OnStartDrag();

        Rigidbody2D copyPastRigidbody = copyPastObject.GetComponent<Rigidbody2D>();

        _manager.SubscribeToCollided(copyPastObject);

        _dragNDrop.DeleteActiveObject();
        _dragNDrop.SetActiveObject(copyPastRigidbody);
    }
}
