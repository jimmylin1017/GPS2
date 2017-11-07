using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LabelEdit : MonoBehaviour {

    private Dictionary<string, LabelNode> labelList;
    private string selectedToEditLabelDetail;

    private LabelNode label;

    public Image labelImage; // label 的圖片
    public Text labelName; // label 的名稱
    public Text labelLatitude; // label 的緯度
    public Text labelLongitude; // label 的經度
    public Text labelStars; // label 的星級

    public InputField labelNameInputField; // label 的名稱，顯示用
    public InputField labelStarsInputField; // label 的星級，顯示用

    private void Start()
    {
        // 取得主要的 labelList
        labelList = LabelMain.Instance.labelList;

        // 取得選擇的 label
        selectedToEditLabelDetail = LabelMain.Instance.selectedToEditLabelDetail;
        label = labelList[selectedToEditLabelDetail];

        labelNameInputField.text = label.labelName;
        labelLatitude.text += label.labelLatitude.ToString();
        labelLongitude.text += label.labelLongitude.ToString();


        if (label.isNode)
        {
            GameObject.Find("ImageFrame").SetActive(false);
            GameObject.Find("LabelStarsInputField").SetActive(false);
            GameObject.Find("LabelStars").SetActive(false);
        }
        else
        {
            labelStarsInputField.text = label.labelStars.ToString();
            labelImage.sprite = label.labelSprite;
        }
    }

    private void Update()
    {
        // 偵測手機返回鍵
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 從 EditLabel Scene 跳到 SetLabel Scene
            SceneManager.LoadScene("SetLabel");
        }
    }

    public void saveEditData()
    {
        Debug.Log(labelName.text);

        int labelStarsNum;

        if (label.isNode)
        {
            // 更新 labelName
            if(labelName.text != label.labelName)
            {
                labelList.Remove(label.labelName);
                label.labelName = labelName.text;

                labelList.Add(label.labelName, label);
            }
            
            // 從 EditLabel Scene 跳到 SetLabel Scene
            SceneManager.LoadScene("SetLabel");
        }
        else
        {
            if (int.TryParse(labelStars.text, out labelStarsNum))
            {
                // 更新 labelName
                labelList.Remove(label.labelName);

                label.labelName = labelName.text;
                label.labelStars = labelStarsNum;
                label.labelSprite = labelImage.sprite;

                labelList.Add(label.labelName, label);

                // 從 EditLabel Scene 跳到 SetLabel Scene
                SceneManager.LoadScene("SetLabel");
            }
        }
    }
}
