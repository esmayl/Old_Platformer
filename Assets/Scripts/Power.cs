using UnityEngine;
using System.Collections;

public class Power : MonoBehaviour {

    public AudioSource bulletSource;
    public float range = 3;
    public Texture2D armorTexture;
    public Texture2D attackTexture;
    public GameObject gun;
    public ParticleSystem shotParticle;
    public AudioSource shotSource;
    public int speed;
    public AudioClip shotSound; 
    public int mpCost = 0;
    internal float value;
    internal GameObject instance;
    internal GameObject powerHolder;    

    

	// Use this for initialization
	public virtual void Start () {
        value = 0.1f;
	}

    public virtual void Attack(Transform player)
    {
     
    }

    public virtual bool Detonate()
    {
        return false;
    }
}
