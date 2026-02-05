using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Fruit : DragableObject
{
    [SerializeField] private FruitType _type;

    public FruitType GetFruitType() => _type;

    protected void ChangeFruitType(FruitType typeToSet)
    {
        _type = typeToSet;
    }
}

public enum FruitType
{
    Apple = 0,
    Banana = 1,
    Orange = 2,
    Apricot = 3,
    Kiwi = 4
}
