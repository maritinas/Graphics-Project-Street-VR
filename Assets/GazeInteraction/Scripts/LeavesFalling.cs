using UnityEngine;

public class LeavesFalling : MonoBehaviour
{
    // Reference to the ParticleSystem component
    public ParticleSystem particleSystem;


    // Function to stop emitting further particles
    public void StopEmitting()
    {
        if (particleSystem.isPlaying)
        {
            particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }

    // Function to start emitting particles (if needed)
    public void StartEmitting()
    {
        if (!particleSystem.isPlaying)
        {
            particleSystem.Play();
        }
    }
}


