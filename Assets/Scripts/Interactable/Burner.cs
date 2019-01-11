

namespace UCE
{
    using UnityEngine;

    // TODO: add animation and use coroutine to wait before set fire
    public class Burner : MonoBehaviour {

        public GameObject tubeBreak;
        [HideInInspector]
        public bool lidIsOn = true;

        private bool onFire = false;

        void OnTriggerEnter(Collider other)
        {
            if (!onFire)
            {
                if (!lidIsOn)
                {
                    Match match = other.GetComponent<Match>();
                    if (match && match.onFire)
                    {
                        SetFire();
                    }
                }
            }
        }

        public void SetFire()
        {
            GetComponent<Fire>().SetFire();
            transform.Find("Flames").GetComponent<UCE_Heatable>().SetFire();
            onFire = true;
            TipBoard.Progress(0, 1);
            TipBoard.Progress(2, 0);
        }

        public void PutOutFire()
        {
            if (onFire && TipBoard.sn == 4 && TipBoard.ssn == 0)
            {
                tubeBreak.GetComponent<TubeExplode>().Explode();
            }
            GetComponent<Fire>().PutOutFire();
            transform.Find("Flames").GetComponent<UCE_Heatable>().PutOutFire();
            onFire = false;
            TipBoard.Progress(1, 1);
            TipBoard.Progress(4, 1);
        }

        void FixedUpdate()
        {
           // Debug.Log(transform.parent.rotation);
            if (onFire && (transform.parent.rotation.x < -0.5 || transform.parent.rotation.x > 0.5 || transform.parent.rotation.z < -0.5
                || transform.parent.rotation.z > 0.5))
            {
                // TODO
                GameObject.Find("FlamesParticleEffect").GetComponent<ParticleSystem>().Play();
            }
        }
    }
}