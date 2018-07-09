using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShowMi : MonoBehaviour {


    private Text mi;

	// Use this for initialization
	void Start () {
        mi = this.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        mi.text = GameInfo.mi.ToString();
	}
}
