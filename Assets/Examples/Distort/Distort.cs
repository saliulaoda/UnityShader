﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Distort")]
public class Distort : ImageEffect
{
    //扭曲的时间系数  
    [Range(0.0f, 1.0f)]
    public float DistortTimeFactor = 0.15f;
    //扭曲的强度  
    [Range(0.0f, 0.2f)]
    public float DistortStrength = 0.01f;
    //噪声图  
    public Texture NoiseTexture = null;
    //渲染Mask图所用的shader  
    public Shader maskObjShader = null;
    //降采样系数  
    public int downSample = 4;

    private Camera mainCam = null;
    private Camera additionalCam = null;
    private RenderTexture renderTexture = null;

    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (material)
        {
            material.SetTexture("_NoiseTex", NoiseTexture);
            material.SetFloat("_DistortTimeFactor", DistortTimeFactor);
            material.SetFloat("_DistortStrength", DistortStrength);
            material.SetTexture("_MaskTex", renderTexture);
            Graphics.Blit(source, destination, material);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }

    void Awake()
    {
        //创建一个和当前相机一致的相机  
        InitAdditionalCam();
    }

    private void InitAdditionalCam()
    {
        mainCam = GetComponent<Camera>();
        if (mainCam == null)
            return;

        Transform addCamTransform = transform.FindChild("additionalDistortCam");
        if (addCamTransform != null)
            DestroyImmediate(addCamTransform.gameObject);

        GameObject additionalCamObj = new GameObject("additionalDistortCam");
        additionalCam = additionalCamObj.AddComponent<Camera>();

        SetAdditionalCam();
    }

    private void SetAdditionalCam()
    {
        if (additionalCam)
        {
            additionalCam.transform.parent = mainCam.transform;
            additionalCam.transform.localPosition = Vector3.zero;
            additionalCam.transform.localRotation = Quaternion.identity;
            additionalCam.transform.localScale = Vector3.one;
            additionalCam.farClipPlane = mainCam.farClipPlane;
            additionalCam.nearClipPlane = mainCam.nearClipPlane;
            additionalCam.fieldOfView = mainCam.fieldOfView;
            additionalCam.backgroundColor = Color.clear;
            additionalCam.clearFlags = CameraClearFlags.Color;
            additionalCam.cullingMask = 1 << LayerMask.NameToLayer("Distort");
            additionalCam.depth = -999;
            //分辨率可以低一些  
            if (renderTexture == null)
                renderTexture = RenderTexture.GetTemporary(Screen.width >> downSample, Screen.height >> downSample, 0);
        }
    }

    void OnEnable()
    {
        SetAdditionalCam();
        additionalCam.enabled = true;
    }

    void OnDisable()
    {
        additionalCam.enabled = false;
    }

    void OnDestroy()
    {
        if (renderTexture)
        {
            RenderTexture.ReleaseTemporary(renderTexture);
        }
        DestroyImmediate(additionalCam.gameObject);
    }

    //在真正渲染前的回调，此处渲染Mask遮罩图  
    void OnPreRender()
    {
        //maskObjShader进行渲染  
        if (additionalCam.enabled)
        {
            additionalCam.targetTexture = renderTexture;
            additionalCam.RenderWithShader(maskObjShader, "");
        }
    }
}