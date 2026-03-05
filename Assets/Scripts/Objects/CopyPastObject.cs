using UnityEngine;

public class CopyPastObject : MergeableObject
{
    public override void OnCollisionEnter2D(Collision2D coll)
    {
        CheckIfCollIsWall(coll);

        InvokeCollided();

        if(_canCollideWithDragableObjects == false)
            return;

        if(coll.gameObject.TryGetComponent<MergeableObject>(out MergeableObject obj) == true)
        {
            ChangeMergeableObjectLevel(obj.GetMergeableObjectLevel());
            InvokeCollidedWithDragableObject(obj);
        }
    }
}
