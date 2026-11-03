using UnityEngine;

// Removes the ball once it falls below a certain height
// (for example, after it rolls off the edge of the ground).
// Put this script on the Ball prefab.
public class DestroyBelowGround : MonoBehaviour
{
    // If the ball's Y position goes lower than this, it gets destroyed.
    // The ground is at Y = 0, so -5 gives it a moment to fall out of view.
    public float destroyHeight = -5f;

    void Update()
    {
        if (transform.position.y < destroyHeight)
        {
            Destroy(gameObject);
        }
    }
}
