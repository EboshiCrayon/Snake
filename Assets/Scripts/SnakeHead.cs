using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SnakeHead : MonoBehaviour
{
    public GameObject food;
    public GameObject tail;

    public List<GameObject> tails = new List<GameObject>();

    void Start()
    {
        MakeFood();
        StartCoroutine(ForwardLoop());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Right();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Left();
        }

    }

    IEnumerator ForwardLoop()
    {
        // play中はずっとループする
        while (Application.isPlaying)
        {
            // 0.2s やすむ 
            yield return new WaitForSeconds(0.2f);

            Forward();
        }
    }

    void Right()
    {
        Debug.Log("right");
        transform.LookAt(transform.position + transform.right);
    }

    void Left()
    {
        Debug.Log("left");
        transform.LookAt(transform.position - transform.right);
    }

    const float interval = 1.1f;
    void Forward()
    {
        // 頭の位置を覚える
        Vector3 nextPosition = transform.position;
        Vector3 currentPosition = Vector3.zero;

        // 頭を一つ進める
        transform.position += transform.forward * interval;

        foreach (GameObject tail in tails)
        {
            currentPosition = tail.transform.transform.position;

            // 位置を進める
            tail.transform.position = nextPosition;

            nextPosition = currentPosition;
        }
    }

    void MakeFood()
    {
        var randomPosition =
            new Vector3(
                Random.Range(-9, 9),
                0,
                Random.Range(-9, 9)
            );
        Instantiate(food, randomPosition, Quaternion.identity);
    }

    void MakeTail()
    {
        Vector3 lastTailPosition = Vector3.zero;
        if (tails.Count > 0)
        {
            int last = tails.Count;
            lastTailPosition = tails[last - 1].transform.position;
        }
        else
        {
            lastTailPosition = transform.position;
        }

        GameObject tailObj =
            Instantiate(
                tail,
                lastTailPosition - transform.forward * interval,
                Quaternion.identity) as GameObject;
        tails.Add(tailObj);
    }

    const string WALL_TAG = "Wall";
    const string FOOD_TAG = "Food";
    const string TAIL_TAG = "Tail";
    void OnTriggerEnter(Collider other)
    {
        // Debug.Log(c.gameObject.name + " : " + c.tag);
        switch (other.tag)
        {
            case WALL_TAG:
                Debug.Log("game over");
                GameOver();
                break;
            case FOOD_TAG:
                Debug.Log("eat food, add tail");

                Destroy(other.gameObject);
                MakeFood();

                MakeTail();
                break;
            case TAIL_TAG:
                Debug.Log("game over");
                GameOver();
                break;
            default:
                break;
        }
    }

    void GameOver()
    {
        SceneManager.LoadScene("Title");
    }
}
