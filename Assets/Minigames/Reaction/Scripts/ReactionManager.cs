using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionManager : MonoBehaviour
{
    public GameObject p1;
    public GameObject p2;
    public GameObject p1Image;
    public GameObject p2Image;
    public GameObject ready;
    public GameObject fight;
    bool start = false;
    public AudioSource drum;
    public AudioSource slap1;
    public AudioSource slap2;
    public GameObject p1Win;
    public GameObject p2Win;
    int winner = 0;
    // Start is called before the first frame update
    void Start()
    {
        // Set p1Image rect transform position
        p1Image.GetComponent<RectTransform>().localPosition = new Vector3(-1400, 190, 0);
        // Set p2Image rect transform position
        p2Image.GetComponent<RectTransform>().localPosition = new Vector3(1400, -190, 0);

        ready.GetComponent<RectTransform>().localPosition = new Vector3(0, 800, 0);
        // Set random Hue to both players
        p1.GetComponentInChildren<SkinnedMeshRenderer>().material.SetFloat("_Hue", Random.Range(0f, 360f));
        // Set p2 to the opposite Hue
        p2.GetComponentInChildren<SkinnedMeshRenderer>().material.SetFloat("_Hue", p1.GetComponentInChildren<SkinnedMeshRenderer>().material.GetFloat("_Hue") + 180f);
        p1Win.GetComponent<RectTransform>().localPosition = new Vector3(0, 2000, 0);
        p2Win.GetComponent<RectTransform>().localPosition = new Vector3(0, 2000, 0);

        IntroSequence();
        // wait for random amount of time, then toggle start
        StartCoroutine(StartDelay());
    }

    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            // check for input
            if (Input.GetKeyDown(KeyCode.A) && winner == 0)
            {
                winner = 1;
            }
            else if (Input.GetKeyDown(KeyCode.L) && winner == 0)
            {
                winner = 2;
            }
        }
        if (start && winner != 0)
        {
            fight.SetActive(false);
            slap2.Play();
            Swing();
            // compare times
            if (winner == 1)
            {
                // p1 wins
                Debug.Log("P1 wins!");
                // Delayed disabling of p2 animator
                StartCoroutine(EndSequence(p1, p2));
                p1Win.SetActive(true);
                StartCoroutine(MoveImage(p1Win, 0f, 0f, 6f));
            }
            else if (winner == 2)
            {
                // p2 wins
                Debug.Log("P2 wins!");
                // Delayed disabling of p1 animator
                StartCoroutine(EndSequence(p2, p1));
                p2Win.SetActive(true);
                StartCoroutine(MoveImage(p2Win, 0f, 0f, 6f));
            }
            else if (winner == 3)
            {
                // tie
                Debug.Log("Tie!");
                // Set p1 and p2 sheath animation
                p1.GetComponent<Animator>().SetInteger("State", 2);
                p2.GetComponent<Animator>().SetInteger("State", 2);
            }
            start = false;
        }
    }

    IEnumerator EndSequence(GameObject winner, GameObject loser)
    {
        yield return new WaitForSeconds(3f);
        winner.GetComponent<Animator>().SetInteger("State", 2);
        loser.GetComponent<Animator>().enabled = false;
    }

    void IntroSequence()
    {
        drum.Play();
        // Move p1Image and p2Image to -400 and 400 x positions respectively over time
        StartCoroutine(MoveImage(p1Image, -400f, 190f, 2f));
        StartCoroutine(MoveImage(p2Image, 400f, -190f, 2f));
        StartCoroutine(MoveImage(ready, 0f, 0f, 2f));
    }

    IEnumerator MoveImage(GameObject image, float x, float y, float time)
    {
        float elapsedTime = 0;
        Vector3 startingPos = image.GetComponent<RectTransform>().localPosition;
        Vector3 targetPos = new Vector3(x, y, 0);
        while (elapsedTime < time)
        {
            image.GetComponent<RectTransform>().localPosition = Vector3.Lerp(startingPos, targetPos, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        image.GetComponent<RectTransform>().localPosition = targetPos;
    }


    IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(Random.Range(3f, 10f));
        start = true;
        slap1.Play();
        Debug.Log("Start!");
        p1Image.SetActive(false);
        p2Image.SetActive(false);
        ready.SetActive(false);
        fight.SetActive(true);
    }

    void Swing()
    {
        // set p1 swing animation
        p1.GetComponent<Animator>().SetInteger("State", 1);
        // set p2 swing animation
        p2.GetComponent<Animator>().SetInteger("State", 1);
        p1.transform.position = new Vector3(1, p1.transform.position.y, p1.transform.position.z);
        p2.transform.position = new Vector3(-1, p2.transform.position.y, p2.transform.position.z);
    }
}
