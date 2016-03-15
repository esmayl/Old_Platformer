using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelCamera : MonoBehaviour {

    public GameObject player;
    public Image hpBar;
    public Text hptext;
    public Image chargeBar;
    public Text scoreText;
    public float cameraDistance;

    internal int score = 0;

    float height = 0f;
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
            hptext.text = player.GetComponent<PlayerMovement>().hp * 10 + "%";
        }
        scoreText.text = score.ToString("000000000");

    }

    void FixedUpdate()
    {
        if (CompareHeight(player.transform))
        {
            height = Mathf.Lerp(height, player.transform.position.y, Time.deltaTime);
        }

        if (height < 2)
        {
            height = 2;
        }
        transform.position = new Vector3(cameraDistance, height, player.transform.position.z+3f );

    }

    bool CompareHeight(Transform tToCompare)
    {
        Vector3 diff = tToCompare.position - transform.position;



        if (Mathf.Abs(diff.y) > 4)
        {
            return true;
        }
        if (diff.y < 2)
        {
            height = Mathf.Lerp(height, player.transform.position.y, Time.deltaTime * 10);
            return true;
        }
        if (height < 1)
        {
            return false;
        }
        return false;
    }

    public void RemoveHP()
    {
        if (player.GetComponent<PlayerMovement>().hp > 0)
        {
            if (hptext)
            {
                hptext.text = player.GetComponent<PlayerMovement>().hp * 10 + "%";
            }
                hpBar.fillAmount = 0.2f+player.GetComponent<PlayerMovement>().hp / 10f*0.8f;
        }
    }

    public void AddHP()
    {
        if (player.GetComponent<PlayerMovement>().hp > 0)
        {
            if (hptext)
            {
                hptext.text = player.GetComponent<PlayerMovement>().hp * 10 + "%";
            }
            hpBar.fillAmount = 0.2f + player.GetComponent<PlayerMovement>().hp / 10f * 0.8f;
        }
    }

    public void AddMP()
    {
        if (player.GetComponent<PlayerMovement>().mp >= 0)
        {
            fading = true;
            StartCoroutine("FadeInMPBar");
        }
    }

    public void RemoveMP()
    {
        if (player.GetComponent<PlayerMovement>().mp >= 0)
        {
            fading = true;
            StartCoroutine("FadeOutMPBar");
        }
    }

    IEnumerator FadeOutMPBar()
    {
        if (fading) { yield return null; }
        while (chargeBar.fillAmount > player.GetComponent<PlayerMovement>().mp / 10f) { chargeBar.fillAmount -= Time.deltaTime; yield return new WaitForEndOfFrame(); }
        fading = false;
        StopCoroutine("FadeOutMPBar");
    }
    IEnumerator FadeInMPBar()
    {
        if (fading) { yield return null; }
        while (chargeBar.fillAmount < player.GetComponent<PlayerMovement>().mp / 10f) { chargeBar.fillAmount += Time.deltaTime; yield return new WaitForEndOfFrame(); }
        fading = false;
        StopCoroutine("FadeInMPBar");
    }

    public void AddScore()
    {
        if (player.GetComponent<PlayerMovement>().score >= 0)
        {
            scoreText.text = "" + score.ToString("000000000");
        }
    }
}
