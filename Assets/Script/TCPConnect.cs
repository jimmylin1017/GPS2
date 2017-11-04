using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class TCPConnect : MonoBehaviour {

    //public static TCPConnect Instance { set; get; }

    public TcpClient tcpClient;

    public Dropdown mapListDropDown;
    public List<string> mapNameList;
    public string selectMapName;

    public void MapDropdown_IndexChanged(int index)
    {
        selectMapName = mapNameList[index];

        Debug.Log(selectMapName);
    }

    private void Start()
    {
        selectMapName = "中山大學";

        // create mapListDropDown
        /*mapNameList = new List<string>();
        DirectoryInfo dir = new DirectoryInfo(Application.persistentDataPath);
        FileInfo[] info = dir.GetFiles("*.txt");
        foreach (FileInfo fileinfo in info)
        {
            mapNameList.Add(fileinfo.Name);
        }

        mapListDropDown.AddOptions(mapNameList);*/
    }

    public void downloadMap()
    {
        Debug.Log(selectMapName);

        tcpClient = createTcpClient();

        if(tcpClient != null)
        {
            // 建立串流
            NetworkStream stream = tcpClient.GetStream();

            // 要求檔案
            string name = selectMapName;
            string word = "0";
            Byte[] data = System.Text.UnicodeEncoding.Unicode.GetBytes(selectMapName);
            Byte[] updown_data = System.Text.UnicodeEncoding.Unicode.GetBytes(word);
            byte[] bytes1 = new byte[20480000];

            string path = Application.persistentDataPath + "/" + name + ".zip";

            try
            {
                stream.Write(data, 0, data.Length);
                Console.WriteLine("Sent: {0}", name);
                data = new Byte[256];
                String responseData = String.Empty;
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.UnicodeEncoding.Unicode.GetString(data, 0, bytes);
                Console.WriteLine("Received: {0}", responseData);


                stream.Write(updown_data, 0, updown_data.Length);
                Console.WriteLine("Sent: {0}", word);
                updown_data = new Byte[256];
                String updown_responseData = String.Empty;
                Int32 ln_bytes = stream.Read(updown_data, 0, updown_data.Length);
                updown_responseData = System.Text.Encoding.ASCII.GetString(updown_data, 0, ln_bytes);
                Console.WriteLine("Received: {0}", updown_responseData);

                FileStream ff = new FileStream(path, FileMode.Create, FileAccess.Write);
                int NoOfPackets = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(bytes1.Length) / Convert.ToDouble(2048)));
                NoOfPackets = NoOfPackets - 1;
                for (int a = 0; a < NoOfPackets; a++)
                {
                    if (stream.CanRead)
                    {
                        stream.Read(bytes1, 2048 * a, 2048);
                        Console.WriteLine(stream.DataAvailable);
                        Console.WriteLine(2048 * a);
                    }
                }
                if (stream.CanRead)
                    stream.Read(bytes1, (NoOfPackets) * 2048, bytes1.Length - (NoOfPackets) * 2048);

                ff.Write(bytes1, 0, bytes1.Length);
                ff.Close();
                stream.Close();

                Debug.Log("Finish");
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
        }

        Unzip();
    }

    public TcpClient createTcpClient()
    {
        string serverIP = "140.117.111.116";
        int serverPort = 12000;

        // 建立一個 TcpClient;
        try
        {
            TcpClient client = new TcpClient(serverIP, serverPort);
            return client;
        }
        catch (SocketException se)
        {
            Debug.Log(se.ToString());
            return null;
        }
    }

    public void Unzip()
    {
        string zipfilePath = Application.persistentDataPath + "/" + selectMapName + ".zip";
        string exportLocation = Application.persistentDataPath + "/" + selectMapName;
        if (File.Exists(zipfilePath) == false)
        {
            Debug.LogError(zipfilePath + "is not found!");
            System.Diagnostics.Process.Start(Path.GetDirectoryName(zipfilePath));
            return;
        }

        ZipUtil.Unzip(zipfilePath, exportLocation);
        System.Diagnostics.Process.Start(Path.GetDirectoryName(zipfilePath));
    }
}
