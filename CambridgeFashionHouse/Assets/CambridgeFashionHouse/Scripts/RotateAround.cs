using UnityEngine;

public class RotateAround : MonoBehaviour
{
    public GameObject target;
    public int DegPerSec = 20;
    public float radius = 0.5f;

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround (target.transform.position, Vector3.up, DegPerSec * Time.deltaTime);
         Vector3 desiredPosition = (transform.position - target.transform.position).normalized * radius + target.transform.position;
         transform.position = Vector3.MoveTowards(transform.position, desiredPosition, Time.deltaTime * DegPerSec);
        // Spin the object around the target at 20 degrees/second.
        // transform.RotateAround(target.transform.position, Vector3.up, DegPerSec * Time.deltaTime);
    }


}
