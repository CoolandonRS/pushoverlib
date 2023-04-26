using System.Text.Json;
using System.Text.Json.Serialization;

namespace CoolandonRS.pushoverlib;

public class PushReceiptResult {
    [JsonInclude, JsonPropertyName("status")]
    public int Status { get; private set; }
    [JsonInclude, JsonPropertyName("acknowledged")]
    public bool Acknowledged { get; private set; }
    [JsonInclude, JsonPropertyName("acknowledged_at")]
    public long? AcknowledgedAt{ get; private set; }
    [JsonInclude, JsonPropertyName("acknowledged_by")]
    public string? AcknowledgedBy { get; private set; }
    [JsonInclude, JsonPropertyName("acknowledged_by_device")]
    public string? AcknowledgedByDevice { get; private set; }
    [JsonInclude, JsonPropertyName("last_delivered_at")]
    public long? DeliveredAt { get; private set; }
    [JsonInclude, JsonPropertyName("expired")]
    public bool Expired { get; private set; }
    [JsonInclude, JsonPropertyName("expires_at")]
    public long ExpiresAt { get; private set; }
    [JsonInclude, JsonPropertyName("request")]
    public string Request { get; private set; }

    public string ToJsonString() {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull });
    }

    public PushReceiptResult(JsonElement json) {
        try {
            Status = json.GetProperty("status").GetInt32();
            Acknowledged = json.GetProperty("acknowledged").GetInt32() == 1;
            AcknowledgedAt = Acknowledged ? json.GetProperty("acknowledged_at").GetInt64() : null;
            AcknowledgedBy = Acknowledged ? json.GetProperty("acknowledged_by").GetString()! : null;
            AcknowledgedByDevice = Acknowledged ? json.GetProperty("acknowledged_by_device").GetString()! : null;
            DeliveredAt = json.GetProperty("last_delivered_at").GetInt64();
            DeliveredAt = DeliveredAt == 0 ? null : DeliveredAt;
            Expired = json.GetProperty("expired").GetInt32() == 1;
            ExpiresAt = json.GetProperty("expires_at").GetInt64();
            Request = json.GetProperty("request").GetString()!;
            // CalledBack and CalledBackAt aren't implemented as callbacks aren't
        } catch (Exception e) when (e is JsonException or KeyNotFoundException) {
            throw new PushResponseException("Invalid JSON", e);
        }
    }
}