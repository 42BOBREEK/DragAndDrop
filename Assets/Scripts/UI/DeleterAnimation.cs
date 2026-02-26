using UnityEngine;
using System;
using System.Collections;
using DG.Tweening;

public class DeleterAnimation : MonoBehaviour
{
    [SerializeField] private FruitsDeleter _deleter;
    [SerializeField] private RectTransform _deleterTransform;
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _deleterCollider;
    [SerializeField] private float _animationDuration;

    public event Action<Fruit> AnimationEnded;

    private void OnEnable()
    {
        _deleter.FruitDeleted += MoveFruitToDeleter;
    }

    private void OnDisable()
    {
        _deleter.FruitDeleted -= MoveFruitToDeleter;
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

    private void MoveFruitToDeleter(Fruit fruitToMove)
    {
        Rigidbody2D rigidbody = fruitToMove.gameObject.GetComponent<Rigidbody2D>();
        rigidbody.simulated = false;
        fruitToMove.transform.DOMove(_deleterCollider.transform.position, _animationDuration);
        StartCoroutine(InvokeFruitCollidedWithDelay(fruitToMove, _animationDuration));
    }

    private IEnumerator InvokeFruitCollidedWithDelay(Fruit fruit, float delay)
    {
        yield return new WaitForSeconds(delay);
        AnimationEnded?.Invoke(fruit);
    }
}
