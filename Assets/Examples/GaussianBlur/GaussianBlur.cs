﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/GaussianBlur_x5")]
public class GaussianBlur : ImageEffect
{
    [Range(0.0f, 2.0f)]
    public float BlurSize = 1;
    [Range(0, 3)]
    public int BlurIterations = 2;
    [Range(1, 8)]
    public int DownSample = 2;

    public static RenderTexture blurTexture;
    public RawImage gaussianImage;

    void Awake()
    {
    }

    void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
    {
        if (BlurSize != 0 && mShader != null)
        {
            int rtW = sourceTexture.width / DownSample;
            int rtH = sourceTexture.height / DownSample;
            //降采样，提升效率，也对最终模糊效果有影响
            //定义一张分辨率是1/DownSample原尺寸的RenderTexture，把sourceTexture缩放到里面
            RenderTexture rtTempA = RenderTexture.GetTemporary(rtW, rtH, 16, sourceTexture.format);
            rtTempA.filterMode = FilterMode.Bilinear;
            Graphics.Blit(sourceTexture, rtTempA);//缩放到rtTempA里面，完成采样

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