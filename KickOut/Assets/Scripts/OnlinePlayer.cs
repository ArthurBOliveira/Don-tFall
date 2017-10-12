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

    private Player _player;

    #region Private
    private void Awake()
    {
        socket = GameObject.FindGameObjectWithTag("Socket").GetComponent<SocketIOComponent>();
        socket.On("moveFromServer", MoveFromServer);
        socket.On("playerActionServer", PlayerActionServer);        
    }

    private void Start()
    {
        _player = GetComponent<Player>();
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

    private IEnumerator PlayerAction(Dictionary<string, string> data)
    {
        if (data["name"] == NAME)
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
