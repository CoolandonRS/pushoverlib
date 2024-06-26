﻿using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text.Json;

namespace CoolandonRS.pushoverlib;

internal static class PushCommunicator {
    private static readonly HttpClient client = new HttpClient();
    private static string url = "https://api.pushover.net/";
    /// <summary>
    /// USED FOR UNIT TESTS! DON'T RUN ME! I WILL BREAK EVERYTHING :)
    /// </summary>
    internal static void TestMode(int port) {
        url = "http://127.0.0.1:" + port + "/";
        Console.WriteLine("AAAAAAAAAAAA");
    }

    internal static async Task<PushResult> ConfirmSendAsync(PushData data) {
        var result = await SendAsync(data);
        if (!result.IsSuccess()) throw new PushRequestException("Failed to send with errors: " + string.Join("; ", result.Errors));
        return result;
    }

    internal static async Task<PushResult> SendAsync(PushData data) {
        HttpContent content;
        if (data.Attachment?.IsBase64 ?? true) {
            content = new FormUrlEncodedContent(data.ToDict());
        } else {
            var form = new MultipartFormDataContent();
            foreach (var kvp in data.ToDict()) {
                form.Add(new StringContent(kvp.Value), kvp.Key);
            }

            var attachmentContent = new ByteArrayContent(data.Attachment.Data);
            attachmentContent.Headers.ContentType = MediaTypeHeaderValue.Parse(data.Attachment.EvaluateType());
            form.Add(attachmentContent, "attachment", data.Attachment.Filename);
            
            content = form;
        }
        await File.WriteAllBytesAsync(@"C:\Users\bstevic\RiderProjects\pushoverlib\pushoverlib-practical-tests\request.txt", await content.ReadAsByteArrayAsync());
        Console.WriteLine(await content.ReadAsStringAsync());

        var httpResponse = await client.PostAsync(url + "1/messages.json", content);
        var statusCode = (int)httpResponse.StatusCode;
        // who let this python syntax into my c#
        if ((statusCode / 100) is not (2 or 4)) throw new PushServerException("Pushover failed to send JSON");
        return new PushResult(JsonDocument.Parse(await httpResponse.Content.ReadAsStringAsync()).RootElement);
    }

    internal static async Task<PushReceiptResult> GetReceiptResult(string token, string receipt) {
        var httpResponse = await client.GetAsync(url + "1/receipts/" + receipt + ".json?token=" + token);
        var statusCode = (int)httpResponse.StatusCode;
        if ((statusCode / 100) != 2) throw new PushServerException("Pushover failed to get Receipt JSON");
        return new PushReceiptResult(JsonDocument.Parse(await httpResponse.Content.ReadAsStringAsync()).RootElement);
    }

    internal static async Task CancelReceipt(string token, string receipt) {
        var httpResponse = await client.PostAsync(url + "1/receipts/" + receipt + "/cancel.json", new FormUrlEncodedContent(new Dictionary<string, string>() { { "token", token } }));
        var statusCode = (int)httpResponse.StatusCode;
        if ((statusCode / 100) != 2) throw new PushServerException("Pushover sent failure status");
    }
}
