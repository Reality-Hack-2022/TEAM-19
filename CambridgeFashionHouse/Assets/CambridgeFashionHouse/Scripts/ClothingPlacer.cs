using UnityEngine;

public class ClothingPlacer : MonoBehaviour
{
    public Transform cameraTransform;
    public float yOffset = -0.3f;
    Vector3 newPos;
    Vector3 newRot;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Place this GameObject directly below the camera
        this.newPos.x = cameraTransform.position.x;
        this.newPos.y = cameraTransform.position.y + yOffset;
        this.newPos.z = cameraTransform.position.z;
        this.transform.position = newPos;

        // print("Camera" + cameraTransform.position.y);
        // print("Pos y" + this.transform.position.y);

        newRot.y = cameraTransform.eulerAngles.y;
        this.transform.eulerAngles = newRot;
    }
}
