using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        CreatePlayer();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CreatePlayer()
    {
        Debug.Log("Creating player.");
    }
}
