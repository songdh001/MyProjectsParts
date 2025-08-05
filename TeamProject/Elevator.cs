using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Elevator : MonoBehaviour, IActivatable
{
    //!!!! ����ġ, ������ �۵� �����ϰ� �Ϸ��� ���� IActivatable �̰� �߰��ؾ� �� !!!!!


    //���������� �ְ�,������
    public Transform topPoint;
    public Transform bottomPoint;

    //���������� ���ǵ�
    public float speed = 2f;

    //�۵��ϴ��� ���� ������ üũ
    private bool isActivated = false;
    private bool isMovingUp = false;

    //����ġ�� ���� �۵����Ѽ� Ȱ��ȭ �ߴ��� üũ
    private bool isTriggered = false;

    // �÷��̾ �ö�� �ִ��� üũ
    private bool playerOnElevator = false;

    [Header("����ġ ���� ����")]
    public bool useSwitchControl = true; // �����Ϳ��� ���� ����


    public void Activate()
    {
        if (useSwitchControl)
        {
            isTriggered = true;
        }

    }
    public void Deactivate()
    {
        if (useSwitchControl)
        {
            isTriggered = false;
        }

    }


    //�÷��̾�� ������ ��(���������Ϳ� ���� ��) Ȱ��ȭ
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�÷��̾ ť�갡 �ö�Ż �� Ȯ��
        if (collision.CompareTag("Player") || collision.CompareTag("Cube"))
        {
            Debug.Log("�ö�Ž!");

            isActivated = true;
        }

        if (collision.CompareTag("Player"))
        {
            playerOnElevator = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerOnElevator = false;
        }
    }


    private void Start()
    {
        // ����ġ ���� �������ڸ��� �۵��ǵ��� �ϰ� ���� ���
        if (!useSwitchControl)
        {
            isTriggered = true;
        }

    }


    // Update is called once per frame
    void Update()
    {
        //���������� ��������
        if (isActivated && isTriggered && playerOnElevator)
        {
            Debug.Log("�̵���");

            MoveElevator();
        }

    }


    void MoveElevator()
    {
        //���� �����δٸ� ž����Ʈ���� �̵�, �ƴϸ� ��������Ʈ���� �̵� ����Ʈ�� ���ҿ��� ž���� �̵��ϴ� �ŷ� ������
        Vector3 target = isMovingUp ? topPoint.position : bottomPoint.position;

        //���� ��ġ���� Ÿ�� ��ġ���� ���� �ӵ��� �̵�
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        //Ÿ�� ��ġ���� �����ϸ� ��Ȱ��ȭ(�ٽ� ���ȴٰ� Ż ��� �۵�) + �̵� ������ �ٲ���
        if (Vector3.Distance(transform.position, target) < 0.01f)
        {
            Debug.Log("������ ���� ���� �ٲ�");
            isActivated = false;
            isMovingUp = !isMovingUp; // �պ��� �����ߴٸ� �ٲ��༭ �ٸ� �� �������� �̵��� �� �ֵ��� ����
        }
    }


}
