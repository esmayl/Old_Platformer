using UnityEngine;
using System.Collections;

public class UpdateForGI : MonoBehaviour
{

	void Update ()
    {
	
        DynamicGI.UpdateMaterials(GetComponent<Renderer>());
        DynamicGI.UpdateEnvironment();
	}
}
