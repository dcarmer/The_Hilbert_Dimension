using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("Game controller.");
        PhotonNetwork.isMessageQueueRunning = true;
        if (!PhotonNetwork.connected)
        {
            SceneManager.LoadScene("MainMenu");

            if (!Application.isEditor)
            {
                PersistentController.AddStatus("Attempted to start game with no connection!", true);
            }
            return;
        }

        CreatePlayer();
    }

    public void CreatePlayer()
    {
        Debug.Log("Creating player.");
        PhotonNetwork.Instantiate("NetworkedFPSController", new Vector3(0, 2, 0), Quaternion.identity, 0);
    }


}
