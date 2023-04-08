namespace pushoverlib; 

public class PushSound {
    public readonly Types Type;
    public readonly string? Name;

    internal string Evaluate() {
        return Type == Types.Custom ? Name! : Type.ToString().ToLower();
    }

    public PushSound(Types type, string? name = null) {
        if (type == Types.Custom && name == null) throw new InvalidOperationException("Name must not be null when type is Custom");
        if (type != Types.Custom && name != null) throw new InvalidOperationException("Name must be null when type is not Custom");
        this.Type = type;
        this.Name = name;
    }

    public enum Types {
        Pushover,
        Bike,
        Bugle,
        CashRegister,
        Classical,
        Cosmic,
        Falling,
        GameLan,
        Incoming,
        Intermission,
        Magic,
        Mechanical,
        PianoBar,
        Siren,
        SpaceAlarm,
        TugBoat,
        Alien,
        Climb,
        Persistent,
        Echo,
        UpDown,
        Vibrate,
        None,
        Custom
    }
}