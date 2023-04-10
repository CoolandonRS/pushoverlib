namespace pushoverlib; 

public class PushApp {
    private string apiToken;

    public async Task<PushResult> Send(string userToken, string msg, PushData data) {
        return await PushCommunicator.SendAsync(data.AddNeeded(apiToken, userToken, msg));
    }

    public PushClient GetClient(string userToken) {
        return new PushClient(apiToken, userToken);
    }
    
    public async Task<PushReceiptResult> GetReceiptResult(PushResult result){
        return await PushCommunicator.GetReceiptResult(apiToken, result.Receipt!);
    }
    
    public async Task CancelReceipt(PushResult result){
        await PushCommunicator.CancelReceipt(apiToken, result.Receipt!);
    }

    public PushApp(string apiToken) {
        this.apiToken = apiToken;
    }
}