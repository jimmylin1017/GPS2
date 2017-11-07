using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class LabelMapSelect : MonoBehaviour {

    // 獲取全域變數
    private Dictionary<string, LabelNode> labelList;

    // Function createLabelList() 所需要的變數
    private LabelNode label; // 暫存 label
    private string labelName; // label 的名稱
    private float labelLatitude; // label 的緯度
    private float labelLongitude; // label 的經度
    private double labelDistance; // label 跟人的距離
    private int labelStars; // label 的星級

    private void Start()
    {
        labelList = new Dictionary<string, LabelNode>();
    }

    public void selectMap()
    {
        LabelMain.Instance.selectFileName = gameObject.name;

        Debug.Log(LabelMain.Instance.selectFileName);

        LabelMain.Instance.selectFileNameChange = true;

        createLabelList();
    }

    public void createLabelList()
    {
        // 清空 labelList，重新加入檔案中的 label
        labelList.Clear();

        // 檔案位置，由 LabelMain.Instance.selectFileName 決定
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

            if (lineSplite.Length > 3)
            {
                labelStars = int.Parse(lineSplite[3], CultureInfo.InvariantCulture.NumberFormat);
            }
            else
            {
                labelStars = 0;
            }

            // 建立 label 和 labelToggle
            label = new LabelNode(labelName, labelLatitude, labelLongitude, labelStars);

            // 尋找圖片
            string imgPath = dirPath + "/content/" + labelName + ".jpg";
            if (File.Exists(imgPath))
            {
                label.labelSprite = LabelMain.Instance.LoadNewSprite(imgPath);
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

        // 取代掉原本的 labelList
        LabelMain.Instance.labelList = labelList;

        Debug.Log("createLabelList");
    }
}
