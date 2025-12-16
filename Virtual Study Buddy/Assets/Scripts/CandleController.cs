using UnityEngine;

public class CandleController : MonoBehaviour
{
    public ParticleSystem flame;
    public ParticleSystem smoke;
    public AudioSource crackling;
    public Light candleLight;

    public void TurnOn()
    {
        if (!flame.isPlaying)
            flame.Play();

        if (!smoke.isPlaying)
            smoke.Play();

        if (!crackling.isPlaying)
            crackling.Play();

        candleLight.enabled = true;
    }

    public void TurnOff()
    {
        flame.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        smoke.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        crackling.Stop();
        candleLight.enabled = false;
    }

    void Start()
    {
        TurnOff();
    }
}
