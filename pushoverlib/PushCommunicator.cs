using System.Text.Json;

namespace pushoverlib;

internal static class PushCommunicator{
    private static readonly HttpClient client = new HttpClient();

    public static async Task<PushResult> ConfirmSendAsync(PushData data){
        var result = await SendAsync(data);
        if (!result.IsSuccess())
            throw new PushRequestException("Failed to send with errors: " + string.Join("; ", result.Errors));
        return result;
    }

    public static async Task<PushResult> SendAsync(PushData data){
        HttpContent content;
        if (!data.Attachment?.IsBase64() ?? true){
            content = new FormUrlEncodedContent(data.ToDict());
        }
        else{
            var form = new MultipartFormDataContent();
            foreach (var kvp in data.ToDict()){
                form.Add(new StringContent(kvp.Value), kvp.Key);
            }

            form.Add(new ByteArrayContent(data.Attachment.Data));
            content = form;
        }

        var httpResponse = await client.PostAsync("https://api.pushover.net/1/messages.json", content);
        var statusCode = (int)httpResponse.StatusCode;
        // who let this python syntax into my c#
        if ((statusCode / 100) is not (2 or 4)) throw new PushException("Pushover failed to send JSON");
        return new PushResult(JsonSerializer.SerializeToElement(await httpResponse.Content.ReadAsStringAsync()));
    }

    public static async Task<PushReceiptResult> GetReceiptResult(string token, string receipt){
        var httpResponse = await client.GetAsync("https://api.pushover.net/1/receipts/" + receipt + ".json?token=" + token);
        var statusCode = (int)httpResponse.StatusCode;
        if ((statusCode / 100) != 2) throw new PushException("Pushover failed to get Receipt JSON");
        
        return new PushReceiptResult(JsonSerializer.SerializeToElement(await httpResponse.Content.ReadAsStringAsync()));
    }

    public static async Task CancelReceipt(string token, string receipt) {
        var httpResponse = await client.PostAsync("https://api.pushover.net/1/receipts/" + receipt + "/cancel.json", new FormUrlEncodedContent(new Dictionary<string, string>() { { "token", token } }));
        var statusCode = (int)httpResponse.StatusCode;
        if ((statusCode / 100) != 2) throw new PushException("Pushover sent failure status");
    }
}