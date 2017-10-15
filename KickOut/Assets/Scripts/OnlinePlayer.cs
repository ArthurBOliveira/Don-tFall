using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlinePlayer : MonoBehaviour
{
    public string NAME;
    public string ROOM;

    public float bulletSpeed = -10;

    public GameObject bulletPrefab;
    public GameObject bombPrefab;
    public Transform bulletSpawn;

    private SocketIOComponent socket;
    private string actionFire = "Fire";
    private string actionBomb = "Bomb";

    #region Private
    private void Awake()
    {
        socket = GameObject.FindGameObjectWithTag("Socket").GetComponent<SocketIOComponent>();
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
            if (data["action"] == actionFire)
                CmdFire();
            if (data["action"] == actionBomb)
                CmdBomb();
        }        

        yield return new WaitForSeconds(0);
    }

    public void CmdFire()
    {
        // Create the Bullet from the Bullet Prefab
        GameObject bullet = Instantiate(
            bulletPrefab,
            bulletSpawn.position,
            bulletSpawn.rotation);

        // Add velocity to the bullet
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.right * bulletSpeed;

        // Destroy the bullet after 2 seconds
        Destroy(bullet, 3f);
    }

    public void CmdBomb()
    {
        // Create the Bullet from the Bullet Prefab
        GameObject bomb = Instantiate(
            bombPrefab,
            bulletSpawn.position,
            bulletSpawn.rotation);

        // Add velocity to the bullet
        bomb.GetComponent<Rigidbody>().velocity = bomb.transform.right * bulletSpeed;

        // Destroy the bullet after 2 seconds
        Destroy(bomb, 3f);
    }
    #endregion
}
