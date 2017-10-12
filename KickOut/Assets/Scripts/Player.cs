using SocketIO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Text txtScore;
    public Text txtBombs;
    public Transform initialPos;

    public int score;
    public int bombs;

    public float horSpeed;
    public float verSpeed;
    public float bulletSpeed;

    public string horKey;
    public string verKey;
    public string fireKey;
    public string bombKey;

    public GameObject bulletPrefab;
    public GameObject bombPrefab;
    public Transform bulletSpawn;

    private Online online;

    #region Private
    private void Awake()
    {
        online = GetComponent<Online>();
    }

    private void Start()
    {
        score = 0;
        txtScore.text = "Score: " + score;
        txtBombs.text = "Bombs: " + bombs;
    }

    private void FixedUpdate()
    {
        var x = Input.GetAxis(verKey) * Time.deltaTime * verSpeed;
        var y = Input.GetAxis(horKey) * Time.deltaTime * horSpeed;

        transform.Translate(x, 0, 0);
        transform.Rotate(0, y, 0);

        if (Input.GetKeyDown(fireKey))
        {
            //Shoot local
            CmdFire();

            //Broadcast shoot
            online.BroadcastAction("Fire");
        }

        if (Input.GetKeyDown(bombKey) && bombs > 0)
        {
            //Shoot local
            CmdBomb();

            //Broadcast shoot
            online.BroadcastAction("Bomb");
        }
    }
    #endregion

    #region Public
    public void Respawn()
    {
        score++;
        txtScore.text = "Score: " + score;

        GetComponent<Rigidbody>().velocity = new Vector3();
        transform.position = initialPos.position;
    }

    public void AddBomb(int b)
    {
        bombs += b;
        txtBombs.text = "Bombs: " + bombs;
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

        bombs--;
        txtBombs.text = "Bombs: " + bombs;
    }
    #endregion
}