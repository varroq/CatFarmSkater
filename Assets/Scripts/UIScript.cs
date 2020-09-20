using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : CatCommunicator
{
    [SerializeField] Button resumeButton = default;
    [SerializeField] Button restartButton = default;
    [SerializeField] Button mainMenuButton = default;

    [SerializeField] Text resumeText = default;
    List<Button> buttonList = new List<Button>();

    private void Awake()
    {
        buttonList.Add(resumeButton);
        buttonList.Add(restartButton);
        buttonList.Add(mainMenuButton);
    }
    // Start is called before the first frame update
    void Start()
    {
        disableButtons();
    }

    // Update is called once per frame
    void Update()
    {
        if (complete)
        {
            resumeText.text = "Next";
        }
        if (fail)
        {
            resumeButton.interactable = false;
        }
        if (paused)
        {
            enableButtons();
        } else
        {
            disableButtons();
        }
    }

    private void disableButtons()
    {
        foreach (Button b in buttonList)
        {
            b.gameObject.SetActive(false);
        }
    }

    private void enableButtons()
    {
        foreach (Button b in buttonList)
        {
            b.gameObject.SetActive(true);
        }
    }

    public void onResumeClick()
    {
        if (complete)
        {
            nextScene++;
            changeScene = true;
        }
        paused = false;
    }

    public void onRestartClick()
    {
        changeScene = true;
    }

    public void onMainMenuClick()
    {
        nextScene = 0;
        changeScene = true;
    }
    
}
