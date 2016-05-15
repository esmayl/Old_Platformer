using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelCamera : MonoBehaviour {

    public GameObject player;
    public Image hpBar;
    public Text hptext;
    public Image chargeBar;
    public Text scoreText;

    internal float cameraDistance;
    internal float cameraHeight;
    internal int score = 0;

    Image[] hp = new Image[10];
    Image[] mp = new Image[10];
    RaycastHit hit;
    bool fading = false;
    Camera cam;

    void Start()
    {
        cam = transform.GetComponent<Camera>();

        if (hpBar)
        {
            hpBar.fillAmount = 1;
        }
        if (hptext)
        {
            hptext.text = player.GetComponent<PlayerBase>().hp * 10 + "%";
        }
        scoreText.text = score.ToString("000000000");

        transform.position = new Vector3(cameraDistance, player.transform.position.y + cameraHeight, Mathf.Lerp(transform.position.z, player.transform.position.z + 3f, 1));

    }

    void FixedUpdate()
    {
        if (CompareHeight(player.transform))
        {
            cameraHeight = Mathf.Lerp(cameraHeight, player.transform.position.y, Time.fixedDeltaTime);
        }



        transform.position = new Vector3(cameraDistance, player.transform.position.y+ cameraHeight, Mathf.Lerp(transform.position.z,player.transform.position.z+3f,Time.fixedDeltaTime));

    }

    bool CompareHeight(Transform tToCompare)
    {
        Vector3 diff = tToCompare.position - transform.position;



        if (Mathf.Abs(diff.y) > 4)
        {
            return true;
        }
        if (Mathf.Abs(diff.y) < 4)
        {
            return false;
        }
        return false;
    }

    public void RemoveHP()
    {
        if (player.GetComponent<PlayerBase>().hp > 0)
        {
            if (hptext)
            {
                hptext.text = player.GetComponent<PlayerBase>().hp * 10 + "%";
            }
                hpBar.fillAmount = 0.2f+player.GetComponent<PlayerBase>().hp / 10f*0.8f;
        }
    }

    public void AddHP()
    {
        if (player.GetComponent<PlayerBase>().hp > 0)
        {
            if (hptext)
            {
                hptext.text = player.GetComponent<PlayerBase>().hp * 10 + "%";
            }
            hpBar.fillAmount = 0.2f + player.GetComponent<PlayerBase>().hp / 10f * 0.8f;
        }
    }

    public void AddMP()
    {
        if (player.GetComponent<PlayerBase>().mp >= 0)
        {
            fading = true;
            StartCoroutine("FadeInMPBar");
        }
    }

    public void RemoveMP()
    {
        if (player.GetComponent<PlayerBase>().mp >= 0)
        {
            fading = true;
            StartCoroutine("FadeOutMPBar");
        }
    }

    IEnumerator FadeOutMPBar()
    {
        if (fading) { yield return null; }
        while (chargeBar.fillAmount > player.GetComponent<PlayerBase>().mp / 10f) { chargeBar.fillAmount -= Time.deltaTime; yield return new WaitForEndOfFrame(); }
        fading = false;
        StopCoroutine("FadeOutMPBar");
    }
    IEnumerator FadeInMPBar()
    {
        if (fading) { yield return null; }
        while (chargeBar.fillAmount < player.GetComponent<PlayerBase>().mp / 10f) { chargeBar.fillAmount += Time.deltaTime; yield return new WaitForEndOfFrame(); }
        fading = false;
        StopCoroutine("FadeInMPBar");
    }

    public void AddScore()
    {
        if (player.GetComponent<PlayerBase>().score >= 0)
        {
            scoreText.text = "" + score.ToString("000000000");
        }
    }
}
