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

    private void Start()
    {
        score = 0;
        txtScore.text = "Score: " + score;
        txtBombs.text = "Bombs: " + bombs;
    }

    private void Update()
    {
        var x = Input.GetAxis(verKey) * Time.deltaTime * verSpeed;
        var y = Input.GetAxis(horKey) * Time.deltaTime * horSpeed;

        transform.Translate(x, 0, 0);
        transform.Rotate(0, y, 0);

        if (Input.GetKeyDown(fireKey))
            CmdFire();

        if (Input.GetKeyDown(bombKey) && bombs > 0)
            FireBomb();
    }

    private void CmdFire()
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

    private void FireBomb()
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

    public void Respawn()
    {
        score++;
        txtScore.text = "Score: " + score;

        transform.position = initialPos.position;
    }

    public void AddBomb(int b)
    {
        bombs += b;
        txtBombs.text = "Bombs: " + bombs;
    }
}