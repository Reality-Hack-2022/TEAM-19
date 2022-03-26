using UnityEngine;

public class RotateAround : MonoBehaviour
{
    public GameObject target;
    public int DegPerSec = 20;

    // Update is called once per frame
    void Update()
    {
        // Spin the object around the target at 20 degrees/second.
        transform.RotateAround(target.transform.position, Vector3.up, DegPerSec * Time.deltaTime);
    }
}
