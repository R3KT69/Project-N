using PurrNet;
using PurrNet.Transports;
using UnityEngine;

public class Connection_helper : MonoBehaviour
{
    public NetworkManager net_manager;
    public UDPTransport udpTransport;
    
    private void OnEnable()
    {
        udpTransport.onConnected += HandleConnected;
    }

    private void OnDisable()
    {
        udpTransport.onConnected -= HandleConnected;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (net_manager == null || udpTransport == null)
        {
            net_manager = gameObject.GetComponent<NetworkManager>();
            udpTransport = gameObject.GetComponent<UDPTransport>();
        }   
    }

    void Start()
    {
        udpTransport.address = Network_connection.server_ip;
        udpTransport.serverPort = 5000;
        

        if (Network_connection.start_host)
        {
            Debug.Log($"connection type: host: {Network_connection.start_host}");
            net_manager.StartHost();
            
        }

        if (Network_connection.start_client)
        {
            Debug.Log($"connection type: client: {Network_connection.start_client}");
            net_manager.StartClient();
        }
    }

    private void HandleConnected(Connection conn, bool asServer)
    {
        if (!asServer) return; 
        var peer = udpTransport.peers[conn]; 
        Debug.Log($"Client connected: {peer.Address}:{peer.Port}");
        
        
        
    }

}
