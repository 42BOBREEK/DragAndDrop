using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DeadLine : MonoBehaviour
{
    public event Action CollidedWithFruit;

    private void OnTriggerEnter2D(Collider2D coll)
    {
        coll.gameObject.TryGetComponent<Fruit>(out Fruit fruit);

        if(fruit != null && fruit.CanCollideWithDeadLine == true)
            CollidedWithFruit?.Invoke();
    }
}
