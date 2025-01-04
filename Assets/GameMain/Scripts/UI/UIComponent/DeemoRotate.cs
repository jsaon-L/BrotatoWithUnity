using UnityEngine;
using System.Collections;

public class DeemoRotate : MonoBehaviour {

    public bool isLocal = true;
    public Vector3 rotateSpeed;
        
	void Update () {
        transform.Rotate(rotateSpeed.x * Time.deltaTime, rotateSpeed.y * Time.deltaTime, rotateSpeed.z * Time.deltaTime, (isLocal ? Space.Self : Space.World));
    }
}
