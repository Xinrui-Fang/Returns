//
//  UMSocialCObject.h
//  UmengGame
//
//  Created by yeahugo on 14-6-26.
//
//

#import <Foundation/Foundation.h>
#import "UMSocial.h"

/// 各个分享平台的枚举
enum Platform {
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
	/// facebook
	FACEBOOK = 11,
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

typedef void (*ShareHandler)(int platform, int stCode, const char * errorMsg);

@interface UMSocialCObject : NSObject<UMSocialUIDelegate>
{
    ShareHandler handler;
}

-(id)initWithCallback:(ShareHandler)callBack;

@end
