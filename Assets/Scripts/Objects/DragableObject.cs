using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Attractor))]
[RequireComponent(typeof(RotatingObject))]
public class DragableObject : MonoBehaviour
{
    [SerializeField] private GravityField _gravityField; //TODO: suck my dick
    [SerializeField] private string _wallsTag;

    private Rigidbody2D _rigidbody;
    private Attractor _attractor;
    private bool _canCollide;
    private bool _isMergable;

    protected bool _canCollideWithDragableObjects;

    public event Action<DragableObject> Collided;
    public event Action<DragableObject, DragableObject> CollidedWithDragableObject;

    public bool CanCollideWithDeadLine { get; private set; }

    public bool IsMergable => _isMergable;
    public RotatingObject RotatingObject;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        _attractor = GetComponent<Attractor>();
        RotatingObject = GetComponent<RotatingObject>();
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
        _attractor.AttractToObject(objectAttractTo);
    }

    public virtual void OnCollisionEnter2D(Collision2D coll)
    {
        if(_canCollide == false)
        {
            return;
        }

        if(coll.gameObject.tag == _wallsTag)
            return;

        InvokeCollided();
        CanCollideWithDeadLine = true;

        if(_canCollideWithDragableObjects == false)
        {
            return;
        }

        if(coll.gameObject.TryGetComponent<Fruit>(out Fruit obj) == true)
        {
            CanCollideWithDeadLine = true;

            if(_isMergable == true)
                InvokeCollidedWithDragableObject(obj);
        }
    }

    public void ChangeCanCollideWithDragableObjects(bool canCollide) => 
        _canCollideWithDragableObjects = canCollide;

    public void OnStartDrag()
    {
        _rigidbody.gravityScale = 0f;
        _rigidbody.bodyType = RigidbodyType2D.Kinematic;
        _canCollide = false;
    }

    public void OnEndDrag()
    {
        _rigidbody.bodyType = RigidbodyType2D.Dynamic;
        _canCollide = true;
        ChangeCanCollideWithDragableObjects(true);
        SetIsMergable(true);
    }

    public void InitializeCollidableObject()
    {
        _canCollide = true;
        _canCollideWithDragableObjects = true;
        _isMergable = true;
    }

    public void SetIsMergable(bool isMergable) => _isMergable = isMergable;

    protected void InvokeCollided()
    {
        Collided?.Invoke(this);
    }

    protected void InvokeCollidedWithDragableObject(DragableObject obj)
    {
        CollidedWithDragableObject?.Invoke(this, obj);
    }

    protected bool CheckIfCollIsWall(Collision2D coll)
    {
        return coll.gameObject.tag == _wallsTag;
    }
}
