using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIControl : MonoBehaviour
{
    public GameObject StartButton;

    public GameObject Score;

    public GameObject GameOver;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame()
    {
        StartButton.SetActive(false);
        Score.SetActive(true);
        GameOver.SetActive(false);
        // game start here
    }

    public void UpdateScore(int score)
    {
        Score.GetComponent<TextElement>().text = score.ToString();
    }

    public void EndGame(){
        Score.SetActive(false);
        GameOver.SetActive(true);
        GameOver.GetComponent<TextElement>().text = "Game over /n Score: " + Score.GetComponent<TextElement>().text;
    }
}
