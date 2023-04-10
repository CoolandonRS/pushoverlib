using System.Text.Json;

namespace CoolandonRS.pushoverlib; 

public class PushResult {
    public int Status { get; private set; }
    public string Request { get; private set; }
    public string[] Errors { get; private set; }
    public string? Receipt { get; private set; }

    public bool IsSuccess() {
        return Status == 1 && Errors.Length == 0;
    }

    public bool HasErrors() {
        return Status != 1 || Errors.Length > 0;
    }

    public bool HasReceipt() {
        return Receipt != null;
    }

    public string AssertReceipt() {
        if (Receipt == null) throw new NullReferenceException("Receipt is null");
        return Receipt!;
    }
    
    public PushResult(JsonElement json) {
        try {
            Status = json.GetProperty("status").GetInt32();
            Request = json.GetProperty("request").GetString()!;
            try {
                var arr = json.GetProperty("errors");
                var len = arr.GetArrayLength();
                var list = new List<string>(1);
                for (int i = 0; i < len - 1; i++) {
                    list.Add(arr[i].GetString()!);
                }
                Errors = list.ToArray();
            } catch (KeyNotFoundException) {
                Errors = Array.Empty<string>();
            }
            try {
                Receipt = json.GetProperty("receipt").GetString()!;
            } catch (KeyNotFoundException) {
                Receipt = null;
            }
        } catch (Exception e) {
            throw new InvalidOperationException("Invalid JSON", e);
        }
    }
}