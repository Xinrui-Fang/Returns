//
//  UMSocialCObject.m
//  UmengGame
//
//  Created by yeahugo on 14-6-26.
//
//

#import "UMSocialCObject.h"

@implementation UMSocialCObject

-(id)initWithCallback:(ShareHandler)callBack
{
    self = [super init];
    if (self) {
        handler = callBack;
    }
    return self;
}

-(void)didFinishGetUMSocialDataInViewController:(UMSocialResponseEntity *)response
{
    int stCode = response.responseCode;
    int platform = -1;
    if ([response.data allKeys].count > 0) {
        NSString *platformName = [[response.data allKeys] objectAtIndex:0];
        if ([platformName isEqualToString:UMShareToSina]) {
            platform = SINA;
        } else if ([platformName isEqualToString:UMShareToTencent]) {
            platform = TENCENT_WEIBO;
        } else if ([platformName isEqualToString:UMShareToQzone]){
            platform = QZONE;
        } else if ([platformName isEqualToString:UMShareToQQ]){
            platform = QQ;
        } else if ([platformName isEqualToString:UMShareToRenren]){
            platform = RENREN;
        } else if ([platformName isEqualToString:UMShareToDouban]){
            platform = DOUBAN;
        } else if ([platformName isEqualToString:UMShareToWechatSession]){
            platform = WEIXIN;
        } else if ([platformName isEqualToString:UMShareToWechatTimeline]){
            platform = WEIXIN_CIRCLE;
        } else if ([platformName isEqualToString:UMShareToLWSession]){
            platform = LAIWANG;
        } else if ([platformName isEqualToString:UMShareToLWTimeline]){
            platform = LAIWANG_CIRCLE;
        } else if ([platformName isEqualToString:UMShareToYXSession]){
            platform = YIXIN;
        } else if ([platformName isEqualToString:UMShareToYXTimeline]){
            platform = YIXIN_CIRCLE;
        } else if ([platformName isEqualToString:UMShareToSms]){
            platform = SMS;
        } else if ([platformName isEqualToString:UMShareToEmail]){
            platform = EMAIL;
        } else if ([platformName isEqualToString:UMShareToFacebook]){
            platform = FACEBOOK;
        } else if ([platformName isEqualToString:UMShareToTwitter]){
            platform = TWITTER;
        }
    }
    if (handler) {
        handler(platform,stCode,response.message.UTF8String);
    }
}

@end
