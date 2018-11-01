#import <StoreKit/StoreKit.h>
#import "Flurry.h"

NSString* strToNSString(const char* str)
{
	if (!str)
		return [NSString stringWithUTF8String: ""];

	return [NSString stringWithUTF8String: str];
}

NSMutableDictionary* strToDict(const char* str)
{
    if (!str)
        return nil;
    
	NSMutableDictionary* dict = [[NSMutableDictionary alloc] init];

	NSArray* array = [strToNSString(str) componentsSeparatedByString: @"|;"];
	for (int i = 0; i < [array count]; i++)
	{
		NSArray* kv = [[array objectAtIndex: i] componentsSeparatedByString: @"|:"];
		[dict setObject:[kv objectAtIndex: 0] forKey:[kv objectAtIndex: 1]];
	}

	return dict;
}

@interface PaymentObserver : NSObject<SKPaymentTransactionObserver>
{
}
+ (PaymentObserver*)sharedInstance;
-(void)initObserver;
@end

static PaymentObserver *sharedPaymentObserver = nil;

@implementation PaymentObserver

+(PaymentObserver*)sharedInstance
{
    if (sharedPaymentObserver == nil)
        sharedPaymentObserver = [[super allocWithZone:NULL] init];
    return sharedPaymentObserver;
}

- (void)initObserver
{
    [[SKPaymentQueue defaultQueue] addTransactionObserver:self];
}

- (void)paymentQueue:(SKPaymentQueue *)queue updatedTransactions:(NSArray<SKPaymentTransaction *> *)transactions
{
    for (SKPaymentTransaction *transaction in transactions) {
        switch (transaction.transactionState) {
            case SKPaymentTransactionStatePurchased:
                NSLog(@"Payment went through successfully!");
                
                [Flurry logPaymentTransaction:transaction statusCallback:^(FlurryTransactionRecordStatus status) {
                    
                }];
                
                //[[SKPaymentQueue defaultQueue] finishTransaction:transaction];
                break;
            case SKPaymentTransactionStateFailed:
//                NSLog(@"Payment was a failure.");
//                [Flurry logPaymentTransaction:transaction statusCallback:^(FlurryTransactionRecordStatus status) {
//                }];
                //[[SKPaymentQueue defaultQueue] finishTransaction:transaction];
                break;
                
            default:
                // NSLog(@"Not a failure nor a success! => %ld", transaction.transactionState);
                break;
        }
    }
}

@end

#if defined (__cplusplus)
extern "C" {
#endif
	void StartSessionImpl(const char* apiKey)
	{
		[Flurry startSession: strToNSString(apiKey)];
	}

	void SetDebugLogEnabledImpl(bool isEnabled)
	{
		[Flurry setDebugLogEnabled: isEnabled];
	}
	
	void SetUserIdImpl(const char* userId)
	{
		[Flurry setUserID: strToNSString(userId)];
	}

	void LogEventImplA(const char* eventName)
	{
		[Flurry logEvent: strToNSString(eventName)];
	}

	void LogEventImplB(const char* eventName, const char* parameters)
	{
		[Flurry logEvent: strToNSString(eventName) withParameters: strToDict(parameters)];
	}
    
    void StartPaymentObserverImpl()
    {
        [[PaymentObserver sharedInstance] initObserver];
    }
    
#if defined (__cplusplus)
}
#endif
