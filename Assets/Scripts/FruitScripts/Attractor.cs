using System.Collections;
using UnityEngine;

public class Attractor : MonoBehaviour
{
    [SerializeField] private float _attractionForce;

    private Rigidbody2D _rigidbody;

    private WaitForFixedUpdate _wait = new WaitForFixedUpdate();

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void AttractToObject(Transform obj)
    {
        StartCoroutine(AttractToObjectSlowly(obj));
    }

    private IEnumerator AttractToObjectSlowly(Transform objectAttractTo)
    {
        Vector2 direction = (objectAttractTo.position - transform.position).normalized;

        _rigidbody.AddForce(direction * _attractionForce, ForceMode2D.Force);

        yield return _wait;
    }
}
