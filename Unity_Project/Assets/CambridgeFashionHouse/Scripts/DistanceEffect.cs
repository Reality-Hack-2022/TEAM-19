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
        float normalizedValue = (value - minDist) / (maxDist - minDist);
        if (normalizedValue > maxDist) normalizedValue = maxDist;
        if (normalizedValue < minDist) normalizedValue = minDist;
        return normalizedValue;
    }
}
