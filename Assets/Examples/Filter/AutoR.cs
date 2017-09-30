using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoR : MonoBehaviour {

    public float _RotationSpeed = 0.5f;
    void Start () {
	}

	void Update () {
        transform.Rotate(Vector3.down* _RotationSpeed, Space.World);
	}
}
