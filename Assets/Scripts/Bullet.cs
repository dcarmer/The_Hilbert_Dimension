using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Bullet : MonoBehaviour {

    private const float TIME_TO_LIVE = 1.0f;
    private static string hits = "NetworkedFPSController";
    private float remainingTime;
    private bool fired = false;
    public Gun gun;
    //public GameObject hitmarker = GameObject.Find("HitMarker");


    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name.Contains(hits))
        {
            Debug.Log("ded");
            //Put health stuff here
            //This means it hit a person
            this.GetComponent<AudioSource>().Play();
            Color color = GameObject.Find("HitMarker").GetComponent<Image>().color;
            color.a = 1.0f;
            GameObject.Find("HitMarker").GetComponent<Image>().color = color;
        }
        Return();
    }

    public void Fire ()
    {
        remainingTime = TIME_TO_LIVE;
        fired = true;
    }

    void Return ()
    {
        fired = false;
        remainingTime = 1;
        if(gun != null)
        {
            gun.GetComponent<Gun>().Load(this.gameObject);
        }
    }

    void SetColor (Color c)
    {
        this.gameObject.GetComponent<Renderer>().material.color = c;
        this.gameObject.GetComponent<Light>().color = c;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(fired)
        {
            remainingTime -= Time.deltaTime;
            if(remainingTime <= 0)
            {
                this.Return();
            }
        }
	}
}
