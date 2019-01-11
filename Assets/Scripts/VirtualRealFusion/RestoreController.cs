using UnityEngine;
using Obi;

// Restore the system
public class RestoreController : MonoBehaviour {

    [Header("Destroy Objects")]
    public string needDestroyTag;
    private GameObject[] needDestroyObjects;

    [Header("Restore Objects")]
    public GameObject[] needRestoreObjects;

    [Header("Restore Optitrack Tracking")]
    public OptitrackViveTransform ovTransfrom;

    [Header("Restore Obi Renderer")]
    public ObiFluidRenderer obiRenderer;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.Space)) {
            needDestroyObjects = GameObject.FindGameObjectsWithTag("Restore");

            for (int i = 0; i < needDestroyObjects.Length; i++) {
                DestroyImmediate(needDestroyObjects[i]);
            }

            for (int i = 0; i < needRestoreObjects.Length; i++) {
                Instantiate(needRestoreObjects[i]);
            }

            ovTransfrom.FindObjects();
        }
	}
}
