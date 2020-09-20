using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    AudioSource player;
    [SerializeField] GameObject pete = default;

    private void Awake()
    {
        player = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
        player.Play();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float offset = 1.5f;
        if(pete.transform.position.x >= transform.position.x + offset)
        {
            Vector3 pos = new Vector3(pete.transform.position.x - offset, transform.position.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, pos, 6 * Time.deltaTime);
        }

        if (pete.transform.position.x <= transform.position.x - offset)
        {
            Vector3 pos = new Vector3(pete.transform.position.x + offset, transform.position.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, pos, 6 * Time.deltaTime);
        }
    }
}
