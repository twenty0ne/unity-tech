using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuControl : MonoBehaviour {

    public static MenuControl instance = null;

    [SerializeField] private Button btnCreate;
    [SerializeField] private Button btnJoin;
    [SerializeField] private Button btnStart;
    [SerializeField] private GameObject panelHost;
    [SerializeField] private Text txtNumber;
    [SerializeField] private Text txtStatus;

    private int curNum = 0;
    private int maxNum = XNetManager.MAX_CONNECTIONS;

    private void Start ()
    {
        instance = this;

        panelHost.SetActive(false);

        txtStatus.text = "";
    }

    private void OnDestroy()
    {
    }

    private void OnEnable()
    {
        XNetManager.playerJoined += OnPlayerJoined;
    }

    private  void OnDisable()
    {
        XNetManager.playerJoined -= OnPlayerJoined;
    }

    public void OnCreateClick()
    {
        btnCreate.enabled = false;
        btnJoin.enabled = false;

        panelHost.SetActive(true);
        btnStart.enabled = false;

        curNum = 1;
        txtNumber.text = curNum.ToString() + "/" + maxNum.ToString();

        XNetManager.instance.CreateHost();
    }

    public void OnJoinClick()
    {
        btnCreate.enabled = false;
        btnJoin.enabled = false;

        panelHost.SetActive(false);

        XNetManager.instance.CreateClient();
    }

    public void OnStartClick()
    {
        XNetManager.instance.StartGame();
    }

    public void OnPlayerJoined(bool success)
    {
        curNum += 1;
        btnStart.enabled = (curNum >= 2);

        txtNumber.text = curNum.ToString() + "/" + maxNum.ToString();
    }

    public void SetStatusText(string txt)
    {
        txtStatus.text = txt;
    }
}
