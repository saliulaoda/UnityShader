using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRound : MonoBehaviour {

    public float _RotationSpeed = 0.1f; //定义自转的速度
    void Start () {
        _RotationSpeed = 0.1f;
    }

	void Update () {
        transform.Rotate(Vector3.forward* _RotationSpeed, Space.World); //物体自转
	}
}
