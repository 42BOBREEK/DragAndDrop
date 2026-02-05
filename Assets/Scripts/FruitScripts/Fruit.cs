using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Fruit : DragableObject
{
    [SerializeField] private FruitType _type;

    public virtual FruitType GetFruitType() => _type;
}

public enum FruitType
{
    Apple = 0,
    Banana = 1,
    Orange = 2,
    Apricot = 3,
    Kiwi = 4
}
