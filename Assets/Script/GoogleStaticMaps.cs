using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoogleStaticMaps : MonoBehaviour {

    List<GoogleMapMarker> markers;

    GoogleMapLocation center;
    string zoom = "16";
    string size = "640x640";
    string scale = "2";
    string maptype = "roadmap";

    private Dictionary<string, LabelNode> labelList;

    private char[] labelMarker; // labelMarker 給 Map 用
    private int labelMarkerCounter; // labelMarkerCounter 計算 labelMarker

    private void Start()
    {
        labelList = LabelMain.Instance.labelList;

        // 建立 labelMarker
        labelMarkerCounter = 0;
        labelMarker = new char[] {
        'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
        'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
        'U', 'V', 'W', 'X', 'Y', 'Z', '1', '2', '3', '4',
        '5', '6', '7', '8', '9', '0'};

        markers = new List<GoogleMapMarker>();

        foreach (KeyValuePair<string, LabelNode> labelTemp in labelList)
        {
            GoogleMapMarker tempMarker;
            if (!labelTemp.Value.isNode)
            {
                tempMarker = new GoogleMapMarker("mid", "red", labelMarker[labelMarkerCounter].ToString(), new GoogleMapLocation(labelTemp.Value.labelLatitude, labelTemp.Value.labelLongitude));
            }
            else
            {
                tempMarker = new GoogleMapMarker("mid", "blue", labelMarker[labelMarkerCounter].ToString(), new GoogleMapLocation(labelTemp.Value.labelLatitude, labelTemp.Value.labelLongitude));
            }
            markers.Add(tempMarker);
            labelMarkerCounter++;
        }

        StartCoroutine(GetGoogleMap());
    }

    private void Update()
    {
        // 偵測手機返回鍵
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 從 Map Scene 跳到 SetLabel Scene
            SceneManager.LoadScene("SetLabel");
        }
    }

    IEnumerator GetGoogleMap()
    {
        string googleMapUrl = "https://maps.googleapis.com/maps/api/staticmap?";
        string parameters = string.Empty;

        parameters += ("zoom="+zoom);
        parameters += ("&size=" + size);
        parameters += ("&scale=" + scale);
        parameters += ("&maptype=" + maptype);

        foreach(GoogleMapMarker temp in markers)
        {
            parameters += "&markers=" + string.Format("size:{0}|color:{1}|label:{2}", temp.size, temp.color, temp.label);
            parameters += "|" + WWW.UnEscapeURL(string.Format("{0},{1}", temp.location.latitude, temp.location.longitude));
        }

        WWW url = new WWW(googleMapUrl + parameters);

        yield return url;
        //GetComponent<CanvasRenderer>().SetTexture(url.texture);

        GetComponent<RawImage>().texture = url.texture;

        Debug.Log(googleMapUrl + parameters);
    }
}