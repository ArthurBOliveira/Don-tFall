using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Online : MonoBehaviour
{
    public string NAME;

    private SocketIOComponent socket;    

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
        data["xr"] = gameObject.transform.rotation.x.ToString();
        data["yr"] = gameObject.transform.rotation.y.ToString();
        data["zr"] = gameObject.transform.rotation.z.ToString();
        data["wr"] = gameObject.transform.rotation.w.ToString();

        obj = new JSONObject(data);

        socket.Emit("updatePosition", obj);
    }
    #endregion
}
