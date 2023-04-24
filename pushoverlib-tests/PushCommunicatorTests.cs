namespace pushoverlib_tests; 

public class PushCommunicatorTests {
    private TestServer server;
    private PushData defData;
    
    [OneTimeSetUp]
    public void SingleSetUp() {
        server = new TestServer(8080);
        PushCommunicator.TestMode(8080);
        defData = new PushDataBuilder().Build().AddNeeded("fakeToken", "fakeToken", "msg");
    }

    [Test]
    public void SendAsync() {
        Assert.Multiple(async () => {
            Respond(500);
            Assert.ThrowsAsync(typeof(PushException), async () => {
                await PushCommunicator.SendAsync(defData);
            }, "Success on error code");
            Respond(400, "{\"message\":\"cannot be blank\",\"errors\":[\"message cannot be blank\"],\"status\":0,\"request\":\"reqToken\"}");
            Assert.That((await PushCommunicator.SendAsync(defData)).HasErrors(), Is.False, "Didn't recognize errors");
            Assert.ThrowsAsync(typeof(PushRequestException), async () => {
                await PushCommunicator.ConfirmSendAsync(defData);
            }, "Errors success in ConfirmSendAsync");
        });
    }

    private void Respond(int status, string msg = "") {
        server.SetResponse(status, msg);
    }
}