using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginUI : MonoBehaviour
{
    [SerializeField] TMP_InputField input_email;
    [SerializeField] TMP_InputField input_password;
    [SerializeField] Toggle toggle;

    string path;

    private void Awake()
    {
        StringBuilder sb = new();
        sb.Append(Application.persistentDataPath);
        sb.Append("/RememberMe.txt");
        path = sb.ToString();
    }

    private void OnEnable()
    {
        input_email.text = string.Empty;
        input_password.text = string.Empty;
        if (File.Exists(path))
        {
            FileStream rm_read = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(rm_read);
            if(sr.ReadLine().Equals("1"))
            {
                toggle.isOn = true;
                input_email.text = sr.ReadLine();
                Debug.Log("Read 1");
            }
            else
            {
                toggle.isOn = false;
                Debug.Log("Read 0");
            }
                
            sr.Close();
        }
    }

    public void OnClickLogin()
    {
        FirebaseManager.Instance.Login(input_email.text, input_password.text);
        FileStream rm_write = new FileStream(path, FileMode.Create);
        StreamWriter sw = new StreamWriter(rm_write);
        if (toggle.isOn)
        {
            sw.WriteLine("1");
            sw.WriteLine(input_email.text);
            Debug.Log(1);
        }
        else
        {
            sw.WriteLine("0");
            Debug.Log(0);
        }
        sw.Close();
    }
}
