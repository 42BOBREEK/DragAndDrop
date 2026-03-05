using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MergeableObject : DragableObject
{
    [SerializeField] private MergeableObjectLevel _type;

    public MergeableObjectLevel GetMergeableObjectLevel() => _type;

    protected void ChangeMergeableObjectLevel(MergeableObjectLevel typeToSet)
    {
        _type = typeToSet;
    }
}

public enum MergeableObjectLevel
{
    Level1 = 0,
    Level2 = 1,
    Level3 = 2,
    Level4 = 3,
    Level5 = 4,
    Level6 = 5,
    Level7 = 6,
    Level8 = 7,
    Level9 = 8,
    Level10 = 9
}
