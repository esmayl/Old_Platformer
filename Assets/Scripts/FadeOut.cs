using UnityEngine;
using System.Collections;

public class FadeOut : MonoBehaviour {

	void Update () {
        GetComponent<Light>().intensity -= Time.deltaTime*1.5f;
	}
}
