using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallowDualPlayer : MonoBehaviour
{

    public float CameraMoveSpeed = 120.0f;
    public GameObject cameraFollowObj1;
    public GameObject cameraFollowObj2;
    Vector3 followPOS;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        CameraUpdater();
    }

    void CameraUpdater()
    {
        Transform target1 = cameraFollowObj1.transform;
        Transform target2 = cameraFollowObj2.transform;

        print(target1.position.x);

        float step = CameraMoveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, (target1.position + target2.position)/2, step);
    }
}
