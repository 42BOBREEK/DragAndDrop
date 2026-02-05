using UnityEngine;

public class BountyFruit : MonoBehaviour
{
    [SerializeField] private int _bountyForMerge;

    public int GetBounty()
    {
        return _bountyForMerge;
    }
}
