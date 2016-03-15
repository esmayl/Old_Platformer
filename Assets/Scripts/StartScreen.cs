using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartScreen : MonoBehaviour {


    void Start()
    {
        GetComponent<Image>().material.color = new Color(255,255,255,255);
    }

	// Update is called once per frame
	void Update () {
            GetComponent<Image>().material.color -= new Color(0, 0, 0, 1);
	}
}
