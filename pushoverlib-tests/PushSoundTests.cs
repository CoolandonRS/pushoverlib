namespace pushoverlib_tests; 

using static PushSound.Types;

public class PushSoundTests {
    [Test]
    public void Evaluate() {
        Assert.Multiple(() => {
            Assert.That(new PushSound(Pushover).Evaluate(), Is.EqualTo("pushover"));
            Assert.That(new PushSound(Custom, "cool_sound").Evaluate(), Is.EqualTo("cool_sound"));
        });
    }

    [Test]
    public void Constructor() {
        Assert.Multiple(() => {
            Assert.Throws(typeof(InvalidOperationException), () => {
                new PushSound(Custom);
            });
            Assert.Throws(typeof(InvalidOperationException), () => {
                new PushSound(Falling, "hi");
            });
            Assert.DoesNotThrow(() => {
                new PushSound(Custom, "abcd");
            });
            Assert.DoesNotThrow(() => {
                new PushSound(UpDown);
            });
        });
    }
}