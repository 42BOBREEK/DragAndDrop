using System;
using UnityEngine;

public class ObjectsMerger : MonoBehaviour
{
    [SerializeField] private ObjectsSpawner _spawner;

    public event Action<int> BountyMerged;
    public event Action<Vector2, MergeableObjectLevel> MergeableObjectsMerged;
    public event Action<Vector2> LastLevelMerged;

    public void MergeObjects(MergeableObject obj1, MergeableObject obj2)
    {
        if(obj1.GetMergeableObjectLevel() != obj2.GetMergeableObjectLevel())
            return;

        if(obj1.IsMergable == false || obj2.IsMergable == false)
            return;

        MergeableObjectLevel commonType = obj1.GetMergeableObjectLevel();

        if(IsLastLevel(commonType))
            LastLevelMerged?.Invoke((obj1.transform.position + obj2.transform.position) / 2f);

        obj1.ChangeCanCollideWithDragableObjects(false);
        obj2.ChangeCanCollideWithDragableObjects(false);

        if(obj1.TryGetComponent<BountyMergeableObject>(out BountyMergeableObject bounty))
            BountyMerged?.Invoke(bounty.GetBounty());

        Vector2 middlePoint = 
            (obj1.transform.position + obj2.transform.position) / 2f;

        if(TryGetNextObject(commonType, out MergeableObjectLevel nextType))
        {
            MergeableObject spawnedObject = _spawner.SpawnMergedMergeableObject(middlePoint, nextType);
            MergeableObjectsMerged?.Invoke(spawnedObject.transform.position, nextType);
        }

        Destroy(obj1.gameObject);
        Destroy(obj2.gameObject);
    }

    private bool IsLastLevel(MergeableObjectLevel level)
    {
        int enumCount = Enum.GetValues(typeof(MergeableObjectLevel)).Length;
        return (int)level == enumCount - 1;
    }

    private bool TryGetNextObject(MergeableObjectLevel current, out MergeableObjectLevel next)
    {
        int nextValue = (int)current + 1;

        if(Enum.IsDefined(typeof(MergeableObjectLevel), nextValue))
        {
            next = (MergeableObjectLevel)nextValue;
            return true;
        }

        next = current;
        return false;
    }
}
