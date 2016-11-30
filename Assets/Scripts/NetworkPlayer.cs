using UnityEngine;
using UnityEngine.UI;

public class NetworkPlayer : MonoBehaviour
{
    public static int mainID;
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
        id = (int)_PhotonView.owner.customProperties["ID"] + 1;
        mainID = id;

        _Renderer = this.GetComponentInChildren<Renderer>();
        colorIndicator = GameObject.Find("ColorIndicator").GetComponent<Image>();
        overviewCamera = GameObject.Find("OverviewCam");
        _Camera = GetComponentInChildren<Camera>();
        _AudioListener = GetComponentInChildren<AudioListener>();

        _Renderer.material.color = ColorAlgorithm.GetColor(id);
        //Debug.Log(this.gameObject.layer);
        this.gameObject.layer = LayerMask.NameToLayer("Player " + (id+1));
        //Debug.Log(this.gameObject.layer);

        teamCamera(_Camera, id);

        if (_PhotonView.isMine)
        {
            overviewCamera.SetActive(false);
            colorIndicator.color = ColorAlgorithm.GetColor(id);
        }

        GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = _PhotonView.isMine;
        _Camera.enabled = _PhotonView.isMine;
        _AudioListener.enabled = _PhotonView.isMine;

        GameObject gun1 = Resources.Load<GameObject>("Gun");
        GameObject gun2 = (GameObject)Instantiate(gun1);

        //playerID = player.GetComponent<NetworkPlayer>().id;
        //playerID = PhotonView.owner.customProperties["ID"] + 1;

        Vector3 pos1 = this.transform.position;

        gun2.transform.position = pos1 + this.transform.forward * 2;

        gun2.transform.parent = this.GetComponentInChildren<Camera>().transform;
        gun2.GetComponent<Gun>().SetTeam(id);


        GameController.GenerateLevel(id);
    }

    private const int teamoffset = 8;
    private const int walloffset = 15;
    private const int numberOfTeams = 7;
    private static void teamCamera(Camera c, int team)
    {
        int currentTeamsWalls = team + walloffset;
        for (int i = walloffset; i < numberOfTeams + walloffset; i++)
        {
            if(i != currentTeamsWalls) hideLayer(c, LayerMask.LayerToName(i));
        }
    }

    private static void hideLayer(Camera c, int layer)
    {
        c.cullingMask |= layer;
    }

    private static void showLayer(Camera c, int layer)
    {
        c.cullingMask &= ~layer;
    }

    private static void hideLayer(Camera c, string layer)
    {
        hideLayer(c, 1 << LayerMask.NameToLayer(layer));
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
