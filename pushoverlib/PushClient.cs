namespace pushoverlib; 

public class PushClient {
    private string apiToken;
    private string userToken;

    public async Task<PushResult> SendAsync(string msg, PushData data) {
        return await PushCommunicator.SendAsync(data.AddNeeded(apiToken, userToken, msg));
    }

    public PushClient(string apiToken, string userToken) {
        this.apiToken = apiToken;
        this.userToken = userToken;
    }
}