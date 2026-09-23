using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Network_connection : MonoBehaviour
{
    public TMP_InputField inputField;
    public Button host_btn;
    public Button connect_btn;
    public static string server_ip;
    public static bool start_host = false;
    public static bool start_client = false;

    

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Exec_host()
    {
        server_ip = inputField.text;
        start_host = true;
        Debug.Log($"Hosting, ip:  {server_ip}");
        SceneManager.LoadScene("Test");
    }

    public void Exec_conn()
    {
        server_ip = inputField.text;
        start_client = true;
        Debug.Log($"Connecting to ip:  {server_ip}");
        SceneManager.LoadScene("Test");
    }
}
