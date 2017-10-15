using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlinePlayer : MonoBehaviour
{
    public string NAME;
    public string ROOM;

    private SocketIOComponent socket;
    private string actionFire = "Fire";
    private string actionBomb = "Bomb";

    private Player _player;

    #region Private
    private void Awake()
    {
        socket = GameObject.FindGameObjectWithTag("Socket").GetComponent<SocketIOComponent>();
        _player = GetComponent<Player>();
    }

    private void Start()
    {        
        socket.On("moveFromServer", MoveFromServer);
        socket.On("playerActionServer", PlayerActionServer);
    }

    #endregion

    #region Public
    public void PlayerActionServer(SocketIOEvent obj)
    {
        Debug.Log(obj);
        Dictionary<string, string> data = obj.data.ToDictionary();

        StartCoroutine(PlayerAction(data));        
    }

    public void MoveFromServer(SocketIOEvent obj)
    {
        Dictionary<string, string> data = obj.data.ToDictionary();

        if (data["name"] == NAME && data["room"] == ROOM)
        {
            float x = float.Parse(data["x"]);
            float y = float.Parse(data["y"]);
            float z = float.Parse(data["z"]);
            float xr = float.Parse(data["xr"]);
            float yr = float.Parse(data["yr"]);
            float zr = float.Parse(data["zr"]);
            float wr = float.Parse(data["wr"]);

            transform.position = new Vector3(x, y, z);
            transform.rotation = new Quaternion(xr, yr, zr, wr);
        }
    }

    public IEnumerator PlayerAction(Dictionary<string, string> data)
    {
        if (data["name"] == NAME && data["room"] == ROOM)
        {
            Debug.Log(_player);

            if (data["action"] == actionFire)
                _player.CmdFire();
            if (data["action"] == actionBomb)
                _player.CmdBomb();
        }        

        yield return new WaitForSeconds(0);
    }
    #endregion
}
