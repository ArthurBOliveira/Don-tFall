using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Text txtScore;
    public Transform initialPos;

    public int score;

    public float horSpeed;
    public float verSpeed;
    public float bulletSpeed;

    public string horKey;
    public string verKey;
    public string fireKey;

    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    private void Start()
    {
        score = 0;
        txtScore.text = "" + score;
    }

    private void Update()
    {
        var x = Input.GetAxis(verKey) * Time.deltaTime * verSpeed;
        var y = Input.GetAxis(horKey) * Time.deltaTime * horSpeed;

        transform.Translate(x, 0, 0);
        transform.Rotate(0, y, 0);

        if (Input.GetKeyDown(fireKey))
        {
            CmdFire();
        }
    }

    private void CmdFire()
    {
        // Create the Bullet from the Bullet Prefab
        var bullet = (GameObject)Instantiate(
            bulletPrefab,
            bulletSpawn.position,
            bulletSpawn.rotation);

        // Add velocity to the bullet
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.right * bulletSpeed;

        // Destroy the bullet after 2 seconds
        Destroy(bullet, 3f);
    }

    public void Respawn()
    {
        score++;
        txtScore.text = "" + score;

        transform.position = initialPos.position;
    }
}