﻿using System.Buffers.Text;
using System.Runtime.InteropServices;

using static CoolandonRS.pushoverlib.PushAttachment.Types;

namespace pushoverlib_tests; 

public class PushAttachmentTests {
    private const PushAttachment.Types Base64 = PushAttachment.Types.Base64; // to solve ambiguity
    
    [Test]
    public void EvaluateImageType([Range(1, 6, 1)] int typeInt) {
        var type = (PushAttachment.Types)typeInt;
        var attach = new PushAttachment(new byte[1], type, "name");
        Assert.That(attach.EvaluateType(), Is.EqualTo("image/" + type.ToString().ToLower()), type + " failed to evaluate");
    }

    [Test]
    public void EvaluateTypeOddities() {
        Assert.Multiple(() => {
            Assert.That(new PushAttachment(new byte[1], Base64, "").EvaluateType(), Is.EqualTo("attachment_base64"), "Base64 fail");
            Assert.That(new PushAttachment(new byte[1], SvgXml, "").EvaluateType(), Is.EqualTo("image/svg+xml"), "SvgXml fail");
        });
    }

    [Test]
    public void IsBase64() {
        Assert.Multiple(() => {
            Assert.That(new PushAttachment(new byte[1], PushAttachment.Types.Base64, "").IsBase64(), Is.True, "Truthy base64 failure");
            Assert.That(new PushAttachment(new byte[1], Png, "").IsBase64(), Is.False, "Falsey base64 success");
        });
    }

    [Test]
    public void GetData() {
        var data = new byte[] { 3 };
        Assert.That(new PushAttachment(data, Gif, "").GetData(), Is.EqualTo(data), "Data mismatch");
    }

    [Test]
    public void GetTypes() {
        Assert.That(new PushAttachment(new byte[1], Webp, "").GetTypes(), Is.EqualTo(Webp), "Type mismatch");
    }
}  