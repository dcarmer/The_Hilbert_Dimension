using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class NetworkController : MonoBehaviour
{
    private void Awake()
    {
        PersistentController._NetworkController = this;        
    }

    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings(Application.version);
    }

    public void OnJoinedLobby()
    {
        RoomOptions _RoomOptions = new RoomOptions();
        _RoomOptions.MaxPlayers = 9;
        _RoomOptions.PlayerTtl = 5;
        _RoomOptions.EmptyRoomTtl = 5000;

        PhotonNetwork.JoinOrCreateRoom("Test Room", _RoomOptions, TypedLobby.Default);
    }

    private void OnJoinedRoom()
    {    
        PersistentController.ClearStatus();

        if (SceneManagerHelper.ActiveSceneName != "Main" && PhotonNetwork.connected)
        {
            PhotonNetwork.isMessageQueueRunning = false;
            SceneManager.LoadScene("Main");
        }            
    }
}
