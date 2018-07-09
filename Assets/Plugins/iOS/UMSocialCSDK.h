//
//  UMSocialCSDK.h
//  UmengGame
//
//  Created by yeahugo on 14-6-26.
//
//

#ifndef __UmengGame__UMSocialCSDK__
#define __UmengGame__UMSocialCSDK__


extern "C"
{
    
typedef void (*AuthHandler)(int platform, int stCode,const char* usid, const char *token);
typedef void (*ShareHandler)(int platform, int stCode, const char * errorMsg);

//设置SDK的appkey
void setAppKey(const char* appKey);

//初始化sdk
void initUnitySDK(const char *sdkType, const char *version);

//设置分享的url
void setTargetUrl(const char * targetUrl);

//授权某社交平台
void authorize(int platform, AuthHandler callback);

//解除某平台授权
void deleteAuthorization(int platform, AuthHandler callback);

//是否已经授权某平台
bool isAuthorized(int platform);

//打开分享面板
void openShareWithImagePath(int platform[], int platformNum, const char* text, const char* imagePath,ShareHandler callback);

//直接分享到各个社交平台
void directShare(const char* text, const char* imagePath, int platform, ShareHandler callback);

//打开SDK的log输出
void openLog(int flag);

//设置SSO授权
void openSSOAuthorization(int platform, const char * redirectURL);
    
//分别设置各个分享平台内容
void setPlatformShareContent(int platform, const char* text,
                                        const char* imagePath, const char* title,
                                        const char* targetUrl);

//设置QQ互联appid，appkey
void setQQAppIdAndAppKey(const char *appId,const char *appKey);

//设置微信appid，secret
void setWechatAppId(const char *appId,const char *appSecret);

//设置来往appid，appkey
void setLaiwangAppInfo(const char *appId, const char *appKey, const char * appName);

//设置易信appid
void setYiXinAppKey(const char *appKey);

//设置Facebook的appid
void setFacebookAppId(const char *appId);

//打开Twitter的开关
void openTwitter();

//打开Instagram的开关
void openInstagram();
    
}
#endif /* defined(__UmengGame__UMSocialCSDK__) */
