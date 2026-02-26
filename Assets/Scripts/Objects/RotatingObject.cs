using UnityEngine;
using DG.Tweening;

public class RotatingObject : MonoBehaviour
{
    [SerializeField] private Vector3 _angleToMove;
    [SerializeField] private bool _isRotatable;
    [SerializeField] private float _rotatingDuration;

    private void Start()
    {
        if(_isRotatable == false)
            return;

    transform
        .DORotate(_angleToMove, _rotatingDuration, RotateMode.FastBeyond360)
        .SetLoops(-1, LoopType.Incremental)
        .SetEase(Ease.Linear)
        .SetLink(gameObject);
    }

    public void StopRotation() 
    {
        _isRotatable = false;
        transform.DOKill();
    }
}
