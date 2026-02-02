using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Attractor))]
public class DragableObject : MonoBehaviour, IDragable
{
    [SerializeField] private bool _isMerged;
    [SerializeField] private FruitType _type;
    [SerializeField] private GravityField _gravityField;
    [SerializeField] private string _wallsTag;

    private Rigidbody2D _rigidbody;
    private Attractor _attractor;

    public event Action<DragableObject> Collided;
    public event Action<DragableObject, DragableObject> CollidedWithDragableObject;

    public FruitType Type => _type;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        _attractor = GetComponent<Attractor>();
    }

    private void OnEnable()
    {
        _gravityField.Triggered += AttractToObject;
    }
     
    private void OnDisable()
    {
        _gravityField.Triggered -= AttractToObject;
    }
     
    private void AttractToObject(DragableObject objectAttractTo)
    {
        if(GetFruitType() != objectAttractTo.GetFruitType())
            return;

        _attractor.AttractToObject(objectAttractTo.transform);
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.tag == _wallsTag)
            return;

        Collided?.Invoke(this);

        if(_isMerged)
            return;

        if(coll.gameObject.TryGetComponent<DragableObject>(out DragableObject obj) == true)
            CollidedWithDragableObject?.Invoke(this, obj);
    }

    public void ChangeIsMerged(bool isMerged) => _isMerged = isMerged;

    public FruitType GetFruitType() => _type;

    public void OnStartDrag()
    {
        _rigidbody.gravityScale = 0f;
    }

    public void OnEndDrag()
    {
        _rigidbody.gravityScale = 1f;
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
