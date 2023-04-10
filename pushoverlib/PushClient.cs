namespace CoolandonRS.pushoverlib; 

public class PushClient {
    private string apiToken;
    private string userToken;

    public async Task<PushResult> SendAsync(string msg, PushData? data = null) {
        return await PushCommunicator.SendAsync((data ?? new PushDataBuilder().Build()).AddNeeded(apiToken, userToken, msg));
    }

    public PushClient(string apiToken, string userToken) {
        this.apiToken = apiToken;
        this.userToken = userToken;
    }
}