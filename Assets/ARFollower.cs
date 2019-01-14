using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARFollower : MonoBehaviour {

    public Transform Leader;

    public float LerpSpeed = 0.2f;

	// Update is called once per frame
	void Update () {
        transform.position = Vector3.Lerp(transform.position, Leader.position, LerpSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, Leader.rotation, LerpSpeed);
	}
}
