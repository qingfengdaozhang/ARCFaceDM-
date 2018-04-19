using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AndroidScripts : MonoBehaviour
{
    public static AndroidScripts Instance { set; get; }
    // Use this for initialization
    void Start()
    {
        Instance = this;
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        string res = jo.Call<string>("Test");
        //DebugText.Out(res);
    }
    /// <summary>
    /// 打开相册
    /// </summary>

    public void OpenPhoto()
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        jo.Call("OpenGallery");
    }
    /// <summary>
    /// 打开相机
    /// </summary>

    public void OpenCamera()
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        jo.Call("takephoto");
    }

    public void GetImagePath(string imagePath)
    {
        if (imagePath == null)
            return;
        StartCoroutine(LoadImage(imagePath));
    }
    
    public void GetTakeImagePath(string imagePath)
    {
        if (imagePath == null)
            return;
        StartCoroutine(LoadImage(imagePath));
    }

    private IEnumerator LoadImage(string imagePath)
    {
        WWW www = new WWW("file://" + imagePath);
        yield return www;
        if (www.error == null)
        {
            //成功读取图片，写自己的逻辑 
            // DebugText.Out("正在缓存");
            Texture2D tex = www.texture;
            Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            PostController.Instance.image.sprite = sprite;
            PostController.Instance.imgBytes = tex.EncodeToJPG();
        }
        else
        {
            Debug.Log("read error");
        }
    }
}
