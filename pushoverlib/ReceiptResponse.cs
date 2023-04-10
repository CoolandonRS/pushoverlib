using System.Text.Json;

namespace pushoverlib;

public class PushReceiptResponse{
    public int Status { get; private set; }
    public bool Acknowledged { get; private set; }
    public long? AcknowledgedAt{ get; private set; }
    
    public PushReceiptResponse(JsonElement json){
        Status = json.GetProperty("status").GetInt32()!;
        Acknowledged = json.GetProperty("acknowledged").GetInt32()! == 1;
        AcknowledgedAt = json.GetProperty("acknowledged_at").GetInt64();
        AcknowledgedAt = AcknowledgedAt == 0 ? null : AcknowledgedAt;
        
        //TODO FINISH https://pushover.net/api/receipts
    }
}