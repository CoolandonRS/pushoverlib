namespace CoolandonRS.pushoverlib; 

public class PushServerException : PushException {
    public PushServerException() {
    }
    public PushServerException(string msg) : base(msg) {
        
    }
}