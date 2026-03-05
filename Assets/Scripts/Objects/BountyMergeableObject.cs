using UnityEngine;

public class BountyMergeableObject : MonoBehaviour
{
    [SerializeField] private int _bountyForMerge;

    public int GetBounty()
    {
        return _bountyForMerge;
    }
}
