namespace pushoverlib; 

public class PushUrl {
    public readonly string Url;
    public readonly string? Title;

    public PushUrl(string url, string? title = null) {
        this.Url = url;
        this.Title = title;
    }
}