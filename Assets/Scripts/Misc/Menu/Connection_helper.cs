using PurrNet;
using PurrNet.Transports;
using UnityEngine;

public class Connection_helper : MonoBehaviour
{
    public NetworkManager net_manager;
    public UDPTransport udpTransport;
    
    


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

}
