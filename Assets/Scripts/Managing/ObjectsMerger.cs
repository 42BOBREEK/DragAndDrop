using System;
using UnityEngine;

public class ObjectsMerger : MonoBehaviour
{
    [SerializeField] private ObjectsSpawner _spawner;

    public event Action<int> BountyMerged;
    public event Action<Vector2, FruitType> FruitsMerged;
    public event Action<Vector2> WatermelonMerged;

    public void MergeObjects(Fruit obj1,Fruit obj2)
    {
        if(obj1.GetFruitType() != obj2.GetFruitType())
            return;

        if(obj1.IsMergable == false || obj2.IsMergable == false)
            return;

        FruitType commonFruitType = obj1.GetFruitType();

        if(commonFruitType == FruitType.Watermelon)
            WatermelonMerged?.Invoke((obj1.transform.position + obj2.transform.position) / 2f);

        obj1.ChangeCanCollideWithDragableObjects(false);
        obj2.ChangeCanCollideWithDragableObjects(false);

        if(obj1.TryGetComponent<BountyFruit>(out BountyFruit bounty))
            BountyMerged?.Invoke(bounty.GetBounty());

        Vector2 middlePoint = 
            (obj1.transform.position + obj2.transform.position) / 2f;

        if(TryGetNextFruit(commonFruitType, out FruitType nextFruitType))
        {
            Fruit spawnedFruit = _spawner.SpawnMergedFruit(middlePoint, nextFruitType);
            FruitsMerged?.Invoke(spawnedFruit.transform.position, nextFruitType);
        }

        Destroy(obj1.gameObject);
        Destroy(obj2.gameObject);
    }

    private bool TryGetNextFruit(FruitType current, out FruitType next)
    {
        int nextValue = (int)current + 1;

        if(Enum.IsDefined(typeof(FruitType), nextValue))
        {
            next = (FruitType)nextValue;
            return true;
        }

        next = current;
        return false;
    }
}
