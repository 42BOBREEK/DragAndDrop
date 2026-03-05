using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MergeableObject))]
public class Attractor : MonoBehaviour
{
    [SerializeField] private float _attractionForce;

    private Rigidbody2D _rigidbody;
    private MergeableObject _mergeableObject;

    private WaitForFixedUpdate _wait = new WaitForFixedUpdate();

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _mergeableObject = GetComponent<MergeableObject>();
    }

    public void AttractToObject(DragableObject objectAttractTo)
    {
        if(objectAttractTo.TryGetComponent<MergeableObject>(out MergeableObject mergeableObjectAttractTo))
            if(IsCommonType(mergeableObjectAttractTo) == false)
                return;

        StartCoroutine(AttractToObjectSlowly(mergeableObjectAttractTo.transform));
    }

    private IEnumerator AttractToObjectSlowly(Transform objectAttractTo)
    {
        Vector2 direction = (objectAttractTo.position - transform.position).normalized;

        _rigidbody.AddForce(direction * _attractionForce, ForceMode2D.Force);

        yield return _wait;
    }

    private bool IsCommonType(MergeableObject mergeableObjectToCheck)
    {
        if(mergeableObjectToCheck.GetMergeableObjectLevel() == _mergeableObject.GetMergeableObjectLevel())
            return true;
        else
            return false;
    }
}
