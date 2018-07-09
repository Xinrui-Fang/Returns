using UnityEngine;
using System.Collections;
using System.IO;

public class Example : MonoBehaviour {

    //分享本地纹理
    public Texture2D texture;
	void Start () {
        //设置Umeng Appkey
		Social.SetAppKey("559d34a567e58e808000167d");
	}



    void OnGUI()
    {


        if (GUI.Button(new Rect(150, 100, 500, 100), "打开分享面板"))
        {
            //按需设置您需要的平台
            Platform[] platforms = { Platform.DOUBAN, Platform.INSTAGRAM, Platform.QQ, Platform.LAIWANG, Platform.SINA};


            //接入instagram
            Social.OpenInstagram();
            //接入QQ
            Social.SetQQAppIdAndAppKey("100424468", "c7394704798a158208a74ab60104f0ba");
            //接入来往
            Social.SetLaiwangAppInfo("laiwangd497e70d4", "d497e70d4c3e4efeab1381476bac4c5e", "");






           
            

            //设置分享回调
            //如果无需回调 OpenShareWithImagePath最后一个参数可不填
            Social.ShareDelegate callback =
                delegate(Platform platform, int stCode, string errorMsg)
                {
                    if (stCode == Social.SUCCESS)
                    {
                        Debug.Log("分享成功");
                    }
                    else
                    {
                        Debug.Log("分享失败" + errorMsg);
                    };
                };



            
           


            //分享纹理
            //纹理 需要为 ARGB32 and RGB24 格式. 而且纹理的import settings/Advanced下的"Read/Write Enabled"需要勾选
            File.WriteAllBytes(Application.persistentDataPath + "/icon.png",texture.EncodeToPNG());
            //打开分享面版
            Social.OpenShareWithImagePath(platforms, "HelloWorld", Application.persistentDataPath + "/icon.png", callback);



        }
        if (GUI.Button(new Rect(150, 300, 500, 100), "授权"))
        {
            //接入QQ
            Social.SetQQAppIdAndAppKey("100424468", "c7394704798a158208a74ab60104f0ba");
            //授权
            Social.Authorize(Platform.QQ, delegate(Platform platform, int stCode, string usid, string token)
            {
                if (stCode == Social.SUCCESS)
                {
                    Debug.Log("授权成功" + "usid:" + usid + "token:" + token);
                }
                else
                {
                    Debug.Log("授权失败");
                }
            });
            
        }

        if (GUI.Button(new Rect(150, 500, 500, 100), "直接分享"))
        {
            //接入QQ
            Social.SetQQAppIdAndAppKey("100424468", "c7394704798a158208a74ab60104f0ba");

            Social.ShareDelegate callback =
                delegate(Platform platform, int stCode, string errorMsg)
                {
                    if (stCode == Social.SUCCESS)
                    {
                        Debug.Log("直接分享");
                    }
                    else
                    {
                        Debug.Log("直接分享" + errorMsg);
                    };
                };
            //截屏
            Application.CaptureScreenshot("Sceenshot.png");
            //分享
            Social.DirectShare(Platform.QQ, "Hello World", Application.persistentDataPath + "/Sceenshot.png", callback);
        }
    }


	
}
 
