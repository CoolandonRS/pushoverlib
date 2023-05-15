namespace CoolandonRS.pushoverlib; 

public class PushResponseException : Exception {
    public PushResponseException() {
        
    }
    public PushResponseException(string msg) : base(msg) {
        
    }

    public PushResponseException(string msg, Exception e) : base(msg, e) {
        
    }
}