using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    public float moveSpeed;

    public static float thirst = 100.0f;
    public static float health = 100.0f;

    public float footstepSpeed;
    public AudioClip[] footstepSounds;
    public AudioSource audioSource;

    private Animator anim;
    private Rigidbody2D myRigidbody;

    private bool playerMoving;
    private Vector2 lastMove;

    public CardinalDirection facing;
    public Text uiFacing; // ref to debug ui facing text

    void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        StartCoroutine("Footsteps");
        facing = CardinalDirection.down; // init to down to match starting anim
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        DrainThirst(1.0f);
        PlayerMove();
    }
    
    void PlayerMove() {
        //movement code
        playerMoving = false;

        if (Input.GetAxisRaw("Horizontal") > 0.5f || Input.GetAxisRaw("Horizontal") < -0.5f)
        {
            myRigidbody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, myRigidbody.velocity.y);
            playerMoving = true;
            lastMove = new Vector2(Input.GetAxisRaw("Horizontal"), 0f);
        }
        else
        {
            myRigidbody.velocity = new Vector2(0f, myRigidbody.velocity.y);
        }
        if (Input.GetAxisRaw("Vertical") > 0.5f || Input.GetAxisRaw("Vertical") < -0.5f)
        {
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, Input.GetAxisRaw("Vertical") * moveSpeed);
            playerMoving = true;
            lastMove = new Vector2(0f, Input.GetAxisRaw("Vertical"));
        }
        else
        {
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, 0f);
        }
        //movement code
        
        anim.SetFloat("MoveX", Input.GetAxisRaw("Horizontal"));//has the animator check how the player is moving in X
        anim.SetFloat("MoveY", Input.GetAxisRaw("Vertical"));//has the animator check how the player is moving in y
        anim.SetBool("PlayerMoving", playerMoving);//sets the moving bool
        anim.SetFloat("LastMoveX", lastMove.x);
        anim.SetFloat("LastMoveY", lastMove.y);
        SetFacing();
    }

    void SetFacing() 
    {
        if(playerMoving)
        {
            float x, y;
            x = Input.GetAxisRaw("Horizontal");
            y = Input.GetAxisRaw("Vertical");
            if(x == 1 && y == 1) facing = CardinalDirection.up;
            else if(x == 0 && y == 1) facing = CardinalDirection.up;
            else if(x == -1 && y == 1) facing = CardinalDirection.up;
            else if(x == -1 && y == -1) facing = CardinalDirection.down;
            else if(x == -1 && y == 0) facing = CardinalDirection.left;
            else if(x == 0 && y == -1) facing = CardinalDirection.down;
            else if(x == 1 && y == 0) facing = CardinalDirection.right;
            else if(x == 1 && y == -1) facing = CardinalDirection.down;
            uiFacing.text = "Player facing: " + facing;
        }
    }
    
    IEnumerator Footsteps()
    {
        while(true)
        {
            if(playerMoving)
            {
                audioSource.PlayOneShot(footstepSounds[Random.Range(0, 2)]);
            }
            yield return new WaitForSeconds(footstepSpeed);
        }
    }

    public static void TakeDamage(float damage) {
        health -= 10;
    }

    public static void Heal(float healthHealed) {
        health += healthHealed;
    }

    public static void DrainThirst(float drainAmount) {
        thirst -= drainAmount * Time.deltaTime;
        if (thirst < 0) {
            thirst = 0;
        }
    }
}
