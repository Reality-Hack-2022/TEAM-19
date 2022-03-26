using UnityEngine;

public class Gift : MonoBehaviour
{
    public MonoBehaviour rotateScript;
    public MonoBehaviour moveTowardsScript;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Listen for trigger to turn off the 
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space))
        {
            ToggleScripts();
        }
    }

    void ToggleScripts()
    {
        rotateScript.enabled = !rotateScript.enabled;
        moveTowardsScript.enabled = !moveTowardsScript.enabled;
    }

    public void RotateNewTarget(GameObject target)
    {
        GetComponent<RotateAround>().target = target;
        ToggleScripts();
    }
}
