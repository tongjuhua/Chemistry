﻿namespace UCE
{
    using UnityEngine;

    public class Bottle : MonoBehaviour
    {
        
        [Tooltip("This variable decides how fast the water surface decrease")]
        public float collectVelocity = 0.5f;
	    public Obi.ObiEmitter emitter;
        private bool isCollecting = false;
        private float height = 1.2f;
        private float trueHeight = 1.0f;

        public void StartCollecting()
        {
            isCollecting = true;
        }

        public void StopCollecting()
        {
            isCollecting = false;
        }

        // ideal input: 0.3/s
        public void AddAir(float amount)
        {
            //emitter.speed = amount;
            height -= amount * collectVelocity;
            trueHeight = height;
            if (trueHeight > 1.000f) {
                trueHeight = 1.000f;
            }

            if (trueHeight < 0.001f)
            {
               // emitter.speed = 0.5f * collectVelocity;
                trueHeight = 0.001f;
                Debug.Log("Add air complete");
                //transform.Find("bottleWaterUp").gameObject.SetActive(false);
            }
            transform.Find("bottleWaterUp").localScale = new Vector3(1f, trueHeight, 1f);
            transform.Find("bottleWaterUp").Find("water").gameObject.SetActive(false);
            transform.Find("bottleWaterDown").gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (isCollecting)
            {
                AddAir(Time.deltaTime * 0.3f);
            }
        }
    }
}