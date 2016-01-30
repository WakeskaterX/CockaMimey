using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
	  float horiz = Input.GetAxis("Horizontal");
    float vert = Input.GetAxis("Vertical");
    float tilt = Input.GetAxis("Tilt");
    bool reset = Input.GetButton("Reset");
    if (horiz != 0) {
      transform.Rotate(new Vector3(0, horiz * 3, 0));
    }

    if (vert != 0) {
      transform.Rotate(new Vector3(vert * -3, 0, 0));
    }

    if (tilt != 0) {
      transform.Rotate(new Vector3(0, 0, tilt * -3));
    }

    if (reset) {
      transform.rotation = Quaternion.Euler(new Vector3(0,0,0));
    }
	}
}
