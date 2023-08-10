using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    // public Animator anim; // �ִϸ��̼� ���� �� Ȱ��ȭ�� �ڵ�
    public Animator anim;
    public AudioClip flip;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void openCard()
    {
        // anim.SetBool("isOpen", true);  // �ִϸ��̼� ���� �� �־���� �ڵ�
        anim.SetBool("isOpen", true);
        AudioManager.Instance.PlayClip(flip);

        transform.Find("front").gameObject.SetActive(true);
        transform.Find("back").gameObject.SetActive(false);

        if (GameManager.I.firstCard == null)
        {
            GameManager.I.firstCard = gameObject;
            GameManager.I.openLimitTime = GameManager.I.time + 5.0f;
        }
        else
        {
            GameManager.I.secondCard = gameObject;
            GameManager.I.isMatched();
        }
    }
    public void destroyCard()
    {
        Invoke("destroyCardInvoke", 1.0f);
    }

    void destroyCardInvoke()
    {
        Destroy(gameObject);
    }

    public void closeCard()
    {
        Invoke("closeCardInvoke", 1.0f);
    }

    void closeCardInvoke()
    {
        // anim.SetBool("isOpen", false); // �ִϸ��̼� ���� �� Ȱ��ȭ
        anim.SetBool("isOpen", false);

        transform.Find("back").gameObject.SetActive(true);
        transform.Find("front").gameObject.SetActive(false);
    }

}
