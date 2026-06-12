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
            if (flowVisuals != null)
            {
                foreach (var visual in flowVisuals)
                {
                    if (visual != null)
                    {
                        visual.SetActive(active);
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
                    particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                }
            }
        }
    }
}
