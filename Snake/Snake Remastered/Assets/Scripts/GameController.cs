using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;


public class GameController : MonoBehaviour {

    // Base variables.
    public GameObject snakePrefab;
    public GameObject foodPrefab;
    public GameObject currentFood;
    [Space]
    public GameObject pauseScreen;
    public GameObject endScreen;
    public Text scoreNumber;
    public Text endFinalScore;
    public Text endBestScore;
    [Space]
    public Text rankUI;
    public Text rankFinalUI;
    public float rankUpLimit;
    public int rank;
    [Space]
    private GameObject snakeHeadCheck;
    [Space]
    public Snake head;
    public Snake tail;
    public Vector2 nextPos;
    public int NESW;
    [AddComponentMenu("Dank Memes")]
    public int xBound;
    public int yBound;
    public int maxSize;
    public int currentSize;
    public float invokeTime;
    [Space]
    public int snakeType;
    public GameObject[] snakePrefaba;
    public int foodType;
    public GameObject[] foodPrefaba;
    [Space]
    public GameObject grayCamera;
    public GameObject blackCamera;
    public int cameraSelection;
    [Space]
    public TwitterIntergration twitter;


    public int score;

    private void Start()
    {
        cameraSelection = PlayerPrefs.GetInt("cameraColor");
        snakeType = PlayerPrefs.GetInt("snakeType");
        CameraSelectFunction();
        SpawnSnakeFunction();
        FoodSelectFunction();
        // A repeating function that starts at the begining of the script and repeats every .5 secconds.
        InvokeRepeating(/*Function Name*/ "TimerInvoke", /*Start Time*/ 0,/*Repeating Time*/ invokeTime);
        FoodFunction();
        rankUI.text = "Rank: " + rank.ToString();
        
    }

    void TimerInvoke()
    {
        Movement();
        StartCoroutine(CheckRender(snakeHeadCheck));
        if (currentSize >= maxSize)
        {
            TailFunction();
            Debug.Log("Current Size >= Max");
        }
        else
        {
            currentSize++;
            Debug.Log("CurrentSize++");
        }

    }

    void Movement()
    {
        GameObject temp;
        // Sets the Vector2 nextPos the the currnt positon of the head object.
        nextPos = head.transform.position;
        // Your common switch statement using NESW ans it's variabe input.
        switch (NESW)
        {
            case 0:
                nextPos = new Vector2(nextPos.x, nextPos.y + 1);
                break;
            case 1:
                nextPos = new Vector2(nextPos.x + 1, nextPos.y);
                break;
            case 2:
                nextPos = new Vector2(nextPos.x, nextPos.y - 1);
                break;
            case 3:
                nextPos = new Vector2(nextPos.x - 1, nextPos.y);
                break;
        }
        // Creates a new game object in scene.
        temp = (GameObject)Instantiate(snakePrefab, nextPos, transform.rotation);
        head.SetNext(temp.GetComponent<Snake>());
        snakeHeadCheck = temp;
        head = temp.GetComponent<Snake>();
        return;
    }

    void CompChangeD()
    {
        if (NESW != 2 && Input.GetKeyDown(KeyCode.W))
        {
            NESW = 0;
        }
        if (NESW != 3 && Input.GetKeyDown(KeyCode.D))
        {
            NESW = 1;
        }
        if (NESW != 0 && Input.GetKeyDown(KeyCode.S))
        {
            NESW = 2;
        }
        if (NESW != 1 && Input.GetKeyDown(KeyCode.A))
        {
            NESW = 3;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseFunction();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            score = score + 10;
        }
    }

    private void Update()
    {
        CompChangeD();
    }

    IEnumerator CheckRender(GameObject IN)
    {
        yield return new WaitForEndOfFrame();
        if (IN.GetComponent<Renderer>().isVisible == false)
        {
            if(IN.tag == "food")
            {
                Destroy(IN);
                FoodFunction();
            }
            if(IN.tag == "Snake")
            {
                EndGameFunction();
            }
        }
    }

    private void OnEnable()
    {
        Snake.hit += hit;
    }

    private void OnDisable()
    {
        Snake.hit -= hit;
    }

    private void hit(string WhatWasSent)
    {
        if (WhatWasSent == "food")
        {
            FoodFunction();
            maxSize++;
            ScoreFunction();
            RankCheckFunction();
        }

        if (WhatWasSent == "Snake")
        {
            EndGameFunction();
        }
    }

    void TailFunction()
    {
        Snake tempSnake = tail;
        tail = tail.GetNext();
        tempSnake.RemoveTail();
    }

    void FoodFunction()
    {
        int xPos = Random.Range(-xBound, xBound);
        int yPos = Random.Range(-yBound, yBound);
        currentFood = (GameObject)Instantiate(foodPrefab, new Vector2(xPos, yPos), transform.rotation);
        StartCoroutine(CheckRender(currentFood));
    }

    void ScoreFunction()
    {
        score++;
        PlayerPrefs.SetInt("totalPoints", PlayerPrefs.GetInt("totalPoints") + 1);
        if (score > PlayerPrefs.GetInt("bestScore"))
        {
            PlayerPrefs.SetInt("bestScore", score);
        }
        scoreNumber.text = "Score: " + score.ToString();
    }

    void PauseFunction()
    {
        Debug.Log("Attempting to pause.");
        CancelInvoke("TimerInvoke");
        pauseScreen.SetActive(true);
    }

    public void ResumeFunction()
    {
        Debug.Log("Attempting to resume.");
        pauseScreen.SetActive(false);
        InvokeRepeating(/*Function Name*/ "TimerInvoke", /*Start Time*/ 0,/*Repeating Time*/ invokeTime);
    }

    void EndGameFunction()
    {
        string message = "Snake: Someone scored " + score.ToString() + ", and got to rank " + rank.ToString() + ".";
        CancelInvoke("TimerInvoke");
        endScreen.SetActive(true);
        endBestScore.text = "Best Score: " + PlayerPrefs.GetInt("bestScore").ToString();
        endFinalScore.text = "Final Score: " + score.ToString();
        twitter.SendTweet(message);
    }

    public void NewGameFunction()
    {
        SceneManager.LoadScene(1);
    }

    public void MainMenuFunction()
    {
        SceneManager.LoadScene(0);
    }

    void RankCheckFunction()
    {
        if (score >= rankUpLimit)
        {
            RankUpFunction();
        }
        else
        {
           
        }
    }

    void RankUpFunction()
    {
        rank++;
        if (rank >= PlayerPrefs.GetInt("bestRank"))
        {
            Debug.Log("Updating bestRank");
            PlayerPrefs.SetInt("bestRank", rank);
        }
        if (invokeTime >= 0.2f)
        {
            invokeTime = invokeTime - .01f;
            SpeedChangeFunction();
        }
        rankUI.text = "Rank: " + rank.ToString();
        rankUpLimit = rankUpLimit + 4;

    }

    void SpeedChangeFunction()
    {
        CancelInvoke("TimerInvoke");
        InvokeRepeating(/*Function Name*/ "TimerInvoke", /*Start Time*/ 0,/*Repeating Time*/ invokeTime);
    }

    void SpawnSnakeFunction()
    {
        snakeType = PlayerPrefs.GetInt("snakeType");
        int xPos = 0;
        int yPos = 1;
        snakeHeadCheck = (GameObject)Instantiate(snakePrefaba[snakeType], new Vector2(xPos, yPos), transform.rotation);
        head = snakeHeadCheck.GetComponent<Snake>();
        tail = head;
        snakePrefab = snakePrefaba[snakeType];
    }

    void CameraSelectFunction()
    {
        Debug.Log("CSF run");

        if (cameraSelection == 0)
        {
            Debug.Log("Camera Select Black");
            grayCamera.SetActive(false);
            blackCamera.SetActive(true);
        }
        else
        {
            Debug.Log("Camera Select Gray");
            grayCamera.SetActive(true);
            blackCamera.SetActive(false);
        }
    }

    void FoodSelectFunction()
    {
        foodType = PlayerPrefs.GetInt("foodType");
        foodPrefab = foodPrefaba[foodType];
    }
}
