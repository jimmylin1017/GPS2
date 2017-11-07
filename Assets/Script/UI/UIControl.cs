using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIControl : MonoBehaviour {
    public static int uiStatus = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if((uiStatus == 1 || uiStatus == 2 ) && Input.GetKeyDown(KeyCode.Escape)) {
            uiStatus = 0;
			GameObject TagCamera = GameObject.Find("LabelCamera");
			GameObject Anchor =  GameObject.Find("ScrollAnchor");
			GameObject leftBackPoint =  GameObject.Find("BackPointLeft");
			GameObject rightBackPoint =  GameObject.Find("BackPointRight");
			MonoBehaviour scroll= Anchor.GetComponent("FasterScroll") as MonoBehaviour;
			TagCamera.GetComponent<Camera>().enabled = true;
			Anchor.GetComponent<ScrollRect>().enabled = false;
			scroll.enabled = false;
			leftBackPoint.GetComponent<ScrollRect>().enabled = true;
			rightBackPoint.GetComponent<ScrollRect>().enabled = true;
        }
	}

	public void backToHomePage() {
		uiStatus = 0;
		GameObject TagCamera = GameObject.Find("LabelCamera");
		GameObject Anchor =  GameObject.Find("ScrollAnchor");
		GameObject leftBackPoint =  GameObject.Find("BackPointLeft");
		GameObject rightBackPoint =  GameObject.Find("BackPointRight");
		MonoBehaviour scroll= Anchor.GetComponent("FasterScroll") as MonoBehaviour;
		TagCamera.GetComponent<Camera>().enabled = true;
		Anchor.GetComponent<ScrollRect>().enabled = false;
        scroll.enabled = false;
		leftBackPoint.GetComponent<ScrollRect>().enabled = true;
		rightBackPoint.GetComponent<ScrollRect>().enabled = true;
	}
}

