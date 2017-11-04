using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class LabelMain : MonoBehaviour {

    public static LabelMain Instance { set; get; }

    public Dictionary<string, LabelNode> labelList;
    public double zoomDistance; // 給 Google Map 判斷 Zoom 大小

    public Dropdown fileListDropDown;
    public List<string> fileNameList;
    public string selectFileName;

    public string currentLabelContentName;

    public void Dropdown_IndexChanged(int index)
    {
        selectFileName = fileNameList[index];

        Debug.Log(selectFileName);
    }

    private void Start()
    {
        Instance = this;

        // 建立全域 labelList
        labelList = new Dictionary<string, LabelNode>();

        // 初始化 zoomDistance
        zoomDistance = 0;

        // 初始化 currentLabelContentName
        currentLabelContentName = string.Empty;

        // 預設檔名，防止使用者沒有出入檔名
        selectFileName = "temp";
        string dirPath = Application.persistentDataPath + "/" + selectFileName;
        if (!Directory.Exists(dirPath))
            Directory.CreateDirectory(dirPath);

        // 建立暫存檔
        string nodePath = Application.persistentDataPath + "/" + selectFileName + "/" + "map.txt";
        string labelPath = Application.persistentDataPath + "/" + selectFileName + "/" + "tag.txt";

        StreamWriter nodeWriter = new StreamWriter(nodePath, false, Encoding.UTF8);
        StreamWriter labeWriter = new StreamWriter(labelPath, false, Encoding.UTF8);
        nodeWriter.Close();
        labeWriter.Close();

        // create fileListDropDown
        fileNameList = new List<string>();
        DirectoryInfo dir = new DirectoryInfo(Application.persistentDataPath);
        DirectoryInfo[] info = dir.GetDirectories();
        foreach (DirectoryInfo dirinfo in info)
        {
            if(dirinfo.Name != "Unity")
                fileNameList.Add(dirinfo.Name);
        }

        fileListDropDown.AddOptions(fileNameList);
    }

    private LabelNode label; // 暫存 label
    public string labelName; // label 的名稱
    private float labelLatitude; // label 的緯度
    private float labelLongitude; // label 的經度
    private double labelDistance; // label 跟人的距離

    public void createLabelList()
    {
        // 清空 labelList，重新加入檔案中的 label
        labelList.Clear();

        // 檔案位置
        string dirPath = Application.persistentDataPath + "/" + LabelMain.Instance.selectFileName;
        string labelPath = dirPath + "/tag.txt";
        string nodePath = dirPath + "/map.txt";

        // 讀取 tag 檔案
        StreamReader reader = new StreamReader(labelPath);
        string line;
        string[] lineSplite;

        while ((line = reader.ReadLine()) != null)
        {
            if (line == "") break;

            lineSplite = line.Split(' ');

            labelName = lineSplite[0];
            labelLatitude = float.Parse(lineSplite[1], CultureInfo.InvariantCulture.NumberFormat);
            labelLongitude = float.Parse(lineSplite[2], CultureInfo.InvariantCulture.NumberFormat);

            // 建立 label 和 labelToggle
            label = new LabelNode(labelName, labelLatitude, labelLongitude);

            // 尋找圖片
            string imgPath = dirPath + "/content/" + labelName + ".jpg";
            if(File.Exists(imgPath))
            {
                label.labelSprite = LoadNewSprite(imgPath);
            }

            // 尋找詳細資料
            string contentPath = dirPath + "/content/" + labelName + ".txt";
            if (File.Exists(contentPath))
            {
                StreamReader contentReader = new StreamReader(contentPath);
                label.labelContent = contentReader.ReadToEnd();

                Debug.Log(label.labelContent);
                contentReader.Close();
            }

            // 把 label 加入 List 中
            labelList.Add(labelName, label);

            Debug.Log(labelName);
        }

        reader.Close();

        // 讀取 map 檔案
        reader = new StreamReader(nodePath);

        while ((line = reader.ReadLine()) != null)
        {
            if (line == "") break;

            lineSplite = line.Split(' ');

            labelName = lineSplite[0];
            labelLatitude = float.Parse(lineSplite[1], CultureInfo.InvariantCulture.NumberFormat);
            labelLongitude = float.Parse(lineSplite[2], CultureInfo.InvariantCulture.NumberFormat);

            if (!labelList.ContainsKey(labelName))
            {

                // 建立 label 和 labelToggle
                label = new LabelNode(labelName, labelLatitude, labelLongitude);

                // 設定 isNode
                label.isNode = true;

                // 把 label 加入 List 中
                labelList.Add(labelName, label);

                Debug.Log(labelName);
            }
        }

        reader.Close();

        Debug.Log("createLabelList");
    }

    public Sprite LoadNewSprite(string FilePath, float PixelsPerUnit = 100.0f)
    {
        // Load a PNG or JPG image from disk to a Texture2D, assign this texture to a new sprite and return its reference
        Sprite NewSprite = new Sprite();
        Texture2D SpriteTexture = LoadTexture(FilePath);
        NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0), PixelsPerUnit);

        return NewSprite;
    }

    public Texture2D LoadTexture(string FilePath)
    {
        // Load a PNG or JPG file from disk to a Texture2D
        // Returns null if load fails
        Texture2D Tex2D;
        byte[] FileData;

        if (File.Exists(FilePath))
        {
            FileData = File.ReadAllBytes(FilePath);
            Tex2D = new Texture2D(2, 2);           // Create new "empty" texture
            if (Tex2D.LoadImage(FileData))           // Load the imagedata into the texture (size is set automatically)
                return Tex2D;                 // If data = readable -> return texture
        }
        return null;                     // Return null if load failed
    }

    public void findZoomDistance()
    {
        // 判斷 MAX Distance
        if (zoomDistance < label.labelDistance)
        {
            zoomDistance = label.labelDistance;
        }
    }
}
