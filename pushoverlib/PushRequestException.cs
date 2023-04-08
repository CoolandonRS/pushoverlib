namespace pushoverlib; 

public class PushRequestException : PushException {
    public PushRequestException() {
    }
    public PushRequestException(string msg) : base(msg) {
        
    }
}