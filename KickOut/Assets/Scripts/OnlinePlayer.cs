using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlinePlayer : MonoBehaviour
{
    public string NAME;

    private SocketIOComponent socket;
    private string actionFire = "Fire";
    private string actionBomb = "Bomb";

    #region Private
    private void Awake()
    {
        socket = GameObject.FindGameObjectWithTag("Socket").GetComponent<SocketIOComponent>();
        socket.On("moveFromServer", MoveFromServer);
        socket.On("playerActionServer", PlayerActionServer);
    }

    #endregion

    #region Public
    public void PlayerActionServer(SocketIOEvent obj)
    {
        Dictionary<string, string> data = obj.data.ToDictionary();

        if (data["name"] == NAME)
        {
            if (data["action"] == actionFire)
                GetComponent<Player>().CmdFire();
            if (data["action"] == actionBomb)
                GetComponent<Player>().CmdBomb();
        }
    }

    public void MoveFromServer(SocketIOEvent obj)
    {
        Dictionary<string, string> data = obj.data.ToDictionary();

        if (data["name"] == NAME)
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
    #endregion
}
