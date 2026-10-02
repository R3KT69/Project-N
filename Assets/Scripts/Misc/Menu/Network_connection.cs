using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Network_connection : MonoBehaviour
{
    public TMP_InputField server_ip_field, server_port_field;
    public Button host_btn;
    public Button connect_btn;
    public static string server_ip;
    public static string server_port;
    public static bool start_host = false;
    public static bool start_client = false;
    public string DefaultIp;
    public string DefaultPort;

    

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Exec_host()
    {
        server_ip = string.IsNullOrWhiteSpace(server_ip_field.text) ? DefaultIp : server_ip_field.text;
        server_port = string.IsNullOrWhiteSpace(server_port_field.text) ? DefaultPort : server_port_field.text;

        start_host = true;
        Debug.Log($"Hosting, ip:  {server_ip}");
        SceneManager.LoadScene("Test");
    }

    public void Exec_conn()
    {
        server_ip = string.IsNullOrWhiteSpace(server_ip_field.text) ? DefaultIp : server_ip_field.text;
        server_port = string.IsNullOrWhiteSpace(server_port_field.text) ? DefaultPort : server_port_field.text;

        start_client = true;
        Debug.Log($"Connecting to ip:  {server_ip}");
        SceneManager.LoadScene("Test");
    }
}
