using UnityEngine;
using System.Collections;

public class SpawnPoint : MonoBehaviour
{

    public GameObject player;
    public GameObject camera;
    public float cameraRange = 8;

	void Start ()
	{


        GameObject playerInstance = new GameObject();
        GameObject cameraInstance = new GameObject();

        if (!player.GetComponent<Rigidbody>()) { player.AddComponent<Rigidbody>();}
        if (player) { playerInstance  = Instantiate(player, transform.position, Quaternion.identity)as GameObject;}
        if (camera) { cameraInstance = (GameObject)Instantiate(camera, new Vector3(cameraRange, transform.position.y, transform.position.z), Quaternion.identity); cameraInstance.GetComponent<LevelCamera>().player = playerInstance; cameraInstance.transform.LookAt(transform); cameraInstance.GetComponent<LevelCamera>().cameraDistance = cameraRange; }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
