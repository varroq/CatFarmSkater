using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuScript : CatCommunicator
{
    private List<Button> buttonList = new List<Button>();
    private void Awake()
    {
        for(int i = 0; i<9; i++)
        {
            Button b = transform.GetChild(i).gameObject.GetComponent<Button>();
            b.interactable = false;
            buttonList.Add(b);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        int j = 1;
        foreach (Button b in buttonList)
        {
            int l = j;
            b.onClick.AddListener(() => OnLevelSelect(l));
            int s = PlayerPrefs.GetInt("level" + j);
            b.GetComponentInChildren<Text>().text = "Level " + j;
            if (s != 0)
            {
                b.GetComponentInChildren<Text>().text += "\n Best: " + s;
                b.interactable = true;
                if (j < buttonList.Count) buttonList[j].interactable = true;
            }
            j++;
        }

        buttonList[0].interactable = true;
        buttonList[8].GetComponentInChildren<Text>().text = "Bonus Level";
        int n = PlayerPrefs.GetInt("level" + j);
        if (n != 0)
        {
            buttonList[8].GetComponentInChildren<Text>().text += "\n Best: " + n;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    void OnLevelSelect(int index)
    {
        Debug.Log("Loading scene " + index);
        nextScene = index;
        changeScene = true;
        
    }
}
