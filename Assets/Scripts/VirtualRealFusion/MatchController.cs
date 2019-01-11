using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchController : MonoBehaviour {

    [HideInInspector]
    public bool onFire = false;

    public void SetFire() {
        onFire = true;
        GetComponent<Fire>().SetFire();
    }

    public void PutOutFire() {
        onFire = false;
        GetComponent<Fire>().PutOutFire();
    }
}
