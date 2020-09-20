using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CowSript : CatCommunicator
{
    List<GameObject> addTextList = new List<GameObject>();
    AudioSource player;

    [SerializeField] Canvas canvas = default;
    [SerializeField] Text scoreText = default;
    [SerializeField] GameObject textPreFab = default;
    [SerializeField] List<AudioClip> mooList = default;

    private bool jumped;

    private void Awake()
    {
        player = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
        jumped = false;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = addTextList.Count - 1; i >= 0; i--)
        {
            var addText = addTextList[i];
            Vector2 currentPos = addText.transform.position;
            currentPos += Vector2.up * Time.deltaTime;
            addText.transform.position = currentPos;
            Vector4 color = addText.GetComponent<Text>().color;
            color -= new Vector4(0, 0, 0, Time.deltaTime); ;
            addText.GetComponent<Text>().color = color;
            if (color.w <= 0)
            {
                Destroy(addText);
                addTextList.RemoveAt(i);
            }
        }

        if (Physics2D.Raycast(transform.position + Vector3.up, Vector3.up, 5f) && !jumped)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position + Vector3.up, Vector3.up, 5f);
            Debug.Log("Cow hit" + hit.collider.name);
            if (hit.collider.name.Equals("Pete"))
            {

                score += 100;
                scoreText.text = "Score: " + score;

                var movingText = Instantiate(textPreFab, hit.collider.transform.position, Quaternion.identity, canvas.transform);
                movingText.GetComponent<Text>().text = "Cow jump!\n+100";
                addTextList.Add(movingText);
                jumped = true;
                player.PlayOneShot(mooList[UnityEngine.Random.Range(0, mooList.Count)]);
            }
            
        }
    }
}
