using UnityEngine;
using System.Collections;

public class SpawnPoint : MonoBehaviour
{

    public GameObject player;
    public GameObject _camera;
    public float cameraRange = 8;
    public float cameraHeight = 4;

	void Start ()
	{


        GameObject playerInstance = new GameObject();
        GameObject cameraInstance = new GameObject();

        if (!player.GetComponent<Rigidbody>()) { player.AddComponent<Rigidbody>();}
        if (player) { playerInstance  = Instantiate(player, transform.position, Quaternion.identity)as GameObject;}
        if (_camera)
        {
            cameraInstance = (GameObject)Instantiate(_camera, new Vector3(cameraRange, playerInstance.transform.position.y, playerInstance.transform.position.z), Quaternion.identity);
            cameraInstance.GetComponent<LevelCamera>().player = playerInstance;
            cameraInstance.transform.LookAt(transform);
            cameraInstance.GetComponent<LevelCamera>().cameraDistance = cameraRange;
            cameraInstance.GetComponent<LevelCamera>().cameraHeight = cameraHeight;
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
