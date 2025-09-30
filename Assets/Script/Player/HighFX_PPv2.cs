using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using System.Collections;
using System;

public class HighFX_PPv2 : MonoBehaviour
{
    public PostProcessVolume volume;   // tu Post-process Volume (Is Global)
    public float enterTime = 0.6f, exitTime = 0.8f;

    // intensidades máximas
    public float chromaMax = 0.35f;
    public float lensDistortMax = -30f; // PPv2 usa -100..100 aprox.
    public float vignetteMax = 0.35f;
    public float bloomMax = 10f;       // depende de tu escena/escala
    public float hueShiftMax = 15f;

    ChromaticAberration ca;
    LensDistortion ld;
    Vignette vg;
    Bloom bl;
    ColorGrading cg;

    void Awake()
    {
        EventsManagerSpaar.SubscribeToEvent(EventTypeSpaar.ShaderSmoked, PlayShader);

        volume.profile.TryGetSettings(out ca);
        volume.profile.TryGetSettings(out ld);
        volume.profile.TryGetSettings(out vg);
        volume.profile.TryGetSettings(out bl);
        volume.profile.TryGetSettings(out cg);

        // Set a 0
        SetK(0f);
        volume.weight = 0f; // PPv2 también tiene 'weight' para mezclar volúmenes
    }

    private void PlayShader(object[] parameterContainer)
    {
        if ((bool)parameterContainer[0])
            StartTrip(10f);
    }


    void Start()
    {
        //   StartTrip(10);
    }

    public void StartTrip(float sustainSeconds = 5f)
    {
        StopAllCoroutines();
        StartCoroutine(TripRoutine(sustainSeconds));
    }

    IEnumerator TripRoutine(float sustain)
    {
        yield return LerpWeight(0f, 1f, enterTime);
        float t0 = Time.time;
        while (Time.time - t0 < sustain)
        {
            float wob = Mathf.Sin(Time.time * 2f) * 0.12f; // ondulación leve
            Apply(1f, wob);
            yield return null;
        }
        yield return LerpWeight(1f, 0f, exitTime);
        SetK(0f);
        EventsManagerSpaar.TriggerEvent(EventTypeSpaar.ShaderSmoked, false);
    }

    IEnumerator LerpWeight(float a, float b, float dur)
    {
        float t = 0f;
        while (t < dur)
        {
            t += Time.deltaTime;
            float k = Mathf.SmoothStep(a, b, t / dur);
            volume.weight = k;   // mezcla global
            Apply(k, 0f);        // y además ajustamos intensidades
            yield return null;
        }
        volume.weight = b;
    }

    void Apply(float k, float wobble)
    {
        if (ca != null) { ca.intensity.value = chromaMax * k; ca.active = k > 0; }
        if (ld != null) { ld.intensity.value = lensDistortMax * (k + wobble); ld.active = k > 0; }
        if (vg != null) { vg.intensity.value = vignetteMax * (k + Mathf.Abs(wobble) * 0.6f); vg.active = k > 0; }
        if (bl != null) { bl.intensity.value = bloomMax * k; bl.active = k > 0; }
        if (cg != null) { cg.hueShift.value = hueShiftMax * (k * Mathf.Sin(Time.time * 0.6f)); cg.active = k > 0; }
    }

    void SetK(float k) => Apply(k, 0f);
}
