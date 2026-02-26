using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Fruit))]
public class Attractor : MonoBehaviour
{
    [SerializeField] private float _attractionForce;

    private Rigidbody2D _rigidbody;
    private Fruit _fruit;

    private WaitForFixedUpdate _wait = new WaitForFixedUpdate();

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _fruit = GetComponent<Fruit>();
    }

    public void AttractToObject(DragableObject objectAttractTo)
    {
        if(objectAttractTo.TryGetComponent<Fruit>(out Fruit fruitAttractTo))
            if(IsCommonType(fruitAttractTo) == false)
                return;

        StartCoroutine(AttractToObjectSlowly(fruitAttractTo.transform));
    }

    private IEnumerator AttractToObjectSlowly(Transform objectAttractTo)
    {
        Vector2 direction = (objectAttractTo.position - transform.position).normalized;

        _rigidbody.AddForce(direction * _attractionForce, ForceMode2D.Force);

        yield return _wait;
    }

    private bool IsCommonType(Fruit fruitToCheck)
    {
        if(fruitToCheck.GetFruitType() == _fruit.GetFruitType())
            return true;
        else
            return false;
    }
}
