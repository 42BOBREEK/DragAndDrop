using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class GravityField : MonoBehaviour
{
    public event Action<DragableObject> Triggered;

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.TryGetComponent<DragableObject>(out DragableObject obj) == true)
            Triggered?.Invoke(obj);
    }
}
