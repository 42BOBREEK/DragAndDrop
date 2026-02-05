using System;
using UnityEngine;

public class ObjectsSpawner : MonoBehaviour
{
    [SerializeField] private Fruit[] _fruitsToSpawn;
    [SerializeField] private Transform _positionToSpawnAt;
    [SerializeField] private DragAndDrop _dragNDrop;
    [SerializeField] private int _firstObjRandomChance;
    [SerializeField] private int _secondObjRandomChance;
    [SerializeField] private int _thirdObjRandomChance;
    [SerializeField] private int _fourthObjRandomChance;

    public event Action<DragableObject> ObjectSpawned;

    public DragableObject SpawnRandomObject()
    {
        DragableObject newObject = Instantiate(GetRandomObjectToSpawn(), _positionToSpawnAt.position, Quaternion.identity);

        return newObject;
    }

    public Fruit SpawnFruit(Fruit fruitToSpawn)
    {
        Fruit fruit = Instantiate(fruitToSpawn, _positionToSpawnAt.position, Quaternion.identity);

        return fruit;
    }

    public Fruit SpawnMergedFruit(Vector2 posToSpawnAt, FruitType fruitType)
    {
        Fruit fruitToSpawn = null;

        foreach(var obj in _fruitsToSpawn)
        {
            if(obj.GetFruitType() == fruitType)
                fruitToSpawn = obj;
        }

        if(fruitToSpawn == null)
            return null;

        Fruit newObject = Instantiate(fruitToSpawn, posToSpawnAt, Quaternion.identity);
        ObjectSpawned?.Invoke(newObject);

        return newObject;
    }

    private DragableObject GetRandomObjectToSpawn()
    {
        int randomIndex = UnityEngine.Random.Range(0, 101);
        int objectIndex = 0;

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


        return _fruitsToSpawn[objectIndex];
    }
}
