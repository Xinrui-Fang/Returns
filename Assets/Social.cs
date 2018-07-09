using UnityEngine;
using System.Runtime.InteropServices;
using System;




//分享平台枚举

public enum Platform : int
{
    /// 新浪微博
    SINA = 0,
    /// 微信
    WEIXIN = 1,
    /// 微信朋友圈
    WEIXIN_CIRCLE = 2,
    /// QQ
    QQ = 3,
    /// QQ空间
    QZONE = 4,
    /// 人人网
    RENREN = 5,
    /// 豆瓣
    DOUBAN = 6,
    /// 来往
    LAIWANG = 7,
    /// 来往动态
    LAIWANG_CIRCLE = 8,
    /// 易信
    YIXIN = 9,
    /// 易信朋友圈
    YIXIN_CIRCLE = 10,

    /// twitter
    TWITTER = 12,
    /// instagram
    INSTAGRAM = 13,
    /// 短信
    SMS = 14,
    /// 邮件
    EMAIL = 15,
    /// 腾讯微博
    TENCENT_WEIBO = 16
};


public class Social
{
    
    //成功状态码 
    //用于 授权回调 和 分享回调 的是非成功的判断
    public const int SUCCESS = 200;

    //授权回调
    //注意 android 分享失败 没有 errorMsg
    public delegate void AuthDelegate(Platform platform, int stCode, string usid, string token);

    //分享回调
    //注意 android 分享失败 没有 errorMsg
    public delegate void ShareDelegate(Platform platform, int stCode, string errorMsg);


    //授权某社交平台
    //platform 平台名 callback 授权成功完成
    public static void Authorize(Platform platform, AuthDelegate callback=null)
    {
        
        if (string.IsNullOrEmpty(appKey))
        {
            Debug.LogError("请设置appkey");
            return;
        }
            

#if UNITY_ANDROID
		try 
		{

	        SetPlatforms(new Platform[] { platform });
	        Run(delegate
	        {
	            var androidAuthListener = new AndroidAuthListener(callback);
	            UMSocialSDK.CallStatic("authorize", (int)platform, androidAuthListener);
        	});
		}
		catch(AndroidJavaException exp)
		{
			Debug.LogError(exp.Message);
		}

#elif UNITY_IPHONE
        authDelegate = callback;
        authorize((int)platform,AuthCallback);
#endif
    }
    //解除某平台授权
    //platform 平台名 callback 解除完成回调
    public static void DeleteAuthorization(Platform platform, AuthDelegate callback=null)
    {
        if (string.IsNullOrEmpty(appKey))
        {
            Debug.LogError("请设置appkey");
            return;
        }
#if UNITY_ANDROID
		try 
		{
	        Run(delegate
	       {
	           var androidAuthListener = new AndroidAuthListener(callback);
	           UMSocialSDK.CallStatic("deleteAuthorization", (int)platform, androidAuthListener);
	       }
	       );
		}
		catch(AndroidJavaException exp)
		{
			Debug.LogError(exp.Message);
		}

#elif UNITY_IPHONE
        authDelegate = callback;
        deleteAuthorization((int)platform,AuthCallback);
#endif

    }



    //打开分享面板
    //platforms 需要分享的平台数组 ,text 分享的文字, imagePath 分享的照片文件路径, callback 分享成功或失败的回调
    //imagePath可以为url 但是url图片必须以http://或者https://开头
    //imagePath如果为本地文件 只支持 Application.persistentDataPath下的文件
    //例如 Application.persistentDataPath + "/" +"你的文件名"
    //如果想分享 Assets/Resouces的下的 icon.png 请前使用 Resources.Load() 和 FileStream 写到 Application.persistentDataPath下
    public static void OpenShareWithImagePath(Platform[] platforms, string text, string imagePath, ShareDelegate callback=null)
    {
        if (string.IsNullOrEmpty(appKey))
        {
            Debug.LogError("请设置appkey");
            return;
        }

        if(platforms==null)
        {
            Debug.LogError("平台不能为空");
            return;
        }
        //var _platforms = platforms ?? Enum.GetValues(typeof(Platform)) as Platform[];
        var length = platforms.Length;
        var platformsInt = new int[length];
        for (int i = 0; i < length; i++)
        {
            platformsInt[i] = (int)platforms[i];

        }

#if UNITY_ANDROID
		try 
		{

        	Run(delegate
	        {
	            var androidShareListener = new AndroidShareListener(callback);
	            UMSocialSDK.CallStatic("openShareWithImagePath", platformsInt, text, imagePath, androidShareListener);
	        });

		}
		catch(AndroidJavaException exp)
		{
			Debug.LogError(exp.Message);
		}

#elif UNITY_IPHONE
        shareDelegate = callback;
        openShareWithImagePath(platformsInt, length,text,imagePath, ShareCallback);
#endif

    }

    //直接分享到各个社交平台
    //platform 平台名，text 分享的文字，imagePath 分享的照片文件路径，callback 分享成功或失败的回调

    public static void DirectShare(Platform platform, string text, string imagePath, ShareDelegate callback=null)
    {
        
        if (string.IsNullOrEmpty(appKey))
        {
            Debug.LogError("请设置appkey");
            return;
        }

#if UNITY_ANDROID
		try 
		{

	        SetPlatforms(new Platform[] { platform });

	        Run(delegate
	        {
	            var androidShareListener = new AndroidShareListener(callback);
	            UMSocialSDK.CallStatic("directShare", text, imagePath, (int)platform, androidShareListener);
	        });
		}

		catch(AndroidJavaException exp)
		{
			Debug.LogError(exp.Message);
		}


#elif UNITY_IPHONE
        shareDelegate = callback;
        directShare( text, imagePath, (int)platform, ShareCallback);
#endif

    }


    //设置SDK的appkey
    public static void SetAppKey(string appKey)
    {
        if(!string.IsNullOrEmpty(appKey))
        {
            Social.appKey = appKey;
#if UNITY_ANDROID

            UMSocialSDK.CallStatic("setAppKey", appKey);
#elif UNITY_IPHONE

        setAppKey(appKey);
        initUnitySDK("Unity", "1.1");
            
#endif
        }
    }

    //设置用户点击一条图文分享时用户跳转到的目标页面, 一般为app主页或者下载页面
    public static void SetTargetUrl(string targetUrl)
    {


#if UNITY_ANDROID

        UMSocialSDK.CallStatic("setTargetUrl", targetUrl);
#elif UNITY_IPHONE

         setTargetUrl(targetUrl);
#endif

    }
    //是否已经授权某平台
    //platform 平台名
    public static bool IsAuthorized(Platform platform)
    {
        if (string.IsNullOrEmpty(appKey))
        {
            Debug.LogError("请设置appkey");
            return false;
        }

#if UNITY_ANDROID

        return UMSocialSDK.CallStatic<bool>("isAuthorized", (int)platform);
#elif UNITY_IPHONE

        return isAuthorized((int)platform);
#else 
		return false;
#endif
    }


    //打开SDK的log输出
    public static void OpenLog(bool isEnabled)
    {

#if UNITY_ANDROID

        UMSocialSDK.CallStatic("openLog", isEnabled);
#elif UNITY_IPHONE

        openLog( isEnabled?1:0);
#endif

    }

    //打开Twitter的开关
    public static void OpenTwitter()
    {

#if UNITY_ANDROID

        
#elif UNITY_IPHONE

        openTwitter();
#endif

    }

    //设置QQ互联appid，appkey
    public static void SetQQAppIdAndAppKey(string appId, string appKey)
    {

#if UNITY_ANDROID
		try 
		{	

        	UMSocialSDK.CallStatic("setQQAppIdAndAppKey", appId, appKey);
		}
		catch(AndroidJavaException exp)
		{
			Debug.LogError(exp.Message);
		}

#elif UNITY_IPHONE

        setQQAppIdAndAppKey(appId,appKey);
#endif

    }

    //设置来往appid，appkey 
    //appName 应用名称
    public static void SetLaiwangAppInfo(string appId, string appKey, string appName)
    {

#if UNITY_ANDROID
		try 
		{	
        	UMSocialSDK.CallStatic("setLaiwangAppInfo", appId, appKey, appName);
		}
		catch(AndroidJavaException exp)
		{
			Debug.LogError(exp.Message);
		}
#elif UNITY_IPHONE

        setLaiwangAppInfo(appId,appKey,appName);
#endif

    }



    //设置易信appid
    public static void SetYiXinAppKey(string appId)
    {

#if UNITY_ANDROID
		try 
		{
        	UMSocialSDK.CallStatic("setYiXinAppKey", appId);
		}
		catch(AndroidJavaException exp)
		{
			Debug.LogError(exp.Message);
		}
#elif UNITY_IPHONE

        setYiXinAppKey(appId);
#endif

    }




    //打开Instagram的开关
    public static void OpenInstagram()
    {

#if UNITY_ANDROID

        
#elif UNITY_IPHONE

        openInstagram();
#endif

    }


    public static void SetWechatAppIdAndSecret(String appId, String secret)
    {
#if UNITY_ANDROID

		try 
		{

        	UMSocialSDK.CallStatic("setWechatAppIdAndSecret", appId, secret);
		}
		catch(AndroidJavaException exp)
		{
			Debug.LogError(exp.Message);
		}
#elif UNITY_IPHONE

		setWechatAppId(appId,secret);
#endif

    }

	[Obsolete]
    public void SetRenrenAppInfo(String appid, String appKey, String appSecret)
	{


    }





	public static void SetPlatformShareContent(Platform platform, String text, String imagePath,
            String title = null, String targetUrl = null)
    {
#if UNITY_ANDROID
		try 
		{
        	UMSocialSDK.CallStatic("setPlatformShareContent", (int)platform, text, imagePath, title, targetUrl);
		}
		catch(AndroidJavaException exp)
		{
			Debug.LogError(exp.Message);
		}
#elif UNITY_IPHONE

        setPlatformShareContent((int)platform, text, imagePath,title, targetUrl);
#endif


    }


	public static void SupportSSO(Platform platform,string redirectURL)
    {
#if UNITY_ANDROID
		try 
		{
			UMSocialSDK.CallStatic("supportSso",(int)platform,redirectURL);
		}
		catch(AndroidJavaException exp)
		{
			Debug.LogError(exp.Message);
		}
#elif UNITY_IPHONE

		openSSOAuthorization((int)platform,redirectURL);
#endif
    }



#if UNITY_ANDROID

    //设置SDK支持的平台
	public static void SetPlatforms(Platform[] platforms)
    {
        if (string.IsNullOrEmpty(appKey))
        {
            Debug.LogError("请设置appkey");
            return;
        }
        var length = platforms.Length;
        var platformsInt = new int[length];
        for (int i = 0; i < length; i++)
        {
            platformsInt[i] = (int)platforms[i];

        }

        Run(delegate
        {

            UMSocialSDK.CallStatic("setPlatforms", platformsInt);
        });


    }



   


#endif













    //以下代码是内部实现
    //请勿修改

#if UNITY_ANDROID


    delegate void Action();
    static void Run(Action action)
    {
        activity.Call("runOnUiThread", new AndroidJavaRunnable(action));
    }


    class AndroidAuthListener : AndroidJavaProxy
    {


        public AndroidAuthListener(AuthDelegate Delegate)
            : base("com.umeng.socialsdk.UMSocialSDK$AuthListener")
        {
            this.authDelegate = Delegate;
        }

        AuthDelegate authDelegate = null;
        public void onAuth(int platform, int stCode, string usid, string token)
        {
            authDelegate((Platform)platform, stCode, usid, token);
        }
    }

    class AndroidShareListener : AndroidJavaProxy
    {

        ShareDelegate shareDelegate = null;
        public AndroidShareListener(ShareDelegate Delegate)
            : base("com.umeng.socialsdk.UMSocialSDK$ShareListener")
        {
            this.shareDelegate = Delegate;
        }
        public void onShare(int platform, int stCode, string errorMsg)
        {
            shareDelegate((Platform)platform, stCode, errorMsg);
        }
    }


    


    static AndroidJavaClass UMSocialSDK = new AndroidJavaClass("com.umeng.socialsdk.UMSocialSDK");


    static AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");


#endif

    static string appKey = null;

    static AuthDelegate authDelegate = null;

    static ShareDelegate shareDelegate = null;

    [AOT.MonoPInvokeCallback(typeof(AuthDelegate))]
    static void AuthCallback(Platform platform, int stCode, string usid, string token)
    {
        if (authDelegate!=null)
            authDelegate(platform, stCode, usid, token);
    }


    [AOT.MonoPInvokeCallback(typeof(ShareDelegate))]
    static void ShareCallback(Platform platform, int stCode, string errorMsg)
    {
        if (shareDelegate!=null)
            shareDelegate(platform, stCode, errorMsg);
    }









    [DllImport("__Internal")]
    static extern void setAppKey(string appKey);

    [DllImport("__Internal")]
    static extern void initUnitySDK(string sdkType, string version);



    [DllImport("__Internal")]
    static extern void setTargetUrl(string targetUrl);

    [DllImport("__Internal")]
    static extern void authorize(int platform, AuthDelegate callback);

    [DllImport("__Internal")]
    static extern void deleteAuthorization(int platform, AuthDelegate callback);

    [DllImport("__Internal")]
    static extern bool isAuthorized(int platform);

    [DllImport("__Internal")]
    static extern void openShareWithImagePath(int[] platform, int platformNum, string text, string imagePath, ShareDelegate callback);

    [DllImport("__Internal")]
    static extern void directShare(string text, string imagePath, int platform, ShareDelegate callback);

    [DllImport("__Internal")]
    static extern void openLog(int flag);

    [DllImport("__Internal")]
    static extern void setQQAppIdAndAppKey(string appId, string appKey);

  

    [DllImport("__Internal")]
    static extern void setLaiwangAppInfo(string appId, string appKey, string appName);

    [DllImport("__Internal")]
    static extern void setYiXinAppKey(string appKey);



    [DllImport("__Internal")]
    static extern void openTwitter();

    [DllImport("__Internal")]
    static extern void openInstagram();
    [DllImport("__Internal")]
	static extern void setPlatformShareContent(int platformCode, String text, String imagePath,
	                                           String title, String targetUrl);	

    [DllImport("__Internal")]
	static extern  void openSSOAuthorization(int platform,string redirectURL);
   
    [DllImport("__Internal")]
	static extern void setWechatAppId(String appId, String secret);
	
}


