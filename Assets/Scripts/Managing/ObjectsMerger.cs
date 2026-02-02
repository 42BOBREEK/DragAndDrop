using System;
using UnityEngine;

public class ObjectsMerger : MonoBehaviour
{
    [SerializeField] private ObjectsSpawner _spawner;

    public void MergeObjects(DragableObject obj1,DragableObject obj2)
    {
        if(obj1.GetFruitType() != obj2.GetFruitType())
            return;

        FruitType commonFruitType = obj1.GetFruitType();

        obj1.ChangeIsMerged(true);
        obj2.ChangeIsMerged(true);

        Vector2 middlePoint = 
            (obj1.transform.position + obj2.transform.position) / 2f;

        if(TryGetNextFruit(commonFruitType, out FruitType nextFruitType))
            _spawner.SpawnMergedFruit(middlePoint, nextFruitType);

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
