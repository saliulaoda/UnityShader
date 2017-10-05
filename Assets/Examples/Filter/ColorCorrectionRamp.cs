using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Color Adjustments/Color Correction (Ramp)")]
public class ColorCorrectionRamp : ImageEffect
{
    public Texture  textureRamp;
    [Range(0.0f, 2.0f)]
    public float saturation = 1.0f;
    [Range(0.0f, 1.0f)]
    public float brightness = 1.0f;
    [Range(0.0f, 1.0f)]
    public float contrast = 1.0f;
    // Called by camera to apply image effect
    void OnRenderImage (RenderTexture source, RenderTexture destination) {
        material.SetTexture ("_RampTex", textureRamp);
        material.SetFloat("_Saturation", saturation);
        material.SetFloat("_Brightness", brightness);
        material.SetFloat("_Contrast", contrast);
        Graphics.Blit (source, destination, material);
    }
}
