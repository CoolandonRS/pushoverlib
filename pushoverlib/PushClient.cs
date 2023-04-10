namespace pushoverlib; 

public class PushClient {
    private string apiToken;
    private string userToken;

    public async Task<PushResult> SendAsync(string msg, PushData data) {
        return await PushCommunicator.SendAsync(data.AddNeeded(apiToken, userToken, msg));
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