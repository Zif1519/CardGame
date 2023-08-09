using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class gameManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip audioMatch;

    public GameObject card;
    public GameObject firstCard;
    public GameObject secondCard;
    public GameObject endWindow;

    public Text timeTxt;
    public Text nameTxt;
    public Text trialTxt;
    public Text endTxt;
    public Text scoreTxt;
    float time = 0.0f;

    public static gameManager I;

    public int numsOfCards = 12;

    private int numsOfMatches = 0;
    private int numsOfTrials = 0;

    void Awake()
    {
        I = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
        numsOfMatches = 0;

        int[] pics = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5 };

        pics = pics.OrderBy(item => Random.Range(-1.0f, 1.0f)).ToArray();

        for (int i = 0; i < numsOfCards; i++)
        {
            GameObject newCard = Instantiate(card);
            newCard.transform.parent = GameObject.Find("cards").transform;

            float x = (i / 4) * 1.4f - 1.38f;
            float y = (i % 4) * 1.4f - 3.5f;
            newCard.transform.position = new Vector3(x, y, 0);

            string picName = "pic" + pics[i].ToString();
            newCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(picName);

        }
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        timeTxt.text = time.ToString("N2");

        if (time > 30.0f)
        {
            endWindow.SetActive(true);

            endTxt.text = "����";
            endTxt.color = Color.red;
            scoreTxt.text = "0";

            Time.timeScale = 0.0f;
        }
        else
        {
            CalculateScore(); // ���� �� (30�� �̸����� ���� ���� ��) ���� ��� �Լ�
        }

    }

    public void isMatched()
    {
        numsOfTrials++;
        trialTxt.text = numsOfTrials.ToString();

        string firstCardImage = firstCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name;
        string secondCardImage = secondCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name;

        if (firstCardImage == secondCardImage)
        {
            numsOfMatches += 2;

            audioSource.PlayOneShot(audioMatch);
            firstCard.GetComponent<card>().destroyCard();
            secondCard.GetComponent<card>().destroyCard();
            if (firstCardImage == "pic3" || secondCardImage == "pic3" || firstCardImage == "pic4" || secondCardImage == "pic4")
            {
                nameTxt.text = "�ּ���";
            }
            else if (firstCardImage == "pic1" || secondCardImage == "pic1" || firstCardImage == "pic2" || secondCardImage == "pic2")
            {
                nameTxt.text = "�赵��";
            }
            else
            {
                nameTxt.text = "�迹��";
            }

            int cardsLeft = GameObject.Find("cards").transform.childCount;
            Debug.Log(cardsLeft);
            if (cardsLeft == 2)
            {
                endWindow.SetActive(true);

                endTxt.text = "����";
                endTxt.color = Color.yellow;
                scoreTxt.text = (100 - numsOfTrials).ToString();

                Time.timeScale = 0.0f;
            }
            DecreaseTimeOnMatchSuccess(); // ��ġ ���� �� �ð� 1�� ���� �Լ� ȣ��
        }
        else
        {
            IncreaseTimeOnMatchFail(); // ��ġ ���� �� �ð� 2�� ���� �Լ� ȣ��

            firstCard.GetComponent<card>().closeCard();
            secondCard.GetComponent<card>().closeCard();
            nameTxt.text = "����";
        }

        firstCard = null;
        secondCard = null;
    }




    public void IncreaseTimeOnMatchFail() // ��ġ ���� �� �ð� 2�� ����
    {
        time += 2.0f; // �ð� 2�� ����
        timeTxt.text = time.ToString("N2"); // UI�� �ð� ������Ʈ

        // ���� ó��
        if (time > 30.0f) // �ð��� 30�� �̻��̸�
        {
            endWindow.SetActive(true);
            endTxt.text = "����"; 
            endTxt.color = Color.red; 
            scoreTxt.text = "0"; 
            Time.timeScale = 0.0f; 
        }
    }

    private void DecreaseTimeOnMatchSuccess() // ��ġ ���� �� �ð� 1�� ����
    {
        if (time >= 1.0f) 
        {
            time -= 1.0f; // �ð� 1�� ����
            timeTxt.text = time.ToString("N2"); // UI�� �ð� ������Ʈ
        }
    }

    void CalculateScore()     // �ð��� ���� ������ ���

    {
        int baseScore = 100; // �⺻����
        int timeBonus = 0;

        if (time >= 25.0f)
        {
            timeBonus = -15; // ���� �ð��� 25�� �̻��� ��� 15�� ����
        }
        else if (time >= 20.0f)
        {
            timeBonus = -10; // 20�� �̻� 10�� ����
        }
        else if (time >= 15.0f)
        {
            timeBonus = -5; // 15�� �̻� 5�� ����
        }
        // 15�� �̸� ���� 0

        int finalScore = Mathf.Max(0, baseScore + timeBonus - numsOfTrials); // ���� ����
        scoreTxt.text = finalScore.ToString(); // ���� UI�� ����
    }





    public void Retry()
    {
        SceneManager.LoadScene("MainScene");
    }
}
