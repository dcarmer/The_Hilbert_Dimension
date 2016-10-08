using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    private void Start()
    {
        GameObject.Find("lblTitle").GetComponent<Text>().text = Application.productName;
        GameObject.Find("lblVersion").GetComponent<Text>().text = string.Format("Version: {0}", Application.version);
    }

    public void btnPlay_Click()
    {
        PersistentController._NetworkController.Connect();
    }
}
