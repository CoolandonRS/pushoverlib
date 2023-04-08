namespace pushoverlib; 

public class PushAttachment {
    public readonly byte[] Data;
    public readonly Types Type;
    public readonly string Filename;


    public string EvaluateType() {
        return Type switch {
            Types.Base64 => "attachment_base64",
            Types.SvgXml => "image/svg+xml",
            _ => "image/" + Type
        };
    }

    public bool IsBase64() {
        return Type == Types.Base64;
    }

    public byte[] GetData() {
        return Data;
    }

    public Types GetTypes() {
        return Type;
    }

    /// <summary>
    /// NOTE: Only has commonly used types. Want to use a different one? Convert it or extend this class. As long as <see cref="EvaluateType()"/> works, The code shouldn't care
    /// </summary>
    public enum Types {
        Base64,
        Jpeg,
        Gif,
        Png,
        Heic,
        Avif,
        Webp,
        SvgXml
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <param name="type"></param>
    /// <param name="filename">unused if Base64, but still expected for convenience</param>
    public PushAttachment(byte[] data, Types type, string filename) {
        this.Data = data;
        this.Type = type;
        this.Filename = filename;
    }
}