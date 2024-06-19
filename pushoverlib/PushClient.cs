namespace CoolandonRS.pushoverlib; 

public class PushClient {
    private string apiToken;
    private string userToken;

    public async Task<PushResult> Send(string msg, PushData? data = null) {
        return await PushCommunicator.SendAsync((data ?? new PushData()).AddNeeded(apiToken, userToken, msg));
    }

    public async Task<PushReceiptResult> GetReceiptResult(PushResult result) {
        return await PushCommunicator.GetReceiptResult(apiToken, result.Receipt!);
    }
    
    public async Task CancelReceipt(PushResult result) {
        await PushCommunicator.CancelReceipt(apiToken, result.Receipt!);
    }

    public PushClient(string apiToken, string userToken) {
        this.apiToken = apiToken;
        this.userToken = userToken;
    }
}