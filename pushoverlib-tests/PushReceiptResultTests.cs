using System.Text.Json;

namespace pushoverlib_tests;

public class PushReceiptResultTests {
    internal const string raw = "{\"status\":1,\"acknowledged\":0,\"acknowledged_at\":0,\"acknowledged_by\":\"\",\"acknowledged_by_device\":\"\",\"last_delivered_at\":49,\"expired\":1,\"expires_at\":50,\"called_back\":0,\"called_back_at\":0,\"request\":\"reqToken\"}";

    internal const string formatted = "{\"status\":1,\"acknowledged\":false,\"last_delivered_at\":49,\"expired\":true,\"expires_at\":50,\"request\":\"reqToken\"}";

    [Test]
    public void ToJsonString() {
        Assert.That(new PushReceiptResult(Parse(raw)).ToJsonString(), Is.EqualTo(formatted), "Format failure");
    }

    [Test]
    public void Constructor() {
        Assert.DoesNotThrow(() => {
            new PushReceiptResult(Parse(raw));
        });
        Assert.Throws(typeof(PushResponseException), () => {
            new PushReceiptResult(Parse("{}"));
        });
    }

    public JsonElement Parse(string str) {
        return JsonDocument.Parse(str).RootElement;
    }
}