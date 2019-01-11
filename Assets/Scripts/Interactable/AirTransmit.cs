namespace UCE
{
    using UnityEngine;

    public class AirTransmit : MonoBehaviour
    {
        public AirTransmit next = null;
        public bool doPresentation = false;
        [HideInInspector]
        public AirTransmit last = null, end = null;
        public Obi.ObiEmitter emitter;
        public Obi.ObiEmitter bottleEmitter;
        public GameObject Cube;

        public static void Connect(AirTransmit airFrom, AirTransmit airTo)
        {
            if (!airFrom || !airTo)
            {
                Debug.LogError("AirTransmit: Connect objects without script");
                return;
            }
            //Debug.Log(airFrom.name + " is connected to " + airTo.name);
            airTo.last = airFrom;

            AirTransmit cur = airFrom;
            while (cur)
            {
                cur.end = airTo.end;
                cur = cur.last;
            }
        }

        public static void Disconnect(AirTransmit airFrom, AirTransmit airTo)
        {
            if (!airFrom || !airTo)
            {
                Debug.LogError("AirTransmit: Disconnect objects without script");
                return;
            }
            if (airFrom != airTo.last)
            {
                Debug.LogError("AirTransmit: Disconnect objects that aren't connected");
                return;
            }
            //Debug.Log(airFrom.name + " is disconnected with " + airTo.name);
            airTo.last = null;

            AirTransmit cur = airFrom;
            while (cur)
            {
                cur.end = airFrom;
                cur = cur.last;
            }
        }

        // ideal input: 0.3/s
        public void GenerateAir(float amount)
        {
            if (amount < 0.0001f) {
                if (emitter)
                emitter.speed = 0;
                if (bottleEmitter)
                    bottleEmitter.speed = 0;
            }
            //Debug.Log("Generate from [" + name + "] to [" + end.gameObject.name + "] with " + amount.ToString());
            Bottle bottle = end.gameObject.GetComponent<Bottle>();
            if (bottle)
            {
                if (Cube) {
                    if (Cube.activeInHierarchy)
                    {
                        DestroyImmediate(Cube);
                        //Cube.GetComponent<Obi.ObiCollider>().enabled = false;
                    }
                }

                //Debug.Log(name + "bottle"+amount);
                if (bottleEmitter) {
                    bottleEmitter.speed = 0.5f;
                    //bottleEmitter.lifespan = 1f;
                    //print("bottleIn");
                }
                TipBoard.Progress(3, 1);
                bottle.AddAir(amount);
                return;
            }
            PipeTrigger pipe = end.gameObject.GetComponent<PipeTrigger>();
            if (pipe && pipe.isGood)
            {
                // TODO: animation
                if (emitter)
                    emitter.speed = amount*0.1f;
                TipBoard.Progress(0, 2);
            }
        }

        // Use this for initialization
        void Start()
        {
            if (next)
            {
                end = next;
                next.last = this;
            }
            else
            {
                end = this;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (doPresentation)
            {
                GenerateAir(0.3f * Time.deltaTime);
            }
        }
    }
}