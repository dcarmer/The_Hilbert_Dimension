using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnJoinedRoom()
    {
        CreatePlayer();
    }

    public void CreatePlayer()
    {
        Debug.Log("Creating player.");
    }
}
