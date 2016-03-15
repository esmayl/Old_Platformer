using UnityEngine;
using System.Collections;

public class Megaman : PlayerMovement {

    public static Megaman playerRef;

    public KeyCombo combo1;
    public KeyCombo combo2;
    public GameObject[] armorObjects;
    public ParticleSystem chargeParticles;

    internal AudioSource sourceAttack1;
    internal AudioSource sourceAttack2;
    internal AudioSource sourceAttack3;
    
    Armor[] armorPieces;

    int chargeLevel = 0;
    GameObject chargePInstance;
    float chargeCounter = 0;
    float chargeTime = 0.5f;
    float holdTimer;

    void Awake()
    {
        playerRef = this;
    }

    public override void Start()
    {
        base.Start();

        var temp = chargeParticles.emission; 
        temp.enabled = false;
        chargePInstance = Instantiate(chargeParticles).gameObject;

        transform.name = "Player" + playerNumber;

        armorPieces = new Armor[armorObjects.Length];
        for (int i = 0; i < armorObjects.Length; i++)
        {
            armorPieces[i] = armorObjects[i].GetComponent<Armor>();
        }

        sourceAttack1 = transform.FindChild("AttackSource 1").GetComponent<AudioSource>();
        sourceAttack2 = transform.FindChild("AttackSource 2").GetComponent<AudioSource>();
        sourceAttack3 = transform.FindChild("AttackSource 3").GetComponent<AudioSource>();


        for (int i = 0; i < armorPieces.Length; i++)
        {
            powers[i] = armorPieces[i].power;
        }
        base.Start();

        if (powers[0])
        {
            powerHolder1 = Instantiate(powers[0]).gameObject;
        }
        if (powers[1])
        {
            powerHolder2 = Instantiate(powers[1]).gameObject;
        }
        if (powers[2])
        {
            powerHolder3 = Instantiate(powers[2]).gameObject;
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();
        //pointLight.transform.position = transform.position + transform.up*lightHeight;

        if (attack)
        {
            if (!powers[0]) { return; }

            if (powerHolder2.GetComponent<Power>().Detonate()) { return; }
            if (powerHolder3.GetComponent<Power>().Detonate()) { return; }

            chargeLevel = 0;
            transition = true;

            powerHolder1.GetComponent<Power>().shotSource = sourceAttack1;
            powerHolder1.GetComponent<Buster>().chargeLevel = chargeLevel;
            powerHolder1.GetComponent<Power>().Attack(transform);

            holdTimer = 0;
            StopCoroutine("Charge");
            anim.SetTrigger("Attack");

            attack = false;
        }

        if (useMobileInput) { return; }

        if (combo1.Check())
        {
            if (!powers[1]) { return; }
            if (UseMP(powerHolder2.GetComponent<Power>().mpCost))
            {
                transition = true;
                powerHolder2.GetComponent<Power>().shotSource = sourceAttack2;
                powerHolder2.GetComponent<Power>().bulletSource = sourceAttack3;
                powerHolder2.GetComponent<Power>().Attack(transform);
			    anim.SetTrigger("Attack");
                return;
            }
        }

        if (combo2.Check())
        {
            if (!powers[2]) { return; }

            if (UseMP(powerHolder3.GetComponent<Power>().mpCost))
            {
                transition = true;
                powerHolder3.GetComponent<Power>().shotSource = sourceAttack2;
                powerHolder3.GetComponent<Power>().bulletSource = sourceAttack3;
                powerHolder3.GetComponent<Power>().Attack(transform);
			    anim.SetTrigger("Attack");

                return;
            }
        }

        if (Input.GetButtonDown("Fire1"))
        {

            if (!powers[0]) { return; }

            if (powerHolder2.GetComponent<Power>().Detonate()) { return; }
            if (powerHolder3.GetComponent<Power>().Detonate()) { return; }

            chargeLevel = 0;
            transition = true;

            powerHolder1.GetComponent<Power>().shotSource = sourceAttack1;
            powerHolder1.GetComponent<Buster>().chargeLevel = chargeLevel;
            powerHolder1.GetComponent<Power>().Attack(transform);

            holdTimer = 0;
            StopCoroutine("Charge");
			anim.SetTrigger("Attack");
        }
        if (Input.GetButton("Fire1"))
        {
                holdTimer += Time.deltaTime;
                

                if (chargeLevel > 3) { chargeLevel = 3; }
                else
                {
                    if (chargeCounter > chargeTime)
                    {
                        ShowChargeLevel();
                        chargeLevel++;
                        chargeCounter = 0;
                    }
                    else
                    {
                        chargeCounter += Time.deltaTime;
                    }
                }
            
        }
        if (Input.GetButtonUp("Fire1"))
        {
            if (holdTimer > 0.5f)
            {
                powerHolder1.GetComponent<Buster>().chargeLevel = chargeLevel;
                powerHolder1.GetComponent<Power>().shotSource = sourceAttack1;
                powerHolder1.GetComponent<Power>().Attack(transform);
                chargePInstance.SetActive(false);
            }
                transition = true;
                holdTimer = 0;
                chargeLevel = 0;
                
                powerHolder1.GetComponent<Buster>().chargeLevel = chargeLevel;
                StopCoroutine("Charge");
        }

    }

    public void ShowChargeLevel()
    {
        Debug.Log(chargeLevel);

        switch (chargeLevel)
        {
            case 0:
                break;
            case 1:
                StartCoroutine("Charge", Color.blue);
                break;
            case 2:
                StartCoroutine("Charge", Color.cyan);
                break;
            case 3:
                StartCoroutine("Charge", Color.green);
                break;
        }
    }

    public IEnumerator Charge(Color chargeColor)
    {
        chargePInstance.SetActive(true);

        chargePInstance.transform.position = transform.position+transform.up;

        var temp = chargePInstance.GetComponent<ParticleSystem>().emission;
        temp.enabled = true;

        chargePInstance.GetComponent<ParticleSystem>().startColor = chargeColor;

        yield return new WaitForEndOfFrame();
        StartCoroutine("Charge",chargeColor);
    }



}
