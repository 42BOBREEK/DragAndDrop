using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    [SerializeField] private int _minCopyObjectsCharges;
    [SerializeField] private int _maxCopyObjectsCharges;
    [SerializeField] private int _minDeletions;
    [SerializeField] private int _maxDeletions;
    [SerializeField] private Transform _posToSpawnCopyObjects;
    [SerializeField] private Transform _posToSpawnDeletions;

    private int _copyObjectsCharges;
    private int _deletionsCharges;

    private void Start()
    {
        _copyObjectsCharges = UnityEngine.Random.Range(_minCopyObjectsCharges, _maxCopyObjectsCharges + 1);
        _deletionsCharges = UnityEngine.Random.Range(_minDeletions, _maxDeletions + 1);
    }

    public void Open(out int copyObjectsCount,
            out int deletionsCount,
            out Transform posToSpawnCopyObjects,
            out Transform posToSpawnDeletions)
    {
        copyObjectsCount = _copyObjectsCharges;
        deletionsCount = _deletionsCharges;
        posToSpawnCopyObjects = _posToSpawnCopyObjects;
        posToSpawnDeletions = _posToSpawnDeletions;

        Destroy(gameObject);
    }
}
