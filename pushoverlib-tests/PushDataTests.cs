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
        
    }

    [Test]
    public void Constructor() {
        // In each instance, the .Build() calls the PushData constructor. This is where we check for breaks.
        Assert.Multiple(() => {
            Assert.Throws(typeof(PushRequestException), () => {
                new PushDataBuilder().Priority(PushData.PushPriority.RequireAck).Build();
            }, "RequireAck success without retry/expire");
            Assert.Throws(typeof(PushRequestException), () => {
                new PushDataBuilder().Priority(PushData.PushPriority.RequireAck).Retry(30).Build();
            }, "RequireAck success without expire");
            Assert.Throws(typeof(PushRequestException), () => {
                new PushDataBuilder().Priority(PushData.PushPriority.RequireAck).Expire(30).Build();
            }, "RequireAck success without retry");
            Assert.Throws(typeof(PushRequestException), () => {
                new PushDataBuilder().Priority(PushData.PushPriority.RequireAck).Retry(1).Expire(1).Build();
            }, "RequireAck success with short retry");
            Assert.Throws(typeof(PushRequestException), () => {
                new PushDataBuilder().Priority(PushData.PushPriority.RequireAck).Retry(30).Expire(11000).Build();
            }, "RequireAck success with long expire");
            Assert.DoesNotThrow(() => {
                new PushDataBuilder().Priority(PushData.PushPriority.RequireAck).Retry(30).Expire(30).Build();
            }, "RequireAck failure when valid");
            Assert.DoesNotThrow(() => {
                new PushDataBuilder().Priority(PushData.PushPriority.Notify).Build();
            }, "Non RequireAck failure when valid");
        });
    }
}