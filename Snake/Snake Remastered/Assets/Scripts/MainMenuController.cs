using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

    public Text bestScore;
    public Text bestRank;
    public Text totalPoints;
    [Space]
    public GameObject MainMenuUI;
    public GameObject settingsUI;
    public GameObject snakeColorUI;
    public GameObject foodColorUI;
    public GameObject backColorUI;
    [Space]
    public Text currentSnakeColor;
    public Text currentBackColor;
    public Text currentFoodColor;


    void Start() {
        bestScore.text = "Best Score: " + PlayerPrefs.GetInt("bestScore").ToString();
        bestRank.text = "Highest Rank: " + PlayerPrefs.GetInt("bestRank").ToString();
        totalPoints.text = "Total Points: " + PlayerPrefs.GetInt("totalPoints").ToString();
    }

    #region Button Functions
    public void BeginGameFunction()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGameFunction()
    {
        Application.Quit();
    }

    public void SettingsFunction()
    {
        settingsUI.SetActive(true);
        MainMenuUI.SetActive(false);
    }

    public void SnakeColorFunction()
    {
        snakeColorUI.SetActive(true);
        settingsUI.SetActive(false);
        UpdateSnakeColor();
    }

    public void FoodColorFunction()
    {
        foodColorUI.SetActive(true);
        UpdateFoodColor();
        settingsUI.SetActive(false);
    }

    public void BackgroundColorUI()
    {
        backColorUI.SetActive(true);
        UpdateBackgroundColor();
        settingsUI.SetActive(true);
    }

    public void BackSettings()
    {
        backColorUI.SetActive(false);
        foodColorUI.SetActive(false);
        snakeColorUI.SetActive(false);
        settingsUI.SetActive(true);
    }

    public void BackMain()
    {
        settingsUI.SetActive(false);
        MainMenuUI.SetActive(true);
    }

    #endregion

    #region UI Management

    public void ChangeSnakeColor(int color)
    {
        PlayerPrefs.SetInt("snakeType", color);
        UpdateSnakeColor();
    }

    void UpdateSnakeColor()
    {

        switch (PlayerPrefs.GetInt("snakeType"))
        {
            case 0:
                currentSnakeColor.text = "Current Color is: " + "White";
                return;

            case 1:
                currentSnakeColor.text = "Current Color is: " + "Red";
                return;

            case 2:
                currentSnakeColor.text = "Current Color is: " + "Green";
                return;

            case 3:
                currentSnakeColor.text = "Current Color is: " + "Blue";
                return;
        }

        
    }

    public void ChangeBackgroundColor(bool color)
    {
       if (color)
        {
            Debug.Log("CBC True 1");
            PlayerPrefs.SetInt("cameraColor", 1);
        }
       else
        {
            Debug.Log("CBC False 0");
            PlayerPrefs.SetInt("cameraColor", 0);
        }
        UpdateBackgroundColor();
    }

    void UpdateBackgroundColor()
    {
        Debug.Log("UBC Run" + PlayerPrefs.GetInt("cameraColor"));
        switch (PlayerPrefs.GetInt("cameraColor"))
        {
            case 0:
                currentBackColor.text = "Current Color is: " + "Black";
                Debug.Log("UBG 0");
                return;

            case 1:
                Debug.Log("UBC 1");
                currentBackColor.text = "Current Color is: " + "Gray";
                return;
        }

    }

    public void ChangeFoodColor(int color) 
    {
        PlayerPrefs.SetInt("foodType", color);
        Debug.Log("Button is working: Food");
        UpdateFoodColor();
    }

    public void UpdateFoodColor()
    {

        switch (PlayerPrefs.GetInt("foodType"))
        {
            case 0:
                currentFoodColor.text = "Current Color is: " + "White";
                return;

            case 1:
                currentFoodColor.text = "Current Color is: " + "Red";
                return;

            case 2:
                currentFoodColor.text = "Current Color is: " + "Green";
                return;

            case 3:
                currentFoodColor.text = "Current Color is: " + "Blue";
                return;
        }
    }

    #endregion
}
