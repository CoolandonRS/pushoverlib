namespace pushoverlib_tests; 

public class PushDataTests {
    [Test]
    public void AddNeededThrows() {
        Assert.Multiple(() => {
            var dat = new PushData();
            Assert.Throws(typeof(InvalidOperationException), () => {
                dat.ToDict();
            }, "Dict success without needed param");
            Assert.DoesNotThrow(() => {
                dat.AddNeeded("", "", "");
            }, "Throw on first needed set");
            Assert.DoesNotThrow(() => {
                dat.ToDict();
            }, "Throw on valid dict");
            Assert.Throws(typeof(InvalidOperationException), () => {
                dat.AddNeeded("", "", "");
            }, "Success on double throw");
            Assert.DoesNotThrow(() => {
                dat.ToDict();
            }, "Dict failure after redundant set");
        });
    }

    [Test]
    public void DictTests() {
        Assert.Multiple(() => {
            Assert.That(new PushData().AddNeeded("api", "usr", "msg").ToDict(), Is.EqualTo(new Dictionary<string,string>() { { "token", "api"}, {"user", "usr"}, {"message", "msg"} }), "Base dict fail");
        });
    }

    [Test]
    public void Verify() {
        Assert.Multiple(() => {
            Assert.Throws(typeof(PushRequestException), () => {
                new PushData {
                    Priority = PushData.PushPriority.RequireAck
                }.Verify();
            }, "RequireAck success without retry/expire");
            Assert.Throws(typeof(PushRequestException), () => {
                new PushData {
                    Priority = PushData.PushPriority.RequireAck,
                    Retry = 30
                }.Verify();
            }, "RequireAck success without expire");
            Assert.Throws(typeof(PushRequestException), () => {
                new PushData {
                    Priority = PushData.PushPriority.RequireAck,
                    Expire = 30
                }.Verify();
            }, "RequireAck success without retry");
            Assert.Throws(typeof(PushRequestException), () => {
                new PushData {
                    Priority = PushData.PushPriority.RequireAck,
                    Retry = 1,
                    Expire = 1
                }.Verify();
            }, "RequireAck success with short retry");
            Assert.Throws(typeof(PushRequestException), () => {
                new PushData {
                    Priority = PushData.PushPriority.RequireAck,
                    Retry = 30,
                    Expire = 11000
                }.Verify();
            }, "RequireAck success with long expire");
            Assert.DoesNotThrow(() => {
                new PushData {
                    Priority = PushData.PushPriority.RequireAck,
                    Retry = 30,
                    Expire = 30
                }.Verify();
            }, "RequireAck failure when valid");
            Assert.DoesNotThrow(() => {
                new PushData {
                    Priority = PushData.PushPriority.Notify
                }.Verify();
            }, "Non RequireAck failure when valid");
        });
    }
}