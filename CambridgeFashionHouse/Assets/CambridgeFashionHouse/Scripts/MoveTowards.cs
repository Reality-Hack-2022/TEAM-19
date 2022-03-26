using UnityEngine;

public class MoveTowards : MonoBehaviour
{
    // Adjust the speed for the application.
    public float speed = 1.0f;

    // The target (cylinder) position.
    public GameObject target;

    void Update()
    {
        // Move our position a step closer to the target.
        float step =  speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);

        // Check if the position of the cube and sphere are approximately equal.
        if (Vector3.Distance(transform.position, target.transform.position) < 0.001f)
        {
            GetComponent<Gift>().RotateNewTarget(target);
        }
    }
}