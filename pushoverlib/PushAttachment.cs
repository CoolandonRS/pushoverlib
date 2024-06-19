namespace CoolandonRS.pushoverlib; 

public class PushAttachment {
    public readonly byte[] Data;
    public readonly Types Type;
    public readonly string Filename;

    public readonly bool IsBase64;


    public string EvaluateType() {
        return Type switch {
            Types.SvgXml => "image/svg+xml",
            _ => "image/" + Type.ToString().ToLower()
        };
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
        Jpeg,
        Gif,
        Png,
        Heic,
        Avif,
        Webp,
        SvgXml
    }

    /// <summary>
    /// Base64 Constructor
    /// </summary>
    /// <param name="data"></param>
    /// <param name="type"></param>
    public PushAttachment(byte[] data, Types type) {
        this.Data = data;
        this.Type = type;
        this.Filename = "";
        this.IsBase64 = true;
    }
    /// <summary>
    /// Non-Base64 constructor
    /// </summary>
    /// <param name="data"></param>
    /// <param name="type"></param>
    /// <param name="filename"></param>
    public PushAttachment(byte[] data, Types type, string filename) {
        this.Data = data;
        this.Type = type;
        this.Filename = filename;
        this.IsBase64 = false;
    }
}