using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NetworkPlayer : MonoBehaviour
{
    private GameObject overviewCamera;
    private Camera _Camera;
    private AudioListener _AudioListener;
    private Renderer _Renderer;
    private PhotonView _PhotonView;

    private Image colorIndicator;

    public int id = -1;

    private Vector3 realPos = Vector3.zero;
    private Quaternion realRot = Quaternion.identity;

    void Awake()
    {
        _PhotonView = GetComponent<PhotonView>();
        
        //PersistentController.AddStatus(string.Format("{0} joined with ID {1}", _PhotonView.isMine ? "You" : "Player", PhotonNetwork.player.customProperties["ID"]));
        if (_PhotonView.isMine)
        {
            ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable();
            props.Add("ID", PhotonNetwork.room.playerCount - 1);
            PhotonNetwork.player.SetCustomProperties(props);
            PersistentController.AddStatus("You joined with ID " + PhotonNetwork.player.customProperties["ID"]);
        }
    }

    void Start()
    {
        _Renderer = this.GetComponentInChildren<Renderer>();
        colorIndicator = GameObject.Find("ColorIndicator").GetComponent<Image>();
        overviewCamera = GameObject.Find("OverviewCam");
        _Camera = GetComponentInChildren<Camera>();
        _AudioListener = GetComponentInChildren<AudioListener>();

        id= (int)_PhotonView.owner.customProperties["ID"];
        _Renderer.material.color = GameController._GameController._ColorAlgorithm.GetColor(id);

        if (_PhotonView.isMine)
        {
            overviewCamera.SetActive(false);
            colorIndicator.color = GameController._GameController._ColorAlgorithm.GetColor(id);
        }
        GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = _PhotonView.isMine;
        _Camera.enabled = _PhotonView.isMine;
        _AudioListener.enabled = _PhotonView.isMine;
    }


    void Update()
    {         

        if (!_PhotonView.isMine)
        {
            if(Vector3.Distance(transform.position, realPos) > 10.0f)
            {
                transform.position = realPos;
            }
            transform.position = Vector3.Lerp(transform.position, realPos, Time.deltaTime * 10f);
            transform.rotation = Quaternion.Lerp(transform.rotation, realRot, Time.deltaTime * 10f);
        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {            
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {   
            realPos = (Vector3)(stream.ReceiveNext());
            realRot = (Quaternion)(stream.ReceiveNext());
        }
    }
}
