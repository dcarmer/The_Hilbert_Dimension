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
        _RoomOptions.MaxPlayers = 5;
        _RoomOptions.PlayerTtl = 5;
        _RoomOptions.EmptyRoomTtl = 5000;

        PhotonNetwork.JoinOrCreateRoom("Test Room", _RoomOptions, TypedLobby.Default);
    }

    public void OnJoinedRoom()
    {    
        PersistentController.ClearStatus();

        if (SceneManagerHelper.ActiveSceneName != "Main" && PhotonNetwork.connected)
        {
            SceneManager.LoadScene("Main");
        }            
    }

    public void LogRooms()
    {
        Debug.Log("Current rooms: " + PhotonNetwork.GetRoomList().Length);
        foreach (RoomInfo curr in PhotonNetwork.GetRoomList())
        {
            Debug.Log(string.Format("   {0}", curr.ToString()));
        }
    }
}
