using System.Collections;
using System.Drawing;
using UnityEngine;

public class Coin : MonoBehaviour
{void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            FindAnyObjectByType<GameManager>().AddCoinScore();
            Destroy(gameObject);
        }
    }
}

public class PlayerController : MonoBehaviour
{
    //movement
    //shooting
    //scope access modifier private or public
    public int lives = 3;
    private int weaponType;
    private GameManager gameManager;
    

    
    private float playerSpeed;
    private float horizontalInput;
    private float verticalInput;

    private float horizontalScreenLimit = 9.5f;
    private float verticalScreenLimit = 5.5f;
    public GameObject bulletPrefab;
    public GameObject explosionPrefab;
    public GameObject thrusterPrefab;
    public GameObject shieldPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        lives = 3;
        playerSpeed = 6f;
        weaponType = 1;//deafault weapon
        gameManager.ChangeLivesText(lives);
    }

    public void LoseALife()
    {
        //do I have a shield - if so loose the shield first and not the life, by checking visibility and changing UI powerup text
        // if not - lose a life
        lives--;
        gameManager.ChangeLivesText(lives);
        if(lives <= 0)
        {
            //game over
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            gameManager.GameOver();
            Destroy(this.gameObject);
           
        }
    }

    IEnumerator SpeedPowerDown()
    {
        yield return new WaitForSeconds(3f);
        playerSpeed = 6f;
        thrusterPrefab.SetActive(false);
        gameManager.ManagePowerupText(0);
        gameManager.PlaySound(2);
    }

    IEnumerator WeaponPowerDown()
    {
        yield return new WaitForSeconds(3f);
        weaponType = 1;
        gameManager.ManagePowerupText(0);
        gameManager.PlaySound(2);
    }
    private void OnTriggerEnter2D(Collider2D whatDidIHit)
    {

        if (whatDidIHit.tag == "Coin")
        {
            gameManager.AddCoinScore();   // +1 score
            Destroy(whatDidIHit.gameObject);
            gameManager.PlaySound(1);
        }


        if (whatDidIHit.tag == "Powerup")
        {
            Destroy(whatDidIHit.gameObject);
            int whichPowerup = Random.Range(1, 4);
            gameManager.PlaySound(1);
            switch (whichPowerup)
            {
                case 1:
                    playerSpeed = 10f;
                    // start coroutine
                    StartCoroutine(SpeedPowerDown());
                    thrusterPrefab.SetActive(true);
                    gameManager.ManagePowerupText(1);
                    break;
                case 2:
                    weaponType = 2;
                    StartCoroutine(WeaponPowerDown());
                    gameManager.ManagePowerupText(2);
                    break;
                case 3:
                    weaponType = 3;
                    StartCoroutine(WeaponPowerDown());
                    gameManager.ManagePowerupText(3);
                    break;
                case 4:
                    // shield powerup do you have shield if yes do nothinh if no activate it
                    gameManager.ManagePowerupText(4);
                    break;
               

            }

        }
    }


    // Update is called once per frame
    void Update()
    {
        //movement
        //shooting

        Movement();
        Shooting();
    }
    void Movement()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        //move the player
        transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * Time.deltaTime * playerSpeed);
        //limit the player movment on screen
        if (transform.position.x > horizontalScreenLimit || transform.position.x < -horizontalScreenLimit)
        {
            transform.position = new Vector3(transform.position.x *-1, transform.position.y, 0);
        }

        if(transform.position.y > verticalScreenLimit || transform.position.y < -verticalScreenLimit)
        {
            transform.position = new Vector3(transform.position.x, -transform.position.y, 0);
        }
    }
    //shooting
    void Shooting()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            //spawn bullet
            Instantiate(bulletPrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        }
    }
    

}
