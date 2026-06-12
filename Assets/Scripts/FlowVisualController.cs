using UnityEngine;

namespace MechaBloom
{
    public sealed class FlowVisualController : MonoBehaviour
    {
        [SerializeField] private GameObject[] flowVisuals;
        [SerializeField] private ParticleSystem[] flowParticles;

        public void SetActive(bool active)
        {
            if (flowVisuals != null)
            {
                foreach (var item in flowVisuals)
                {
                    if (item != null)
                    {
                        item.SetActive(active);
                    }
                }
            }

            if (flowParticles == null)
            {
                return;
            }

            foreach (var particle in flowParticles)
            {
                if (particle == null)
                {
                    continue;
                }

                if (active)
                {
                    particle.Play();
                }
                else
                {
                    particle.Stop();
                }
            }
        }
    }
}
