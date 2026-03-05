using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DeadLine : MonoBehaviour
{
    public event Action CollidedWithMergeableObject;

    private void OnTriggerEnter2D(Collider2D coll)
    {
        coll.gameObject.TryGetComponent<MergeableObject>(out MergeableObject obj);

        if(obj != null && obj.CanCollideWithDeadLine == true)
            CollidedWithMergeableObject?.Invoke();
    }
}
