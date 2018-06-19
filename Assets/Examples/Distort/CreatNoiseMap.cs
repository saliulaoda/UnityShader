using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CreatNoiseMap : MonoBehaviour {

	// Use this for initialization   
	void Start () {
		
	}


    Texture2D CreatMap(int width, int height)
    {
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);

        //遍历像素点设置  
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int tmp = Random.Range(0, 4);
                int max = tmp * 44;
                int r = Random.Range(0, max);
                int g = Random.Range(0, max);
                int b = Random.Range(0, max);

                Color pixelcolor = new Color(r, g, b, 1.0f);
                tex.SetPixel(x, y, pixelcolor);  
            }
        }

        tex.Apply();

        return tex;
    }

    void AlbumSave(Texture2D tex, System.Action<bool> callback = null)
    {
        System.DateTime now = new System.DateTime();
        now = System.DateTime.Now;

        byte[] bytes = tex.EncodeToPNG();
        string filename = string.Format("NoiseMap{0}x{1}_{2}{3}{4}{5}.png", tex.width, tex.height, now.Day, now.Hour, now.Minute, now.Second);
        string destination = Application.persistentDataPath + "";
        if (!Directory.Exists(destination))
        {
            Directory.CreateDirectory(destination);
        }
        destination = destination + "/" + filename;
        File.WriteAllBytes(destination, bytes);
        if (callback != null)
        {
            callback(true);
        }

    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {  
            Debug.Log("Star To Creat NoiseMap ...");
            Texture2D _tmp = CreatMap(256, 256);
            AlbumSave(_tmp,(flag)=>
            {
                if(flag)
                {
                    Debug.Log("Creat NoiseMap Succeed !!!");
                }
                else
                {
                    Debug.LogError("Creat NoiseMap failed !!!");
                }
            });
        }
    }
}
