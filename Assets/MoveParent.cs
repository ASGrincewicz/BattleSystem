using UnityEngine;

public class MoveParent : MonoBehaviour
{
    public Transform newParent;
    public Transform currentParent;

    private void Start()
    {
        currentParent = this.transform.parent;

    }
    public void ChangeThisParent()
    {
        transform.parent = newParent;
    }
}
