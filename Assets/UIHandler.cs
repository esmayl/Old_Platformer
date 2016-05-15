using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIHandler : MonoBehaviour
{
    public GameObject buttonWindow;
    public Button startGameButton;
    public GameObject levelSelectButton;

	void Start ()
	{
	    int i = 1;

        AsyncOperation tempScene;

        for (int j = 1; j < SceneManager.sceneCountInBuildSettings; j++)
        {
            tempScene = SceneManager.LoadSceneAsync(j);
            tempScene.allowSceneActivation = false;
        }

        while (i < SceneManager.sceneCount)
	    {
	        GameObject temp = Instantiate(levelSelectButton) as GameObject;

	        temp.GetComponentInChildren<Text>().text = SceneManager.GetSceneAt(i).name;
            temp.GetComponent<LevelSwap>().SetLevel(SceneManager.GetSceneAt(i).name);

	        temp.transform.SetParent(buttonWindow.transform);
            temp.transform.localScale = Vector3.one;

            i++;
	    }
        
	}

    public void SwitchToLevelselect()
    {
        buttonWindow.transform.parent.gameObject.SetActive(true);
        startGameButton.gameObject.SetActive(false);
    }

}
