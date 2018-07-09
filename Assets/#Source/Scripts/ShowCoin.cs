using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShowCoin : MonoBehaviour {

    private Text coin;

    // Use this for initialization
    void Start()
    {
        coin = this.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        coin.text = GameInfo.coinNum.ToString();
    }
}
