using UnityEngine;

public class DistanceEffect : MonoBehaviour
{
    public Transform other;
    public Material otherMaterial;
    public float minDist = 0;
    public float maxDist = 1;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (other)
        {
            float dist = Vector3.Distance(other.position, transform.position);
            // print("Distance to other: " + normalize(dist));
            otherMaterial.color = new Color(1,normalize(dist),1);
        }
    }

    float normalize(float value)
    {
        if (value > maxDist) value = maxDist;
        if (value < minDist) value = minDist;
        float normalizedValue = (value - minDist) / (maxDist - minDist);
        return normalizedValue;
    }
}
