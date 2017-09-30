using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using UnityEngine.UI;
using UnityEngine.Events;


public class EffectNode
{
    public string mStrKeyName = null;
    public string mTexPath = null;
    public float mShaderParamSaturation = 1f;
    public Texture mTexture = null;

    public EffectNode(string _name, string _texPath, float _param)
    {
        mTexture = null;
        mStrKeyName = _name;
        mTexPath = _texPath;
        mShaderParamSaturation = _param;
    }
}

public class GameMain : MonoBehaviour {

    ColorCorrectionRamp mColorRamp;

    GameObject lastSelect;

    Transform mTranGrid;
    Transform mTranBtnBase;
    List<EffectNode> mData = new List<EffectNode>();
    void Start () {

        mColorRamp = GameObject.Find("Main Camera").transform.GetComponent<ColorCorrectionRamp>();

        mTranGrid = GameObject.Find("Canvas/Grid").transform;
        mTranBtnBase = GameObject.Find("Canvas/Grid/Button").transform;

        lastSelect = null;

        mData.Clear();
        mData.Add(new EffectNode("原图", "", 1f));
        mData.Add(new EffectNode("复古", "Ramp_FuGu.png", 1f));
        mData.Add(new EffectNode("黑白", "Ramp_HeiBai.png", 0f));
        mData.Add(new EffectNode("冷艳", "Ramp_LengYan.png", 1f));
        mData.Add(new EffectNode("MONO", "Ramp_LOMO.png", 1f));
        mData.Add(new EffectNode("梦幻", "Ramp_MengHuan.png", 1f));

        for (int i=0;i<mData.Count;++i)
        {
            //Debug.Log("================================ " + i + " : " + mData[i].mStrKeyName);
            GameObject _go = GameObject.Instantiate(mTranBtnBase.gameObject);
            _go.name = "Button_" + i;
            _go.transform.SetParent(mTranGrid);
            _go.transform.localScale = Vector3.one;
            _go.SetActive(true);
            _go.transform.Find("Text").GetComponent<Text>().text = mData[i].mStrKeyName;

            GameObject _selImg = _go.transform.Find("Image").gameObject;
            if(lastSelect == null && mData[i].mTexPath == "")
            {
                lastSelect = _selImg;
            }
            _selImg.SetActive(mData[i].mTexPath == "");

            Button _btn = _go.transform.GetComponent<Button>();

            _btn.onClick.AddListener(delegate () {
                OnClick(_go);
            });
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator LoadTexture(EffectNode _data, Action<Texture> _callBack)
    {
        Texture _tex = null;
        string path = "";
        path = "file:///" + Path.Combine(Application.dataPath, "Examples/Filter/ColorCorrect/" + _data.mTexPath);

        WWW _www = new WWW(path);
        yield return _www;
        if (string.IsNullOrEmpty(_www.error))
        {
            _tex = _www.texture;
        }

        if (_callBack != null)
        {
            _callBack(_tex);
        }
    }

    public void OnClick(GameObject _go)
    {
        //Debug.Log("Onclick - " + _go);
        string[] _names = _go.name.Split('_');
        int _index = System.Int32.Parse(_names[1]);
        EffectNode _data = mData[_index];
        if(_data == null)
        {
            Debug.LogError("Null Data In Index : " + _index);
            return;
        }

        GameObject _selImg = _go.transform.Find("Image").gameObject;
        _selImg.SetActive(true);
        if (lastSelect != null)
        {
            lastSelect.SetActive(false);
            lastSelect = _selImg;
        }

        if (_data.mTexture == null && _data.mTexPath != "")
        {
            StartCoroutine(LoadTexture(_data, (_texture) =>
            {
                _data.mTexture = _texture;
                RefreshImageEffect(_data);
            }
            ));
        }
        else
        {
            RefreshImageEffect(_data);
        }
    }

    void RefreshImageEffect(EffectNode _data)
    {
        if(_data != null)
        {
            mColorRamp.textureRamp = _data.mTexture;
            mColorRamp.saturation = _data.mShaderParamSaturation;
        }
    }
}
