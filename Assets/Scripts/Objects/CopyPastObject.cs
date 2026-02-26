using UnityEngine;

public class CopyPastObject : Fruit
{
    public override void OnCollisionEnter2D(Collision2D coll)
    {
        CheckIfCollIsWall(coll);

        InvokeCollided();

        if(_canCollideWithDragableObjects == false)
            return;

        if(coll.gameObject.TryGetComponent<Fruit>(out Fruit obj) == true)
        {
            ChangeFruitType(obj.GetFruitType());
            InvokeCollidedWithDragableObject(obj);
        }
    }
}
