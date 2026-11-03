using UnityEngine;
using UnityEngine.InputSystem;

// Press SPACE to drop a copy of the ball prefab from a random spot 10 units up.
// Put this script on any GameObject in the scene (for example, an empty "Spawner").
public class BallSpawner : MonoBehaviour
{
    // Drag your Ball prefab (from the Prefabs folder) here in the Inspector.
    // The prefab needs a Sphere Collider (with the bouncy material) and a Rigidbody.
    public GameObject ballPrefab;

    // How high the ball starts.
    public float spawnHeight = 10f;

    // How far from the center (left/right and forward/back) a ball can appear.
    // A default Unity Plane is 10 x 10, so 4 keeps the balls over the ground.
    public float spawnRange = 4f;

    void Update()
    {
        // Did the player press the space bar this frame?
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            // Pick a random X and Z between -spawnRange and +spawnRange.
            float randomX = Random.Range(-spawnRange, spawnRange);
            float randomZ = Random.Range(-spawnRange, spawnRange);

            Vector3 spawnPosition = new Vector3(randomX, spawnHeight, randomZ);

            // Make a new copy of the prefab at that position, with no rotation.
            Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
