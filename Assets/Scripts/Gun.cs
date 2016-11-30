using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Gun : MonoBehaviour {

    private const int NUM_OF_BULLETS = 50;
    private const float BULLET_SPEED = 20;
    private Stack<GameObject> clip;

    public void Load (GameObject bullet)
    {
        clip.Push(bullet);
    }

    void Shoot()
    {
        try
        {
            if (clip != null && clip.Peek() != null)
            {
                Vector3 rotation = this.gameObject.transform.forward;
                Vector3 position = this.gameObject.transform.position;

                GameObject bullet = (GameObject)clip.Pop();
                bullet.transform.position = position;
                bullet.transform.rotation.SetLookRotation(rotation * 3);
                bullet.GetComponent<Rigidbody>().velocity = rotation * BULLET_SPEED;
                bullet.GetComponent<Bullet>().Fire();
            }
        }
        catch (InvalidOperationException e) { }
    }

	// Use this for initialization
	void Start () {

    }

    public void SetTeam (int id)
    {
        Color c = ColorAlgorithm.GetColor(id);
        this.SetColor(c);
        for(int i = 0; i < NUM_OF_BULLETS; i++)
        {
            GameObject o = Resources.Load<GameObject>("Bullet");

            GameObject o2 = (GameObject)Instantiate(o);

            o2.GetComponent<Renderer>().material.color = c;
            o2.GetComponentInChildren<Light>().color = c;
            o2.GetComponent<Bullet>().gun = this;
            o2.layer = 22; //The bullet layer
            o2.transform.position = new Vector3(0, -10, 0);

            if (clip == null) clip = new Stack<GameObject>();
            clip.Push(o2);
        }
    }

    void SetColor(Color c)
    {
        this.gameObject.GetComponent<Renderer>().material.color = c;
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            this.Shoot();
        }
	}
}
