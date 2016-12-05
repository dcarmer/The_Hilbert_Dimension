using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkPlayer : MonoBehaviour
{
    private const float MAXIMUM_HEALTH = 100.0f;
    private const float HEAL_AMOUNT = 5.0f;
    private const float HEAL_DELAY = 4.0f;
    private const float INVULN_FRAMES = 2.0f;
    private const float DAMAGE = 10.0f;
    private const float RESPAWN_DELAY = 5.0f;

    private static Dictionary<int, NetworkPlayer> players = new Dictionary<int, NetworkPlayer>();

    public static int mainID;
    public int id = -1;

    private float respawntime = 0;

    private float healthDelay = 0;
    private float health;
    private bool invulnurable = true;
    private float iframes = INVULN_FRAMES;

    private GameObject gun;
    private GameObject overviewCamera;
    private Camera _Camera;
    private AudioListener _AudioListener;
    private Renderer _Renderer;
    private PhotonView _PhotonView;

    private GameObject gui;

    private Image colorIndicator;

    private Vector3 realPos = Vector3.zero;
    private Quaternion realRot = Quaternion.identity;

    public void Hit(int id)
    {
        if (invulnurable) return;
        healthDelay = HEAL_DELAY;

        health -= DAMAGE;
        UpdateHealth();
        if (health <= 0) Die(id);
    }

    public void Die(int id) {
        GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false; //stop moving
        Leaderboards.ReportKill(id, this.id);
        PhotonNetwork.RaiseEvent(2, new byte[] {(byte)id, (byte)this.id }, true, null);
        invulnurable = true;
        respawntime = RESPAWN_DELAY;
        Color c = this.gameObject.GetComponent<Renderer>().material.color;
        c.a = 0;
        this.gameObject.GetComponent<Renderer>().material.color = c;
    }

    public void Respawn()
    {
        Vector3 loc = new Vector3(Random.Range(-10, 10), 2, Random.Range(-10, 10));
        this.gameObject.transform.position = loc;
        GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;
        iframes = INVULN_FRAMES;
        health = MAXIMUM_HEALTH;
        invulnurable = true;
        UpdateHealth();
        Color c = this.gameObject.GetComponent<Renderer>().material.color;
        c.a = 1;
        this.gameObject.GetComponent<Renderer>().material.color = c;
    }

    private void UpdateHealth()
    {
        gui.GetComponent<Text>().text = "Health: " + health;
    }

    private void Awake()
    {
        _PhotonView = GetComponent<PhotonView>();
        if (_PhotonView.isMine)
        {
            ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable();
            props.Add("ID", PhotonNetwork.room.playerCount - 1);
            PhotonNetwork.player.SetCustomProperties(props);
            PhotonNetwork.OnEventCall += this.OnEvent;
            PersistentController.AddStatus(string.Format("You joined with ID {0}", PhotonNetwork.player.customProperties["ID"]));
            Leaderboards.id = (int)PhotonNetwork.player.customProperties["ID"];
            mainID = (int)PhotonNetwork.player.customProperties["ID"];
            this.gui = GameObject.Find("Health");
            health = MAXIMUM_HEALTH;
            UpdateHealth();
        }
    }

    private void Start()
    {
        id = (int)_PhotonView.owner.customProperties["ID"];
        players[id] = this;

        _Renderer = this.GetComponentInChildren<Renderer>();
        colorIndicator = GameObject.Find("ColorIndicator").GetComponent<Image>();
        overviewCamera = GameObject.Find("OverviewCam");
        _Camera = GetComponentInChildren<Camera>();
        _AudioListener = GetComponentInChildren<AudioListener>();


        //Debug.Log("ID: " + id);
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
        this.gun = gun2;
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
            if(i != currentTeamsWalls) showLayer(c, i);
        }
        //c.cullingMask = 1 << 8;
        Debug.Log("Culling Mask: " + c.cullingMask);
    }

    private static void hideLayer(Camera c, int layer)
    {
        c.cullingMask |= 1 << layer;
    }

    private static void showLayer(Camera c, int layer)
    {
        c.cullingMask &= ~(1 << layer);
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
        else if (respawntime > 0)
        {
            respawntime -= Time.deltaTime;
            if (respawntime <= 0)
            {
                this.Respawn();
            }
        }
        else if(invulnurable)
        {
            iframes -= Time.deltaTime;
            if (iframes <= 0) invulnurable = false;
        }
        else if(healthDelay > 0)
        {
            healthDelay -= Time.deltaTime;
        }
        else if(health < MAXIMUM_HEALTH)
        {
            health += HEAL_AMOUNT;
            if (health > MAXIMUM_HEALTH) health = MAXIMUM_HEALTH;
            UpdateHealth();
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

    public static void Shoot(int id)
    {
        PhotonNetwork.RaiseEvent(0, new byte[] {(byte)id }, true, null);
    }

    public static void NetworkHit(int hit, int id)
    {
        Debug.Log("Hits: " + hit + " " + id);
        PhotonNetwork.RaiseEvent(1, new byte[] {(byte)id, (byte)hit}, true, null);
        //Debug.Log("After Hits: " + hit + " " + id);
    }

    private void OnEvent(byte eventcode, object content, int senderid)
    {
        //if (this.id != mainID) return;
       
        byte[] c = (byte[])content;
        //Debug.Log("Event Triggered: " + eventcode + " " + c[0]);
        if (eventcode == 0) //Player shot
        {

            NetworkPlayer sender;
            players.TryGetValue(c[1], out sender);
            sender.gun.GetComponent<Gun>().NetworkShoot();
            //return;
        }
        if (eventcode == 1) //Player was hit
        {
            //Debug.Log("Not yet Hits: " + c[1] + " " + c[0]);
            if (c[1] == mainID)
            {
                //Debug.Log("Hits: " + c[1] + " " + c[0]);
                this.Hit(c[0]);
            }
            return;
        }
        if (eventcode == 2) //Player was killed
        {
            Leaderboards.ReportKill(c[0], c[1]);
            return;
        }
    }
}
