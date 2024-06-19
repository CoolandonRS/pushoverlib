using System.Text.Json;

namespace pushoverlib_tests; 

public class PushAppTests {
    private TestServer server;
    private PushApp? defApp;
    private PushData defData;
    private PushResult defResult;
    
    [OneTimeSetUp]
    public void SingleSetUp() {
        server = new TestServer(30001);
        PushCommunicator.TestMode(30001);
        defData = new PushData();
        defResult = new PushResult(JsonDocument.Parse(PushResultTests.success).RootElement);
    }

    [SetUp]
    public void SetUp() {
        defApp = new PushApp("");
    }

    [TearDown]
    public void TearDown() {
        defApp = null;
    }

    [Test]
    public async Task Send() {
        server.SetResponse(200, PushResultTests.success);
        Assert.That((await defApp!.Send("", "", defData)).ToJsonString(), Is.EqualTo(PushResultTests.success), "Incorrect result json received");
    }

    [Test]
    public void GetClient() {
        Assert.That(defApp!.GetClient(""), Is.AssignableTo(typeof(PushClient)), "GetClient returned nonclient object");
    }

    [Test]
    public async Task GetReceiptResult() {
        server.SetResponse(200, PushReceiptResultTests.raw);
        Assert.That((await defApp!.GetReceiptResult(defResult)).ToJsonString(), Is.EqualTo(PushReceiptResultTests.formatted), "Incorrect receipt json received");
    }

    [Test]
    public void CancelReceipt() {
        server.SetResponse(200);
        Assert.DoesNotThrowAsync(async () => {
            await defApp!.CancelReceipt(defResult);
        });
    }
}