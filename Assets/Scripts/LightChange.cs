using UnityEngine;
using System.Collections;

public class LightChange : MonoBehaviour
{
    private Material thisMat;
    private Color oldColor;
    private Color newColor;

    public float timer = 2.1f;

    private float counter;
    private bool changingLight = false;
    private bool lightOn = true;


    void Start ()
	{
	    thisMat = GetComponent<MeshRenderer>().material;

        oldColor = thisMat.GetColor("_EmissionColor");
        counter = timer;
	}
	
	void Update ()
    {

	    if (counter > timer)
	    {
	        UpdateLight();
	        counter = 0;
	    }
	    else
	    {
	        counter += Time.deltaTime;
	    }

	    if (counter > timer/5)
	    {
	        thisMat.SetColor("_EmissionColor",oldColor);
	    }

    }

    public void UpdateLight()
    {
        if (changingLight){return;}

        changingLight = true;

        if (thisMat != null)
        {
            thisMat.SetColor("_EmissionColor", newColor);
        }

        float randomVal = Random.Range(3.2f, 3.3f);
        newColor = new Color(oldColor.r + randomVal, oldColor.g + randomVal, oldColor.b + randomVal);

        changingLight = false;
    }
}
