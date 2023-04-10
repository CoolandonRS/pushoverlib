namespace CoolandonRS.pushoverlib; 

public class PushDataBuilder {
    private PushData.PushPriority? priority = null; 
    private string? title = null;
    private string? device = null;
    private long? timestamp = null;
    private PushAttachment? attachment = null;
    private bool? html = null;
    private PushSound? sound = null;
    private PushUrl? url = null;
    public PushDataBuilder Priority(PushData.PushPriority? priority) { this.priority = priority; return this; }
    public PushDataBuilder Title(string? title) { this.title = title; return this; }
    public PushDataBuilder Device(string? device) { this.device = device; return this; }
    public PushDataBuilder Timestamp(long? timestamp) { this.timestamp = timestamp; return this; }
    public PushDataBuilder PushAttachment(PushAttachment? attachment) { this.attachment = attachment; return this; }
    public PushDataBuilder Html(bool? html) { this.html = html; return this; }
    public PushDataBuilder PushSound(PushSound? sound) { this.sound = sound; return this; }
    public PushDataBuilder PushUrl(PushUrl? url) { this.url = url; return this; }

    /// <summary>
    /// Shorthand for setting timestamp to the current unix timestamp
    /// </summary>
    public PushDataBuilder CurrentTimestamp() {
        return this.Timestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
    }

    public PushData Build() {
        return new PushData(priority, title, device, timestamp, attachment, html, sound, url);
    }
    
    public PushDataBuilder() {
        
    }
}