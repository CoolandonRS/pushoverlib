using System.Text.Json;

namespace pushoverlib_tests; 

public class PushResultTests {
    internal const string success = "{\"status\":1,\"request\":\"reqToken\"}";
    internal const string failure = "{\"errors\":[\"message cannot be blank\"],\"status\":0,\"request\":\"reqToken\"}";
    internal const string withReceipt = "{\"receipt\":\"rzuz5v3qzky4tq28obzih3afjtwp2u\",\"status\":1,\"request\":\"c7f80361-6c6d-4297-b6c5-7875b354cd1f\"}";
    
    [Test]
    public void ToJsonString() {
        Assert.Multiple(() => {
            Assert.That(new PushResult(Parse(success)).ToJsonString(), Is.EqualTo(success), "success failure");
            Assert.That(new PushResult(Parse(failure)).ToJsonString(), Is.EqualTo(failure), "failure failure");
            Assert.That(new PushResult(Parse(withReceipt)).ToJsonString(), Is.EqualTo(withReceipt), "withReceipt failure");
        });
    }

    [Test]
    public void HasReceipt() {
        Assert.Multiple(() => {
            Assert.That(new PushResult(Parse(success)).HasReceipt, Is.False);
            Assert.That(new PushResult(Parse(withReceipt)).HasReceipt, Is.True);
        });
    }

    [Test]
    public void ErrorSuccess() {
        var yes = new PushResult(Parse(success));
        var not = new PushResult(Parse(failure));
        Assert.Multiple(() => {
            Assert.That(yes.IsSuccess, Is.True);
            Assert.That(not.IsSuccess, Is.False);
            Assert.That(yes.HasErrors, Is.False);
            Assert.That(not.HasErrors, Is.True);
        });
    }
    

    public JsonElement Parse(string str) {
        return JsonDocument.Parse(str).RootElement;
    }
}