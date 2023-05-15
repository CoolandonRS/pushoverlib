using System.Collections.Specialized;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace pushoverlib_tests; 

public class PushCommunicatorTests {
    private TestServer server;
    private PushData defData;
    private string?  expectedType;
    
    [OneTimeSetUp]
    public void SingleSetUp() {
        server = new TestServer(30000, false);
        server.RegisterRequestListener(request => {
            Assert.Multiple(() => {
                if (expectedType != null) Assert.That(request.ContentType!.Split(';')[0], Is.EqualTo(expectedType));
            });
        });
        server.Start();
        PushCommunicator.TestMode(30000);
        defData = new PushDataBuilder().Build().AddNeeded(" ", " ", " ");
    }

    [SetUp]
    public void SetUp() {
        Respond(500);
        NoExpect();
    }

    [Test]
    public void SendAsyncResponseHandling() {
        Assert.Multiple(async () => {
            Respond(500);
            Assert.ThrowsAsync(typeof(PushServerException), async () => {
                await PushCommunicator.SendAsync(defData);
            }, "Success on error code");
            Respond(400, PushResultTests.failure);
            Assert.That((await PushCommunicator.SendAsync(defData)).HasErrors(), Is.True, "Didn't recognize errors");
            Assert.ThrowsAsync(typeof(PushRequestException), async () => {
                await PushCommunicator.ConfirmSendAsync(defData);
            }, "Errors success in ConfirmSendAsync");
            Respond(200, PushResultTests.success);
            Assert.That((await PushCommunicator.SendAsync(defData)).IsSuccess, Is.True, "Failed to acknowledge success");
            Assert.DoesNotThrowAsync(async () => {
                await PushCommunicator.ConfirmSendAsync(defData);
            }, "Confirm threw on success");
        });
    }

    // Kinda sucks, but a decent enough way since I can't think of another one
    [Test]
    public void SendAsyncDataType() {
        Assert.Multiple(async () => {
            Respond(200, PushResultTests.success);
            ExpectType("application/x-www-form-urlencoded");
            await PushCommunicator.SendAsync(defData);
            Console.WriteLine("Blank Data Success");
            await PushCommunicator.SendAsync(new PushDataBuilder().PushAttachment(new PushAttachment(new byte[1], PushAttachment.Types.Base64, "abc")).Build().AddNeeded("", "", ""));
            Console.WriteLine("Base64 Attachment Success");
            ExpectType("multipart/form-data");
            await PushCommunicator.SendAsync(new PushDataBuilder().PushAttachment(new PushAttachment(new byte[1], PushAttachment.Types.Png, "abc")).Build().AddNeeded(" ", " ", " "));
            Console.WriteLine("Attachment Success");
        });
    }

    [Test]
    public void ReceiptResponseHandling() {
        Assert.Multiple(async () => {
            Respond(400);
            Assert.ThrowsAsync(typeof(PushServerException), async () => {
                await PushCommunicator.GetReceiptResult("", "");
            }, "ReceiptResult success on failure");
            Assert.ThrowsAsync(typeof(PushServerException), async () => {
                await PushCommunicator.CancelReceipt("", "");
            }, "CancelReceipt success on failure");
            Respond(200, PushReceiptResultTests.raw);
            Assert.DoesNotThrowAsync(async () => {
                await PushCommunicator.GetReceiptResult("", "");
            }, "ReceiptResult failure on success");
            Assert.DoesNotThrowAsync(async () => {
                await PushCommunicator.CancelReceipt("", "");
            }, "CancelReceipt failure on success");
        });
    }

    private void Respond(int status, string msg = "") {
        server.SetResponse(status, msg);
    }

    private void ExpectType(string type) {
        this.expectedType = type;
    }

    private void NoExpect() {
        this.expectedType = null;
    }
}