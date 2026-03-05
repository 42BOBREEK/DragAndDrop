using UnityEngine;
using System;
using System.Collections;
using DG.Tweening;

public class DeleterAnimation : MonoBehaviour
{
    [SerializeField] private MergeableObjectDeleter _deleter;
    [SerializeField] private RectTransform _deleterTransform;
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _deleterCollider;
    [SerializeField] private float _animationDuration;

    public event Action<MergeableObject> AnimationEnded;

    private void OnEnable()
    {
        _deleter.MergeableObjectDeleted += MoveObjectToDeleter;
    }

    private void OnDisable()
    {
        _deleter.MergeableObjectDeleted -= MoveObjectToDeleter;
    }

    private void Start()
    {
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(
            null,
            _deleterTransform.position
        );

        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(
            new Vector3(screenPoint.x, screenPoint.y, 10f)
        );

        worldPoint.z = 0f;

        transform.position = worldPoint;

        _deleterCollider.transform.position = worldPoint;
    }

    private void MoveObjectToDeleter(MergeableObject objectToMove)
    {
        Rigidbody2D rigidbody = objectToMove.gameObject.GetComponent<Rigidbody2D>();
        rigidbody.simulated = false;
        objectToMove.transform.DOMove(_deleterCollider.transform.position, _animationDuration);
        StartCoroutine(InvokeObjectCollidedWithDelay(objectToMove, _animationDuration));
    }

    private IEnumerator InvokeObjectCollidedWithDelay(MergeableObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        AnimationEnded?.Invoke(obj);
    }
}
