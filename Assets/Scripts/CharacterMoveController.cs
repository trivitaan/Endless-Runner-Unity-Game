using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMoveController : MonoBehaviour
{
    public float lastPositionX;
    private Rigidbody2D rig;
    private Animator anim;
    private CharacterSoundController sound;

    [Header("Movement")]
    public float moveAccel;
    public float maxSpeed;

    [Header("GameOver")]
    public GameObject gameOverScreen;
    public float fallPositionY;

    [Header("Camera")]
    public CameraMoveController gameCamera;

    [Header("Scoring")]
    public ScoreController score;
    public float scoringRatio;

    [Header("Jump")]
    public float jumpAccel;
    private bool isJumping;
    private bool isOnGround;

    [Header("Ground Raycast")]
    public float groundRaycastDistance;
    public LayerMask groundLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sound = GetComponent<CharacterSoundController>();
    }

    // Update is called once per frame
    void Update()
    {
        //read input
        if(Input.GetMouseButtonDown(0)){
            if(isOnGround){ //if the character is standing on ground then it can jump
                isJumping = true;
                sound.PlayJump();
            }
        }
        //change animation 
        anim.SetBool("isOnGround", isOnGround);

        //calculate score
        int distancePassed = Mathf.FloorToInt(transform.position.x - lastPositionX);
        int scoreIncrement = Mathf.FloorToInt(distancePassed / scoringRatio);
        if(scoreIncrement > 0)
        {
            score.IncreaseCurrentScore(scoreIncrement);
            lastPositionX += distancePassed;
        }

        if(transform.position.y < fallPositionY)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        //set High Score
        score.FinishScoring();

        //stop camera movement
        gameCamera.enabled = false;

        //show game over 
        gameOverScreen.SetActive(true);

        //disable this
        this.enabled = false;
    }

    private void FixedUpdate()
    {
        //raycast ground
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundRaycastDistance, groundLayerMask);
        if(hit)
        {
            if(!isOnGround && rig.velocity.y <= 0) //velocity upwards is less than or equal to 0 and not on ground
            {
                isOnGround = true;
            }
        }
            else 
            {
                isOnGround = false;
            }

        
        Vector2 velocityVector = rig.velocity; //speed of object (character)
        //calculate velocity vector
        if(isJumping)
        {
            velocityVector.y += jumpAccel;
            isJumping = false;
        }

        velocityVector.x = Mathf.Clamp(velocityVector.x + moveAccel * Time.deltaTime, 0.0f, maxSpeed);
        rig.velocity = velocityVector;
        
    }

    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, transform.position + (Vector3.down * groundRaycastDistance), Color.white);
    }
}
