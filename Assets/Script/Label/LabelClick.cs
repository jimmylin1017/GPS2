using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LabelClick : MonoBehaviour {

    private Dictionary<string, LabelNode> labelList;

    public void getLabelContent()
    {
        Debug.Log("getLabelContent");
        // 取得主要的 labelList
        labelList = LabelMain.Instance.labelList;

        Image labelContent = GameObject.Find("LabelContent").GetComponent<Image>();
        Text labelContentText = GameObject.Find("LabelContentText").GetComponent<Text>();
        labelContent.enabled = true;
        labelContentText.enabled = true;
        labelContentText.text = labelList[GetComponent<Image>().name].labelContent;
    }
}
