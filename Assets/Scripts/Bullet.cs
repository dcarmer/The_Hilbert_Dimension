using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    private const float TIME_TO_LIVE = 2.0f;
    private static string hits = "NetworkedFPSController";
    private float remainingTime;
    private bool fired = false;
    public Gun gun;


    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name.Contains(hits))
        {
            Debug.Log("ded");
            //Put health stuff here
            //This means it hit a person
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
