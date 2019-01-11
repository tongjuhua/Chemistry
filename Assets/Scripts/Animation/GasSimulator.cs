using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasSimulator : MonoBehaviour {
    
	// Use this for initialization
	private Obi.ObiEmitter emitter;
	// Use this for initialization
	void Start()
	{
		emitter = this.GetComponent<Obi.ObiEmitter>();
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void MoveEimtter(Vector3 position)
	{
		emitter.transform.position = position;
	}

	public void Emit(float amount)
	{
		emitter.speed = amount;
	}

	public void StopEmitting()
	{
		emitter.speed = 0;
	}
}
