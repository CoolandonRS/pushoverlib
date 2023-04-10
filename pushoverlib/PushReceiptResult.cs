using System.Text.Json;

namespace pushoverlib;

public class PushReceiptResult {
    public int Status { get; private set; }
    public bool Acknowledged { get; private set; }
    public long? AcknowledgedAt{ get; private set; }
    public string? AcknowledgedBy { get; private set; }
    public string? AcknowledgedByDevice { get; private set; }
    public long? DeliveredAt { get; private set; }
    public bool Expired { get; private set; }
    public long ExpiresAt { get; private set; }

    public PushReceiptResult(JsonElement json){
        Status = json.GetProperty("status").GetInt32();
        Acknowledged = json.GetProperty("acknowledged").GetInt32() == 1;
        AcknowledgedAt =  Acknowledged ? json.GetProperty("acknowledged_at").GetInt64() : null;
        AcknowledgedBy = Acknowledged ? json.GetProperty("acknowledged_by").GetString()! : null;
        AcknowledgedByDevice = Acknowledged ? json.GetProperty("acknowledged_by_device").GetString()! : null;
        DeliveredAt = json.GetProperty("acknowledged_at").GetInt64();
        DeliveredAt = DeliveredAt == 0 ? null : DeliveredAt;
        Expired = json.GetProperty("expired").GetInt32() == 1;
        ExpiresAt = json.GetProperty("expires_at").GetInt64();

        // CalledBack and CalledBackAt aren't implemented as callbacks aren't
    }
}