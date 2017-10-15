using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Online : MonoBehaviour
{
    public string NAME;
    public string ROOM;

    private SocketIOComponent socket;

    private string actionFire = "Fire";
    private string actionBomb = "Bomb";

    #region Private
    private void Awake()
    {
        socket = GameObject.FindGameObjectWithTag("Socket").GetComponent<SocketIOComponent>();
    }

    private void FixedUpdate()
    {
        EmitPosition();
    }

    private void EmitPosition()
    {
        JSONObject obj;

        Dictionary<string, string> data = new Dictionary<string, string>();
        data["x"] = gameObject.transform.position.x.ToString();
        data["y"] = gameObject.transform.position.y.ToString();
        data["z"] = gameObject.transform.position.z.ToString();
        data["name"] = NAME;
        data["room"] = ROOM;
        data["xr"] = gameObject.transform.rotation.x.ToString();
        data["yr"] = gameObject.transform.rotation.y.ToString();
        data["zr"] = gameObject.transform.rotation.z.ToString();
        data["wr"] = gameObject.transform.rotation.w.ToString();

        obj = new JSONObject(data);

        socket.Emit("updatePosition", obj);
    }
    #endregion

    #region Public
    public void BroadcastAction(string action)
    {
        #region Broadcast
        JSONObject _player;

        Dictionary<string, string> data = new Dictionary<string, string>();
        data["name"] = NAME;
        data["room"] = ROOM;
        data["action"] = action;

        _player = new JSONObject(data);

        socket.Emit("playerAction", _player);
        #endregion
    }

    public void Respawn()
    {
        GameObject[] spawns =  GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameController>().spawnPositions;

        int rnd = UnityEngine.Random.Range(0, 4);

        GameObject spawn = spawns[rnd];

        GetComponent<Rigidbody>().velocity = new Vector3();
        transform.position = spawn.transform.position;
    }
    #endregion
}
