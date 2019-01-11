namespace UCE
{
    using System.Collections;
    using UnityEngine;

    public class HeatingCheckpoint : MonoBehaviour
    {
        static int cnt = 0;

        private bool hasTouched = false;
        private bool isTouching = false;
        private float lasttime = 0;

        public static bool CheckHeating()
        {
            if (cnt == 3)
            {
                return true;
            }
            return false;
        }

        void OnTriggerEnter(Collider other)
        {
            //Debug.Log(other.name + " enter tube");
            if (!hasTouched && other.name == "Flames" && other.GetComponent<UCE_Heatable>().isHeating)
            {
                lasttime = Time.time;
                isTouching = true;
                StartCoroutine(StayAWhile());
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (!hasTouched && other.name == "Flames")
            {
                isTouching = false;
            }
        }

        IEnumerator StayAWhile()
        {
            yield return new WaitForSeconds(0.2f);
            if (isTouching && Time.time - lasttime > 0.199f && !hasTouched)
            {
                if (!TipBoard.Progress(2, ++cnt)) {
                    cnt = 0;
                }
                else {
                    hasTouched = true;
                    Debug.Log("Heating Checkpoint cnt = " + cnt.ToString());
                }
            }
        }
    }
}