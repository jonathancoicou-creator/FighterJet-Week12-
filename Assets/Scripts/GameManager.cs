using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyOnePrefab;
    public GameObject enemyTwoPrefab;
    public GameObject cloudPrefab;
    public GameObject powerupPrefab;
    public GameObject coinPrefab;
    public GameObject gameOverText;
    public GameObject restartText;
    public GameObject audioPlayer;//1 audio source 2 an audio clip
    
    public AudioClip powerUpSound;
    public AudioClip powerDownSound;

    public TextMeshProUGUI livesText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI powerUpText;
    
    public float verticalScreenSize;
    public float horizontalScreenSize;
    public int score;
    public int cloudMove;
    private bool gameOver;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        horizontalScreenSize = 10f;
        verticalScreenSize = 6.5f;
        
        score = 0;
        cloudMove = 1;
        gameOver = false;
        AddScore(0);
        Instantiate(playerPrefab, transform.position, Quaternion.identity);
        CreateSky();//instant, every frame
        InvokeRepeating("CreateEnemyOne", 2f, 3f);
        InvokeRepeating("CreateEnemyTwo", 4f, 4f); //over time
        powerUpText.text = "No Powers yet!";
        StartCoroutine(SpawnPowerup());
        StartCoroutine(SpawnCoin());

        IEnumerator SpawnCoin()
        {
            float spawnTime = Random.Range(3f, 6f);
            yield return new WaitForSeconds(spawnTime);
            CreateCoin();
            StartCoroutine(SpawnCoin());
        }

    }
    IEnumerator SpawnPowerup()
    {
        float spawnTime = Random.Range(3, 5); // random spwan time
        yield return new WaitForSeconds(spawnTime); //waiting that amount of time
        CreatePowerup(); //creating powerup
        StartCoroutine(SpawnPowerup()); //doing it again

    }

    void Update()
    {
       if(gameOver && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); // load scene needs a string of a scene that is in build settings
        }

    }

    public void ManagePowerupText(int powerupType)
    {
        switch (powerupType)
        {
            case 1:
                powerUpText.text = "Speed!";
                break;

                case 2:
                powerUpText.text = "Double Weapon!";
                break;

                case 3:
                powerUpText.text = "Triple Weapon!";
                break;

                case 4:
                powerUpText.text = "Shield!";
                break;

            default:
             powerUpText.text = "No Powers yet!";
                break;

        }
    }

    public void PlaySound(int whichSound)
    {
        switch (whichSound)
        {
            case 1:
                audioPlayer.GetComponent<AudioSource>().PlayOneShot(powerUpSound);
                break;
            case 2:
                audioPlayer.GetComponent<AudioSource>().PlayOneShot(powerDownSound);
                break;
      
        }
    }

    public void ChangeLivesText(int currentLives)
    {
        livesText.text = "Lives" + currentLives;
    }
    public void AddCoinScore()
    {
        AddScore(1);
    }
    void CreatePowerup()
    {
        Instantiate(powerupPrefab, new Vector3(Random.Range(-horizontalScreenSize * 0.8f, horizontalScreenSize * 0.8f), Random.Range(-verticalScreenSize * 0.8f, verticalScreenSize * 0.8f), 0), Quaternion.identity);
    }

    void CreateCoin()
    {
        GameObject coin = Instantiate(coinPrefab, new Vector3(Random.Range(-horizontalScreenSize * 0.8f, horizontalScreenSize * 0.8f), Random.Range(-verticalScreenSize * 0.8f, verticalScreenSize * 0.8f), 0), Quaternion.identity);

        Destroy(coin, 2.5f); 
    }
    void CreateSky()
    {
        for(int i = 0; i < 30; i++)
        {
            Instantiate(cloudPrefab, new Vector3(Random.Range(-horizontalScreenSize, horizontalScreenSize), Random.Range(-verticalScreenSize, verticalScreenSize), 0), Quaternion.identity);
        }
        //while statement is while something is true: keep doing this
    
    }


    public void AddScore(int earnedScore)
    {
        score += earnedScore;
        scoreText.text = "Score: " + score;
    }


    void CreateEnemyOne()
    {
        Instantiate(enemyOnePrefab, new Vector3(Random.Range(-8f, 8f), 6.5f, 0), Quaternion.identity);

    }
    void CreateEnemyTwo()
    {
        Instantiate(enemyTwoPrefab, new Vector3(10f, Random.Range(-6.5f, 6.5f), 0), Quaternion.identity);
    }

    public void GameOver()
    {
        gameOverText.SetActive(true);
        restartText.SetActive(true);
        gameOver = true;
        CancelInvoke();
        cloudMove = 0;

    }


}
