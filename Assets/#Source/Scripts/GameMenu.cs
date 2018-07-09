using UnityEngine;
using System.Collections;
using System.IO;

public class GameMenu : MonoBehaviour {

	public GameObject gameOverPanel;
	public Texture2D texture;
	public void Start()
	{
        if (Application.platform == RuntimePlatform.Android) 
        {
            //设置Umeng Appkey
            Social.SetAppKey("559d34a567e58e808000167d");
        }

	}

    public void StartGame() 
    {
        Application.LoadLevel("Main");
    }

    public void EndGame() 
    {

        Application.Quit();
    }

	public void ShowGameOver()
	{
		gameOverPanel.SetActive (true);
	}

	public void LoadStartScene()
	{
		Application.LoadLevel("Start");
	}


	//分享游戏
	public void ShaderGame()
	{
		//按需设置您需要的平台
		Platform[] platforms = { Platform.QQ, Platform.QZONE,Platform.TENCENT_WEIBO,Platform.SINA};
		
		
		//接入instagram
		Social.OpenInstagram();
		//接入QQ
		Social.SetQQAppIdAndAppKey("100424468", "c7394704798a158208a74ab60104f0ba");
		//接入来往
		//Social.SetLaiwangAppInfo("laiwangd497e70d4", "d497e70d4c3e4efeab1381476bac4c5e", "");

		
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
		
		Social.SetTargetUrl ("http://www.cnblogs.com/plateFace/");
		//纹理 需要为 ARGB32 and RGB24 格式. 而且纹理的import settings/Advanced下的"Read/Write Enabled"需要勾选
		File.WriteAllBytes(Application.persistentDataPath + "/icon.png",texture.EncodeToPNG());
		//打开分享面版
		Social.OpenShareWithImagePath(platforms, "我在玩跑吧盒子男! =.= 我挂了", Application.persistentDataPath + "/icon.png", callback);

	}

}
