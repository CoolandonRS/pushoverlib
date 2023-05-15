namespace pushoverlib_tests; 

public class PushURLTests {
    [Test]
    public void Test() {
        Assert.Multiple(() => {
            var ans = (Url: "www.bob.com", Title: "Bob");
            var url = new PushUrl(ans.Url);
            Assert.That(url.Url, Is.EqualTo(ans.Url));
            Assert.That(url.Title, Is.Null);
            var both = new PushUrl(ans.Url, ans.Title);
            Assert.That(both.Url, Is.EqualTo(ans.Url));
            Assert.That(both.Title, Is.EqualTo(ans.Title));
        });
    }
}