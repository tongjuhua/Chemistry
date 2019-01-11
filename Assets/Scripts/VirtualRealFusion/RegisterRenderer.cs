using UnityEngine;
using Obi;

[RequireComponent(typeof(ObiParticleRenderer))]
public class RegisterRenderer : MonoBehaviour {

    public int registerID;
    public ObiParticleRenderer selfParticleRenderer;
    public ObiFluidRenderer obiFluidRenderer;

	// Use this for initialization
	void Awake () {
        if (selfParticleRenderer == null) {
            selfParticleRenderer = GetComponent<ObiParticleRenderer>();
        }

        if (obiFluidRenderer == null) {
            obiFluidRenderer = Camera.main.GetComponent<ObiFluidRenderer>();
        }

        if (obiFluidRenderer != null) {
            obiFluidRenderer.particleRenderers[registerID] = selfParticleRenderer;
        }
    }
}
