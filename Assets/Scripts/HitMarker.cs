using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HitMarker : MonoBehaviour {

    private const float DELAY = 5.0f;

	// Use this for initialization
	void Start () {
        Image component = this.GetComponent<Image>();
        Color color = component.color;
        color.a = 0;
        component.color = color;
    }
	
	// Update is called once per frame
	void Update () {
        Image component = this.GetComponent<Image>();
        Color color = component.color;
        if (color.a > 0)
        {
            color.a -= Time.deltaTime * DELAY;
            if (color.a < 0) color.a = 0;
            component.color = color;
        }
	}
}
