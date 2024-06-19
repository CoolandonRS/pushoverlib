namespace CoolandonRS.pushoverlib; 
// /
public class PushData {
    // required
    public string? ApiToken { get; private set; }
    public string? UserToken { get; private set; }
    public string? Message { get; private set; }
    private bool set = false;
    // optional
    public PushPriority? Priority;
    public string? Title;
    public string? Device;
    public DateTimeOffset? Timestamp;
    public PushAttachment? Attachment;
    public bool? Html;
    public PushSound? Sound;
    public PushUrl? Url;
    public int? Retry;
    public int? Expire;


    public enum PushPriority {
        Badge = -2,
        NotificationCenter = -1,
        Notify = 0,
        TimeSensitive = 1,
        RequireAck = 2
    }
    
    internal PushData AddNeeded(string apiToken, string userToken, string msg) {
        if (set) throw new InvalidOperationException("Already set");
        set = true;
        this.ApiToken = apiToken;
        this.UserToken = userToken;
        this.Message = msg;
        return this;
    }

    /// <summary>
    /// Verifies that the given PushData is valid
    /// </summary>
    /// <exception cref="PushRequestException">If the PushData is not valid</exception>
    public void Verify() {
        if (Priority == PushPriority.RequireAck && (Retry is null || Expire is null)) throw new PushRequestException("RequireAck messages must have both retry and expire set");
        if (Retry < 30) throw new PushRequestException("Retry can not be lower then 30");
        if (Expire > 10800) throw new PushRequestException("Expire can not be higher then 10800");
    }

    public Dictionary<string, string> ToDict() {
        Verify();
        
        var dict = new Dictionary<string, string>();

        AddToDict("token", ApiToken, true);
        AddToDict("user", UserToken, true);
        AddToDict("message", Message, true);
        // non-base64 attachments are handled in PushCommunicator
        if (Attachment?.IsBase64 ?? false) {
            AddToDict("attachment_base64", Convert.ToBase64String(Attachment.Data));
            AddToDict("attachment_type", Attachment?.EvaluateType());
        }
        AddToDict("device", Device);
        AddBoolToDict("html", Html);
        if (Priority != null) AddToDict("priority", ((int) Priority).ToString());
        AddToDict("sound", Sound?.Evaluate());
        AddToDict("timestamp", Timestamp?.ToUnixTimeSeconds().ToString());
        AddToDict("title", Title);
        AddToDict("url", Url?.Url);
        AddToDict("url_title", Url?.Title);
        if (Priority != PushPriority.RequireAck) return dict;
        AddToDict("retry", Retry.ToString(), true);
        AddToDict("expire", Expire.ToString(), true);
        return dict;
        
        void AddToDict(string key, string? val, bool req = false) {
            if (req && val == null) throw new InvalidOperationException("Required Field " + key + " not set");
            if (val == null) return;
            dict.Add(key, val);
        }
        void AddBoolToDict(string key, bool? val) {
            if (val == null) return;
            dict.Add(key, val.Value ? "1" : "0");
        }
    }

    public PushData() {
        
    }
}