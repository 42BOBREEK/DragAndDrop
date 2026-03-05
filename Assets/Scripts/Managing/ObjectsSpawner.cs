using System;
using UnityEngine;

public class ObjectsSpawner : MonoBehaviour
{
    [SerializeField] private MergeableObject[] _objectsToSpawn;
    [SerializeField] private Transform _positionToSpawnAt;
    [SerializeField] private DragAndDrop _dragNDrop;
    [SerializeField] private int _firstObjRandomChance;
    [SerializeField] private int _secondObjRandomChance;
    [SerializeField] private int _thirdObjRandomChance;
    [SerializeField] private int _fourthObjRandomChance;
    [SerializeField] private TreasureChest _chestToSpawn;

    public event Action<DragableObject> MergedObjectSpawned;

    public DragableObject SpawnRandomObject()
    {
        DragableObject newObject = Instantiate(GetRandomObjectToSpawn(), _positionToSpawnAt.position, Quaternion.identity);

        return newObject;
    }

    public MergeableObject SpawnMergeableObject(MergeableObject objectToSpawn) //для copypast object'a
    {
        MergeableObject obj = Instantiate(objectToSpawn, _positionToSpawnAt.position, Quaternion.identity);

        return obj;
    }

    public MergeableObject SpawnMergedMergeableObject(Vector2 posToSpawnAt, MergeableObjectLevel objectType)
    {
        MergeableObject objectToSpawn = null;

        foreach(var obj in _objectsToSpawn)
        {
            if(obj.GetMergeableObjectLevel() == objectType)
                objectToSpawn = obj;
        }

        if(objectToSpawn == null)
            return null;

        MergeableObject newObject = Instantiate(objectToSpawn, posToSpawnAt, Quaternion.identity);
        newObject.InitializeCollidableObject(); 
        MergedObjectSpawned?.Invoke(newObject);
        newObject.RotatingObject.StopRotation();

        return newObject;
    }

    public TreasureChest SpawnChest(Vector2 posToSpawnAt)
    {
        return Instantiate(_chestToSpawn, posToSpawnAt, Quaternion.identity);
    }

    private DragableObject GetRandomObjectToSpawn()
    {
        int randomIndex = UnityEngine.Random.Range(0, 101);
        int objectIndex;

        if(randomIndex <= _firstObjRandomChance) //0-60
            objectIndex = 0;
        else if(randomIndex > _firstObjRandomChance && randomIndex <= _secondObjRandomChance) //61 - 80
            objectIndex = 1;
        else if(randomIndex > _secondObjRandomChance && randomIndex <= _thirdObjRandomChance) //81 - 90
            objectIndex = 2;
        else if(randomIndex > _thirdObjRandomChance && randomIndex <= _fourthObjRandomChance) //91 - 96
            objectIndex = 3;
        else 
            objectIndex = 4; // 97 - 100


        return _objectsToSpawn[objectIndex];
    }
}
