using UnityEngine;

namespace DnVCorp
{
    namespace Manager
    {
        public static class ParticleManager
        {
            public static void PlayParticle(ParticleSystem particle)
            {
                if (particle.gameObject.activeInHierarchy) particle.Play();
                else particle.gameObject.SetActive(true);
            }
            public static void PlayParticle(ParticleSystem particle, bool withChildren)
            {
                if (particle.gameObject.activeInHierarchy) particle.Play(withChildren);
                else particle.gameObject.SetActive(true);
            }
            public static void StopParticle(ParticleSystem particle)
            {
                if (particle.gameObject.activeInHierarchy) particle.Stop();
            }
            public static void StopParticle(ParticleSystem particle, bool withChildren)
            {
                if (particle.gameObject.activeInHierarchy) particle.Stop(withChildren);
            }
            public static void StopParticle(ParticleSystem particle, bool withChildren, ParticleSystemStopBehavior stopBehavior)
            {
                if (particle.gameObject.activeInHierarchy) particle.Stop(withChildren, stopBehavior);
            }
        }
    }
}
