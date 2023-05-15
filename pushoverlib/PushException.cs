namespace CoolandonRS.pushoverlib; 

public class PushException : Exception {
    public PushException() {
        
    }
    public PushException(string msg) : base(msg) {
        
    }
}