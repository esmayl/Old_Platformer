using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public enum Levels
{
    Scifi = 0,
    Wood = 1
}

public class LevelSwap : MonoBehaviour
{
    public Levels levelToSwitchTo;

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            switch (levelToSwitchTo)
            {
                    case Levels.Scifi:
                                    SceneManager.LoadScene(1);
                                    break;
                    case Levels.Wood:
                                    SceneManager.LoadScene(2);
                                    break;
            }
        }       
    }

    public void Swap()
    {
        switch (levelToSwitchTo)
        {
            case Levels.Scifi:
                SceneManager.LoadScene(1);
                break;
            case Levels.Wood:
                SceneManager.LoadScene(2);
                break;
        }
    }

    public void SetLevel(string levelName)
    {
        if (levelName.ToLower().Contains("scifi")) { levelToSwitchTo = Levels.Scifi;}
        if (levelName.ToLower().Contains("bob")) { levelToSwitchTo = Levels.Wood;}
    }

}
