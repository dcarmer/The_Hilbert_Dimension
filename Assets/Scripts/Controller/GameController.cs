using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    public static GameController _GameController;
    public ColorAlgorithm _ColorAlgorithm = new ColorAlgorithm();
    public List<NetworkPlayer> players;
    public int playerCount = 0;

    private void Start()
    {
        if (_GameController != null)
        {
            GameObject.Destroy(this.gameObject);
            return;
        }
        _GameController = this;

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
        PhotonNetwork.Instantiate("NetworkedFPSController", new Vector3(Random.Range(-10, 10), 2, Random.Range(-10, 10)), Quaternion.identity, 0);
    }
}
