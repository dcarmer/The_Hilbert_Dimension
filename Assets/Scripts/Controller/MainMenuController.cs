using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    string oldState;

    private void Start()
    {
        GameObject.Find("lblTitle").GetComponent<Text>().text = Application.productName;
        GameObject.Find("lblVersion").GetComponent<Text>().text = string.Format("Version: {0}", Application.version);
    }

    private void Update()
    {
        // TODO: There has to be a better way of doing this...
        string curr = PhotonNetwork.connectionStateDetailed.ToString();

        if (curr != oldState)
        {
            PersistentController.AddStatus(curr);
        }

        oldState = curr;
    }

    public void btnPlay_Click()
    {
        PersistentController._NetworkController.Connect();       
    }

}
