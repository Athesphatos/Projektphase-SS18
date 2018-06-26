using UnityEngine;

public class CenteredCamera : MonoBehaviour {

    public Transform center;
    public Vector3 offset;
	
	// Update is called once per frame
	void Update () {
        transform.position = center.position + offset;
	}
}
