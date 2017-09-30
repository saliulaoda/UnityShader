using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Color Adjustments/Color Correction (Mask)")]
public class ColorCorrectionMask : ImageEffect
{
    public Texture textureMask;

    // Called by camera to apply image effect
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        material.SetTexture("_MaskTex", textureMask);
        Graphics.Blit(source, destination, material);
    }
}
