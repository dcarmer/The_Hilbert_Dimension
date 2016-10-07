﻿using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text;

public class PersistentController : MonoBehaviour
{
    private static PersistentController _PersistentController;

    private static List<string> statusList;
    private static Text lblStatus;

    private const float STATUS_TIMEOUT = 15.0f; // Seconds until status messages are removed.

    private void Awake()
    {
        if (_PersistentController != null)
        {
            GameObject.Destroy(this.gameObject);
            return;
        }

        Debug.Log(string.Format("\"{0}\" version {1} started.", Application.productName, Application.version));

        GameObject.DontDestroyOnLoad(this);
        
        PersistentController._PersistentController = this;

        PersistentController.statusList = new List<string>();
        PersistentController.lblStatus = transform.FindChild("lblStatus").GetComponent<Text>();
        PersistentController.lblStatus.text = string.Empty;
    }

    /// <summary>
    /// Add a temporary status message visible to the user.   
    /// </summary>
    /// <param name="msg">Statys message.</param>
    public static void AddStatus(string msg)
    {
        Debug.Log(string.Format("Status: {0}", msg));
        statusList.Insert(0, msg);
        UpdateStatusLabel();

        _PersistentController.Invoke("RemoveLastStatus", STATUS_TIMEOUT);
    }

    /// <summary>
    /// Clear all status messages.
    /// </summary>
    public static void ClearStatus()
    {
        statusList.Clear();
        UpdateStatusLabel();
    }

    private static void UpdateStatusLabel()
    {
        StringBuilder _StringBuilder = new StringBuilder();

        foreach (string curr in statusList)
        {
            _StringBuilder.Append("\n");
            _StringBuilder.Append(curr);
        }

        PersistentController.lblStatus.text = _StringBuilder.ToString();
    }
   
    private void RemoveLastStatus()
    {
        statusList.RemoveAt(statusList.Count - 1);
        UpdateStatusLabel();
    }

    public void QuitGame()
    {
        Debug.Log("Closing game.");
        Application.Quit();
    }
}