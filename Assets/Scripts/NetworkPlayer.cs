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

    private Vector3 realPos = Vector3.zero;
    private Quaternion realRot = Quaternion.identity;

    void Awake()
    {
        _PhotonView = GetComponent<PhotonView>();
    }

    void Start()
    {
        _Renderer = this.GetComponentInChildren<Renderer>();
        overviewCamera = GameObject.Find("OverviewCam");
        _Camera = GetComponentInChildren<Camera>();
        _AudioListener = GetComponentInChildren<AudioListener>();

        if (_PhotonView.isMine)
        {
            overviewCamera.SetActive(false);
        }
        GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = _PhotonView.isMine;
        _Camera.enabled = _PhotonView.isMine;
        _AudioListener.enabled = _PhotonView.isMine;
    }


    void Update()
    {
        if (!_PhotonView.isMine)
        {
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
