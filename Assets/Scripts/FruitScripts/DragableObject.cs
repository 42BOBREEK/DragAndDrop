using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Attractor))]
public class DragableObject : MonoBehaviour, IDragable
{
    [SerializeField] private GravityField _gravityField;
    [SerializeField] protected string _wallsTag;

    private Rigidbody2D _rigidbody;
    private Attractor _attractor;
    protected bool _canCollideWithDragableObjects;

    public event Action<DragableObject> Collided;
    public event Action<DragableObject, DragableObject> CollidedWithDragableObject;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        _attractor = GetComponent<Attractor>();
    }

    private void OnEnable()
    {
        _gravityField.Triggered += AttractToObject;
        ChangeCanCollideWithDragableObjects(true);
    }
     
    private void OnDisable()
    {
        _gravityField.Triggered -= AttractToObject;
        ChangeCanCollideWithDragableObjects(false);
    }
     
    private void AttractToObject(DragableObject objectAttractTo)
    {
        _attractor.AttractToObject(objectAttractTo);
    }

    public virtual void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.tag == _wallsTag)
            return;

        InvokeCollided();

        if(_canCollideWithDragableObjects == false)
            return;

        if(coll.gameObject.TryGetComponent<Fruit>(out Fruit obj) == true)
        {
            InvokeCollidedWithDragableObject(obj);
        }
    }

    public void ChangeCanCollideWithDragableObjects(bool canCollide) => 
        _canCollideWithDragableObjects = canCollide;

    public void OnStartDrag()
    {
        _rigidbody.gravityScale = 0f;
    }

    public void OnEndDrag()
    {
        _rigidbody.gravityScale = 1f;
    }

    protected void InvokeCollided()
    {
        print("collided");
        Collided?.Invoke(this);
    }

    protected void InvokeCollidedWithDragableObject(DragableObject obj)
    {
        CollidedWithDragableObject?.Invoke(this, obj);
    }
}
