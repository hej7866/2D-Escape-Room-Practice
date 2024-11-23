using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    public GameManager gm;
    public float speed;

    float h;
    float v;
    Direction currentDirection = Direction.None;
    Vector3 dirVec;
    GameObject scanObject;


    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;

    

    public enum Direction
    {
        None,
        Horizontal,
        Vertical
    }

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
  
        PlayerMove();

    }

    void PlayerMove()
    {
        // Player Move Input
        if(!gm.isAction)
        {
            h = Input.GetAxisRaw("Horizontal");
            v = Input.GetAxisRaw("Vertical");
        }
        else
        {
            // 대화 중일 때 h와 v를 0으로 설정하여 애니메이션을 멈춤
            h = 0;
            v = 0;
            currentDirection = Direction.None;
            anim.SetBool("isChange", false); // 애니메이션 상태 초기화
        }

        bool hDown = Input.GetButtonDown("Horizontal");
        bool vDown = Input.GetButtonDown("Vertical");
        bool hUp = Input.GetButtonUp("Horizontal");
        bool vUp = Input.GetButtonUp("Vertical");

        // Player Direction Horizontal? Vertical?
        if (hDown)
        {
            currentDirection = Direction.Horizontal;
        }
        else if (vDown)
        {
            currentDirection = Direction.Vertical;
        }

        if (hUp && currentDirection == Direction.Horizontal)
        {
            currentDirection = (v != 0) ? Direction.Vertical : ((h != 0) ? Direction.Horizontal : Direction.None);
        }
        else if (vUp && currentDirection == Direction.Vertical)
        {
            currentDirection = (h != 0) ? Direction.Horizontal : ((v != 0) ? Direction.Vertical : Direction.None);
        }

        // Player Move Animation
        if (anim.GetInteger("hAxisRaw") != h)
        {
            anim.SetBool("isChange", true);
            anim.SetInteger("hAxisRaw", (int)h);
            if(currentDirection == Direction.Horizontal)
            {
                anim.SetBool("isSide", true);
            }
            else if(currentDirection == Direction.Vertical)
            {
                anim.SetBool("isNonSide", true);
            }
        }
        else if (anim.GetInteger("vAxisRaw") != v)
        {
            anim.SetBool("isChange", true);
            anim.SetInteger("vAxisRaw", (int)v);
            if(currentDirection == Direction.Horizontal)
            {
                anim.SetBool("isSide", true);
            }
            else if(currentDirection == Direction.Vertical)
            {
                anim.SetBool("isNonSide", true);
            }
        }
        else
        {
            anim.SetBool("isSide", false);
            anim.SetBool("isNonSide", false);
            anim.SetBool("isChange", false);
        }

        // 입력이 멈추면 애니메이션 끊기
        if (h == 0 && v == 0)
        {
            anim.SetBool("isChange", false);
        }

        // Direction
        if (vDown && v == 1)
        {
            dirVec = Vector3.up;
        }
        else if (vDown && v == -1)
        {
            dirVec = Vector3.down;
        }
        else if (hDown && h == 1)
        {
            dirVec = Vector3.right;
        }
        else if (hDown && h == -1)
        {
            dirVec = Vector3.left;
        }

        //Scan Object
        if(Input.GetButtonDown("Jump") && scanObject != null)
        {
            gm.ScanForNPC(scanObject);
        }
    }


    void FixedUpdate()
    {
        // Move
        Vector2 moveVec = Vector2.zero;
        if (currentDirection == Direction.Horizontal)
        {
            moveVec = new Vector2(h, 0);
        }
        else if (currentDirection == Direction.Vertical)
        {
            moveVec = new Vector2(0, v);
        }

        rigid.velocity = moveVec * speed;

        //Ray
        //Debug.DrawRay(rigid.position, dirVec * 1.0f, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, dirVec, 1.0f, LayerMask.GetMask("Object"));

        if (rayHit.collider != null)
        {
            scanObject = rayHit.collider.gameObject;
        }
        else
        {
            scanObject = null;
        }
    }

}
