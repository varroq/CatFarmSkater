using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PeteScript : CatCommunicator
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    AudioSource player;

    
    
    List<GameObject> addTextList = new List<GameObject>();

    [SerializeField] Canvas canvas = default;
    [SerializeField] Text scoreText = default;
    [SerializeField] GameObject darkness = default;
    [SerializeField] GameObject textPreFab = default;
    [SerializeField] ParticleSystem ps = default;
    [SerializeField] GameObject catList = default;

    [SerializeField] List<AudioClip> uhList = default;
    [SerializeField] List<AudioClip> meowList = default;
    [SerializeField] AudioClip ollie = default;
    [SerializeField] AudioClip rails = default;
    [SerializeField] AudioClip autsch = default;
    [SerializeField] AudioClip meowLight = default;

    private bool walljump;
    private bool jump;
    private bool isRails;
    float h;
    int cats;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        player = GetComponent<AudioSource>();    
            
    }
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        cats = catList.transform.childCount;
        paused = false;
        complete = false;
        isRails = false;
        fail = false;
        player.Play();
    }

    private void FixedUpdate()
    {
        if (walljump)
        {
            rb.AddForce(Vector3.up * 280);
            Vector2 vel = new Vector2(-rb.velocity.x, rb.velocity.y);
            rb.velocity = vel;
            walljump = false;
        }
        if (jump)
        {
            rb.AddForce(Vector3.up * 280);
            jump = false;
        }

        if (Mathf.Abs(rb.velocity.x) < 5) rb.AddForce(Vector3.right * h *10);

    }

    // Update is called once per frame
    void Update()
    {
        if (paused)
        {
            Time.timeScale = 0;
            return;
        } else
        {
            Time.timeScale = 1;
        }
        var emiss = ps.emission;
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

        if(Physics2D.Raycast(transform.position + Vector3.down, Vector3.down, 0.1f))
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position + Vector3.down, Vector3.down, 0.1f);
            if (hit.collider.name.Equals("Rails"))
            {
                //
                
                player.clip = rails;
                if (!isRails) player.Play();
                emiss.rateOverTime = 50 * Mathf.Abs(rb.velocity.x);
                isRails = true;
            }
            else
            {
                emiss.rateOverTime = 0;
                //if (player.isPlaying) player.Pause();
                player.clip = null;
                isRails = false;
            }

        }
        else
        {
            emiss.rateOverTime = 0;
            //if (player.isPlaying) player.Pause();
            player.clip = null;
            isRails = false;
        }


        if (transform.position.x - darkness.transform.position.x < 12.5f)
        {
            player.PlayOneShot(autsch);
            scoreText.text += "\nGAME OVER!";
            fail = true;
            paused = true;
            return;
        }
        if(rb.velocity.x != 0) sr.flipX = rb.velocity.x < 0;

        if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)){
            h = -1;
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)){
            h = 1;
        }
        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow)||Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow)){
            h = 0;
        }

        


        


        if (Input.GetButtonDown("Jump") && Mathf.Abs(rb.velocity.x) > 1 
            && (Physics2D.Raycast(transform.position + Vector3.right* 0.4f, Vector3.right, 0.15f) || Physics2D.Raycast(transform.position + Vector3.left* 0.4f, Vector3.left, 0.15f)))
        {
            walljump = true;
            player.PlayOneShot(uhList[UnityEngine.Random.Range(0, uhList.Count)]);
        } else if (Input.GetButtonDown("Jump") && Physics2D.Raycast(transform.position + Vector3.down, Vector3.down, 0.1f))
        {
            jump = true;
            player.PlayOneShot(ollie);
        }
        if (Input.GetKeyDown(KeyCode.Escape) && !complete)
        {
            paused = !paused;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.name.Equals("CatLight")){
            if (cats == 0)
            {
                player.PlayOneShot(meowLight);
                score += (int) (transform.position.x - darkness.transform.position.x) * 70;
                scoreText.text = "Score: " + score;
                if(score > PlayerPrefs.GetInt("level" + nextScene)) PlayerPrefs.SetInt("level" + nextScene, score);
                scoreText.text += "\nLEVEL COMPLETE!";
                complete = true;
                paused = true;
            }
            else
            {
                var failText = Instantiate(textPreFab, collision.transform.position, Quaternion.identity, canvas.transform);
                failText.GetComponent<Text>().text = "Collect " + cats + " more\nkittens first!";
                addTextList.Add(failText);
            }
            return;
        }
        player.PlayOneShot(meowList[UnityEngine.Random.Range(0, meowList.Count)]);
        score += 1000;
        cats--;
        scoreText.text = "Score: " + score;

        var movingText = Instantiate(textPreFab, collision.transform.position, Quaternion.identity, canvas.transform);
        movingText.GetComponent<Text>().text = "Cat collected!\n+1000";
        addTextList.Add(movingText);
        

        Destroy(collision.gameObject);
    }
}
