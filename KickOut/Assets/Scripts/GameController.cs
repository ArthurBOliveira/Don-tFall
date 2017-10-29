using SocketIO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameController : MonoBehaviour
{
    public GameObject bombPower;
    public GameObject[] spawnPositions;

    public GameObject mainGame;
    public RectTransform chooseUI;

    public GameObject player;
    public GameObject onlinePlayer;

    public InputField txtName;
    public InputField txtRoom;

    public bool isOnline;
    public bool isPlaying = false;

    public Text[] scoresTxts;

    private SocketIOComponent socket;

    private string playerName;
    private string currentRoom;

    private int players;

    #region Private
    private void Awake()
    {
        isPlaying = false;
        socket = GameObject.FindGameObjectWithTag("Socket").GetComponent<SocketIOComponent>();
    }

    private void Start()
    {
        socket.On("newPlayerServer", SetUpNewPlayer);
        socket.On("currentPlayers", SetUpCurrentPlayers);
        SetUpGame();
    }

    private IEnumerator PowersSpawner()
    {
        Vector3 position1;
        Vector3 position2;

        while (true)
        {
            float x1 = UnityEngine.Random.Range(-17.1f, -25.1f);
            float z1 = UnityEngine.Random.Range(-14.1f, 14.1f);
            position1 = new Vector3(x1, 0, z1);

            float x2 = UnityEngine.Random.Range(17.1f, 25.1f);
            float z2 = UnityEngine.Random.Range(-14.1f, 14.1f);
            position2 = new Vector3(x2, 0, z2);

            Destroy(Instantiate(bombPower, position1, bombPower.transform.rotation), 10f);
            Destroy(Instantiate(bombPower, position2, bombPower.transform.rotation), 10f);

            yield return new WaitForSeconds(20f);
        }
    }

    private void SetUpGame()
    {
        if (!isOnline) return;

        players = 1;

        mainGame.SetActive(false);
        chooseUI.gameObject.SetActive(true);
    }

    private void SetUpCurrentPlayers(JSONObject obj)
    {
        string[] players = obj[0].ToString().Replace("\"", "").Split(',');

        Debug.Log(players);

        for (int i = 0; i < players.Length; i++)
        {
            string p = players[i];

            Debug.Log(p);

            if (p != "null")
                InstatiateNewPlayer(p, currentRoom, new Vector3());
        }
    }

    private void InstatiateNewPlayer(string _name, string _room, Vector3 position)
    {
        GameObject mp = Instantiate(onlinePlayer, position, onlinePlayer.transform.rotation);
        mp.GetComponent<OnlinePlayer>().NAME = _name;
        mp.GetComponent<OnlinePlayer>().ROOM = _room;
        mp.GetComponent<OnlinePlayer>().DEATHS = 0;

        #region Set up score texts
        if (players < 9)
        {
            mp.GetComponent<OnlinePlayer>().txtScore = scoresTxts[players];

            players++;
        }
        #endregion
    }
    #endregion

    #region Public
    public void SetUpNewPlayer(SocketIOEvent obj)
    {
        Dictionary<string, string> data = obj.data.ToDictionary();
        Debug.Log(data);

        string room = data["room"];
        string _name = data["name"];
        if (room != currentRoom || playerName == _name) return;

        float x = float.Parse(data["x"]);
        float y = float.Parse(data["y"]);
        float z = float.Parse(data["z"]);

        Vector3 spawn = new Vector3(x, y, z);

        //Instantiate localy
        GameObject mp = Instantiate(onlinePlayer, spawn, onlinePlayer.transform.rotation);
        mp.GetComponent<OnlinePlayer>().NAME = _name;
        mp.GetComponent<OnlinePlayer>().ROOM = room;

        #region Set up score texts
        if (players < 9)
        {
            mp.GetComponent<OnlinePlayer>().txtScore = scoresTxts[players];

            players++;
        }
        #endregion
    }

    public void SetUpCurrentPlayers(SocketIOEvent obj)
    {
        Debug.Log(obj);

        //Dictionary<string, string> data = obj.data.ToDictionary();

        //Debug.Log(data);
    }

    public void EmitNewPlayer()
    {
        playerName = txtName.text;
        currentRoom = txtRoom.text;
        isPlaying = true;

        mainGame.SetActive(true);
        chooseUI.gameObject.SetActive(false);

        int rnd = UnityEngine.Random.Range(0, 4);

        GameObject spawn = spawnPositions[rnd];

        //Instantiate localy
        GameObject mp = Instantiate(player, spawn.transform.position, player.transform.rotation);
        mp.GetComponent<Online>().NAME = playerName;
        mp.GetComponent<Online>().ROOM = currentRoom;

        #region Join Room
        JSONObject _player;

        Dictionary<string, string> data = new Dictionary<string, string>();
        data["x"] = spawn.transform.position.x.ToString();
        data["y"] = spawn.transform.position.y.ToString();
        data["z"] = spawn.transform.position.z.ToString();
        data["name"] = playerName;
        data["room"] = currentRoom;

        _player = new JSONObject(data);

        socket.Emit("join", _player, SetUpCurrentPlayers);
        gameObject.SetActive(true);
        #endregion

        StartCoroutine(PowersSpawner());
    }
    #endregion
}