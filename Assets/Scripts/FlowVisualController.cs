using UnityEngine;

namespace MechaBloom
{
    public sealed class FlowVisualController : MonoBehaviour
    {
        [SerializeField] private GameObject[] flowVisuals;
        [SerializeField] private ParticleSystem[] flowParticles;

        public GameObject[] FlowVisuals => flowVisuals;
        public ParticleSystem[] FlowParticles => flowParticles;

        public void SetActive(bool active)
        {
            // Visual playback is intentionally deferred to a gameplay milestone.
        }
    }
}
