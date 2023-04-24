using System.Net;
using System.Net.Sockets;
using System.Text;

namespace pushoverlib_tests; 

internal class TestServer {
    public int Port { get; private set; }
    private HttpListener listener;
    public Response Resp { get; private set; } = new Response(500, "No response set");

    public class Response {
        public readonly int StatusCode;
        public readonly string msg;

        public Response(int statusCode, string msg = "") {
            StatusCode = statusCode;
            this.msg = msg;
        }
    }

    public void Start() {
        listener.Start();
        Receive();
    }

    public void Stop() {
        listener.Stop();
    }

    private void Receive() {
        listener.BeginGetContext(new AsyncCallback(ListenerCallback), listener);
    }

    private void ListenerCallback(IAsyncResult result) {
        if (!listener.IsListening) return;
        
        var context = listener.EndGetContext(result);
        var request = context.Request;
        var response = context.Response;
        response.StatusCode = Resp.StatusCode;
        response.ContentType = "application/json";
        response.OutputStream.Write(Encoding.UTF8.GetBytes(Resp.msg));
        response.OutputStream.Close();
        
        Receive();
    }

    public void SetResponse(Response response) {
        Resp = response;
    }

    public void SetResponse(int statusCode, string msg = "") {
        Resp = new Response(statusCode, msg);
    }
    
    public TestServer(int port, bool startOnConstruct = true) {
        this.Port = port;
        this.listener = new HttpListener();
        listener.Prefixes.Add("http://127.0.0.1:" + port + "/");
        if (startOnConstruct) Start();
    }
}