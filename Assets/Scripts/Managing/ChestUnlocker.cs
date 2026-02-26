using UnityEngine;
using DG.Tweening;
using System.Collections;

public class ChestUnlocker : MonoBehaviour
{
    [SerializeField] private InputReader _input;
    [SerializeField] private GameObject _copyObjectSprite;
    [SerializeField] private GameObject _deletionSprite;

    [SerializeField] private RectTransform _copyObjectMoveTo;
    [SerializeField] private RectTransform _deletionMoveTo;
    [SerializeField] private float _animationDuration;

    [SerializeField] private CopyPaster _copyPaster;
    [SerializeField] private FruitsDeleter _deleter;

    private void OnEnable()
    {
        _input.Pressed += CheckIfPressedChest;
    }

    private void OnDisable()
    {
        _input.Pressed -= CheckIfPressedChest;
    }

    private void CheckIfPressedChest()
    {
        if(_input.IsPointerOverChest(out TreasureChest chest) == true)
        {
            chest.Open(out int copyObjectsCharges,
            out int deletionsCharges,
            out Transform posToSpawnCopyObjects,
            out Transform posToSpawnDeletions);

            SpawnBuffs(posToSpawnCopyObjects, posToSpawnDeletions);

            StartCoroutine(AddBuffs(copyObjectsCharges, deletionsCharges, _animationDuration));
        }
    }

    private IEnumerator AddBuffs(int chargesToAddToCopyPaster, int chargesToAddToDeleter, float delay)
    {
        yield return new WaitForSeconds(delay);

        _copyPaster.AddCharges(chargesToAddToCopyPaster);
        _deleter.AddCharges(chargesToAddToDeleter);
    }

    private void SpawnBuffs(Transform posToSpawnCopyPastObjects, Transform posToSpawnDeletions)
    {
        Transform copyPastObject = Instantiate(_copyObjectSprite, posToSpawnCopyPastObjects.position, Quaternion.identity).transform;
        Transform deletionObject = Instantiate(_deletionSprite, posToSpawnDeletions.position, Quaternion.identity).transform;

        copyPastObject.DOMove(GetWorldPosMoveTo(_copyObjectMoveTo), _animationDuration);
        deletionObject.DOMove(GetWorldPosMoveTo(_deletionMoveTo), _animationDuration);
    }

    private Vector3 GetWorldPosMoveTo(RectTransform posMoveTo)
    {
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(
            null,
            posMoveTo.position
        );

        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(
            new Vector3(screenPoint.x, screenPoint.y, 10f)
        );

        worldPoint.z = 0f;

        return worldPoint;
    }
}
