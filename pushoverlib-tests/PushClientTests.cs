using System.Text.Json;

namespace pushoverlib_tests; 

public class PushClientTests {
    private TestServer server;
    private PushClient? defClient;
    private PushData defData;
    private PushResult defResult;
    
    [OneTimeSetUp]
    public void SingleSetUp() {
        server = new TestServer(30002);
        PushCommunicator.TestMode(30002);
        defData = new PushData();
        defResult = new PushResult(JsonDocument.Parse(PushResultTests.success).RootElement);
    }

    [SetUp]
    public void SetUp() {
        defClient = new PushClient("", "");
    }

    [TearDown]
    public void TearDown() {
        defClient = null;
    }

    [Test]
    public async Task Send() {
        server.SetResponse(200, PushResultTests.success);
        Assert.That((await defClient!.Send("", defData)).ToJsonString(), Is.EqualTo(PushResultTests.success), "Incorrect result json received");
    }

    [Test]
    public async Task GetReceiptResult() {
        server.SetResponse(200, PushReceiptResultTests.raw);
        Assert.That((await defClient!.GetReceiptResult(defResult)).ToJsonString(), Is.EqualTo(PushReceiptResultTests.formatted), "Incorrect receipt json received");
    }

    [Test]
    public void CancelReceipt() {
        server.SetResponse(200);
        Assert.DoesNotThrowAsync(async () => {
            await defClient!.CancelReceipt(defResult);
        });
    }
}