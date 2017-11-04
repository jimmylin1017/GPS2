using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LabelClick : MonoBehaviour {

    private Dictionary<string, LabelNode> labelList;

    /*private void Start()
    {
        // 取得主要的 labelList
        labelList = LabelMain.Instance.labelList;

        Text labelContentText = GameObject.Find("LabelContentText").GetComponent<Text>();
        labelContentText.text = labelList[GetComponent<Image>().name].labelContent;
    }*/

    public void getLabelContent()
    {
        Debug.Log("getLabelContent");
        // 取得主要的 labelList
        labelList = LabelMain.Instance.labelList;

        GameObject labelContent = GameObject.Find("LabelContent");
        labelContent.transform.localPosition = new Vector3(labelContent.transform.localPosition.x, -275, labelContent.transform.localPosition.z);
        Text labelContentText = GameObject.Find("LabelContentText").GetComponent<Text>();
        labelContentText.text = labelList[GetComponent<Image>().name].labelContent;
    }
}
