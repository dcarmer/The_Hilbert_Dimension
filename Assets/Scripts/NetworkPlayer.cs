using UnityEngine;
using UnityEngine.UI;

public class NetworkPlayer : MonoBehaviour
{
    public int id = -1;

    private GameObject overviewCamera;
    private Camera _Camera;
    private AudioListener _AudioListener;
    private Renderer _Renderer;
    private PhotonView _PhotonView;

    private Image colorIndicator;

    private Vector3 realPos = Vector3.zero;
    private Quaternion realRot = Quaternion.identity;

    private void Awake()
    {
        _PhotonView = GetComponent<PhotonView>();

        if (_PhotonView.isMine)
        {
            ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable();
            props.Add("ID", PhotonNetwork.room.playerCount - 1);
            PhotonNetwork.player.SetCustomProperties(props);
            PersistentController.AddStatus(string.Format("You joined with ID {0}", PhotonNetwork.player.customProperties["ID"]));
        }
    }

    private void Start()
    {
        id = (int)_PhotonView.owner.customProperties["ID"];

        _Renderer = this.GetComponentInChildren<Renderer>();
        colorIndicator = GameObject.Find("ColorIndicator").GetComponent<Image>();
        overviewCamera = GameObject.Find("OverviewCam");
        _Camera = GetComponentInChildren<Camera>();
        _AudioListener = GetComponentInChildren<AudioListener>();

        _Renderer.material.color = ColorAlgorithm.GetColor(id);

        if (_PhotonView.isMine)
        {
            overviewCamera.SetActive(false);
            colorIndicator.color = ColorAlgorithm.GetColor(id);
        }

        GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = _PhotonView.isMine;
        _Camera.enabled = _PhotonView.isMine;
        _AudioListener.enabled = _PhotonView.isMine;
    }


    private void Update()
    {
        if (!_PhotonView.isMine)
        {
            if (Vector3.Distance(transform.position, realPos) > 10.0f)
            {
                transform.position = realPos;
            }
            transform.position = Vector3.Lerp(transform.position, realPos, Time.deltaTime * 10f);
            transform.rotation = Quaternion.Lerp(transform.rotation, realRot, Time.deltaTime * 10f);
        }
    }

    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
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
