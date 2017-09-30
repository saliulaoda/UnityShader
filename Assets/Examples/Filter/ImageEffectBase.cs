using System;
using UnityEngine;

[RequireComponent(typeof (Camera))]
[AddComponentMenu("")]
public class ImageEffect : MonoBehaviour{
    private Material mMaterial;
    public Shader mShader;

    protected Material material{
        get{
            if (null == mMaterial){
                mMaterial = new Material(mShader);
                mMaterial.hideFlags = HideFlags.HideAndDontSave;
            }
            return mMaterial;
        }
    }

    protected virtual void Start(){
        // Disable if we don't support image effects
        if (!SystemInfo.supportsImageEffects){
            enabled = false;
            return;
        }

        if (QualitySettings.GetQualityLevel() <= 1){
            enabled = false;
            return;
        }

        // Disable the image effect if the shader can't run on the users graphics card
        if (!mShader || !mShader.isSupported)
            enabled = false;
    }

    protected virtual void OnDisable(){
        if (mMaterial){
            DestroyImmediate(mMaterial);
        }
    }
}
