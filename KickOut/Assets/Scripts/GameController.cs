using SocketIO;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject bombPower;
    public GameObject[] spawnPositions;

    public GameObject mainGame;
    public RectTransform chooseUI;

    public GameObject player;
    public GameObject onlinePlayer;

    public InputField txtName;
    public bool isOnline;

    private SocketIOComponent socket;

    #region Private
    private void Awake()
    {
        socket = GameObject.FindGameObjectWithTag("Socket").GetComponent<SocketIOComponent>();
        socket.On("newPlayerServer", SetUpNewPlayer);
    }

    private void Start()
    {
        StartCoroutine(PowersSpawner());
        SetUpGame();
    }

    private IEnumerator PowersSpawner()
    {
        Vector3 position1;
        Vector3 position2;

        while (true)
        {
            float x1 = Random.Range(-17.1f, -25.1f);
            float z1 = Random.Range(-14.1f, 14.1f);
            position1 = new Vector3(x1, 0.5f, z1);

            float x2 = Random.Range(17.1f, 25.1f);
            float z2 = Random.Range(-14.1f, 14.1f);
            position2 = new Vector3(x2, 0.5f, z2);

            Destroy(Instantiate(bombPower, position1, Quaternion.identity), 10f);
            Destroy(Instantiate(bombPower, position2, Quaternion.identity), 10f);

            yield return new WaitForSeconds(20f);
        }
    }

    private void SetUpGame()
    {
        if (!isOnline) return;

        mainGame.SetActive(false);
        chooseUI.gameObject.SetActive(true);
    }
    #endregion

    #region Public
    public void SetUpNewPlayer(SocketIOEvent obj)
    {
        Dictionary<string, string> data = obj.data.ToDictionary();

        string name = data["name"];
        float x = float.Parse(data["x"]);
        float y = float.Parse(data["y"]);
        float z = float.Parse(data["z"]);

        Vector3 spawn = new Vector3(x, y, z);

        //Instantiate localy
        GameObject mp = Instantiate(onlinePlayer, spawn, onlinePlayer.transform.rotation);
        mp.GetComponent<OnlinePlayer>().NAME = name;

    }

    public void EmitNewPlayer()
    {
        string name = txtName.text;

        mainGame.SetActive(true);
        chooseUI.gameObject.SetActive(false);

        int rnd = Random.Range(0, 4);

        GameObject spawn = spawnPositions[rnd];

        //Instantiate localy
        GameObject mp = Instantiate(player, spawn.transform.position, player.transform.rotation);
        mp.GetComponent<Online>().NAME = name;

        #region Broadcast
        JSONObject _player;

        Dictionary<string, string> data = new Dictionary<string, string>();
        data["x"] = spawn.transform.position.x.ToString();
        data["y"] = spawn.transform.position.y.ToString();
        data["z"] = spawn.transform.position.z.ToString();
        data["name"] = name;

        _player = new JSONObject(data);

        socket.Emit("newPlayer", _player);
        #endregion
    }
    #endregion
}