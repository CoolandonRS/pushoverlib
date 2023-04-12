using System.Net;
using System.Net.Sockets;

namespace pushoverlib_tests; 

internal class TestServer {
    public int Port { get; private set; }
    private HttpListener listener;

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
        
    }

    public TestServer(int port) {
        this.Port = port;
        this.listener = new HttpListener();
        listener.Prefixes.Add("http://*:" + port + "/");
    }
}