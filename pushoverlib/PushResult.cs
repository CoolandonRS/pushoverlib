using System.Text.Json;
using System.Text.Json.Serialization;

namespace CoolandonRS.pushoverlib; 

public class PushResult {
    [JsonInclude, JsonPropertyName("receipt")]
    public string? Receipt { get; private set; }

    [JsonInclude, JsonPropertyName("errors")]
    public string[]? Errors { get; private set; }

    [JsonInclude, JsonPropertyName("status")]
    public int Status { get; private set; }

    [JsonInclude, JsonPropertyName("request")]
    public string Request { get; private set; }

    public bool IsSuccess() {
        return Status == 1 && Errors == null;
    }

    public bool HasErrors() {
        return Status != 1 || Errors != null;
    }

    public bool HasReceipt() {
        return Receipt != null;
    }

    public string ToJsonString() {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull });
    }

    public PushResult(JsonElement json) {
        try {
            Status = json.GetProperty("status").GetInt32();
            Request = json.GetProperty("request").GetString()!;
            try {
                var jsonArr = json.GetProperty("errors");
                var len = jsonArr.GetArrayLength();
                var arr = new string[len];
                for (var i = 0; i < len; i++) {
                    arr[i] = jsonArr[i].GetString()!;
                }
                Errors = arr;
            } catch (KeyNotFoundException) {
                Errors = null;
            }
            try {
                Receipt = json.GetProperty("receipt").GetString()!;
            } catch (KeyNotFoundException) {
                Receipt = null;
            }
        } catch (Exception e) {
            throw new PushResponseException("Invalid JSON", e);
        }
    }
}