using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/GaussianBlur_x5")]
public class GaussianBlur : ImageEffect
{
    public float BlurSize = 1;
    public int BlurIterations = 2;
    public static RenderTexture blurTexture;
    public RawImage gaussianImage;

    void Awake()
    {
    }

    void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
    {
        if (BlurSize != 0 && mShader != null)
        {
            int rtW = sourceTexture.width / 2;
            int rtH = sourceTexture.height / 2;

            RenderTexture rtTempA = RenderTexture.GetTemporary(rtW, rtH, 16, sourceTexture.format);
            rtTempA.filterMode = FilterMode.Bilinear;
            Graphics.Blit(sourceTexture, rtTempA);

            for (int i = 0; i < BlurIterations; i++)
            {
                float iteraionOffs = i * 1.0f;
                material.SetFloat("_blurSize", BlurSize + iteraionOffs);

                //vertical blur
                RenderTexture rtTempB = RenderTexture.GetTemporary(rtW, rtH, 16, sourceTexture.format);
                rtTempB.filterMode = FilterMode.Bilinear;
                Graphics.Blit(rtTempA, rtTempB, material, 0);
                RenderTexture.ReleaseTemporary(rtTempA);
                rtTempA = rtTempB;

                //horizontal blur
                rtTempB = RenderTexture.GetTemporary(rtW, rtH, 16, sourceTexture.format);
                rtTempB.filterMode = FilterMode.Bilinear;
                Graphics.Blit(rtTempA, rtTempB, material, 1);
                RenderTexture.ReleaseTemporary(rtTempA);
                rtTempA = rtTempB;
            }

            Graphics.Blit(rtTempA, destTexture);

            RenderTexture.ReleaseTemporary(rtTempA);
        }
        else
        {
            Graphics.Blit(sourceTexture, destTexture);
        }
    }
}