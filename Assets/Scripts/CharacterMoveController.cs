using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMoveController : MonoBehaviour
{
    [Header("Movement")]
    public float moveAccel;
    public float maxSpeed;

    [Header("Jump")]
    public float jumpAccel;

    [Header("Ground Raycast")]
    public float groundRaycastDistance;
    public LayerMask groundLayerMask;

    [Header("Scoring")]
    public ScoreController score;
    public float scoringRatio;

    [Header("Game Over")]
    public GameObject gameOverScreen;
    public float fallPositionY;

    [Header("Camera")]
    public CameraMoveController gameCamera;

    private bool isJumping;
    private bool isOnGround;
    private float lastPositionX;

    private Rigidbody2D rig;
    private Animator anim;
    private CharacterSoundController audio;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audio = GetComponent<CharacterSoundController>();
        lastPositionX = transform.position.x;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0)) //keyboard space ditekan
        {
            if(isOnGround)
            {
                isJumping = true;
                audio.PlayJump();
            }
        }
        //change bool value for animator
        anim.SetBool("isOnGround", isOnGround);

        //calculate score
        int distancePassed = Mathf.FloorToInt(transform.position.x - lastPositionX);
        int scoreIncrement = Mathf.FloorToInt(distancePassed/scoringRatio);

        if(scoreIncrement > 0)
        {
            score.IncreaseCurrentScore(scoreIncrement);
            lastPositionX +=distancePassed;
        }

        //GAMEOVER
        if(transform.position.y < fallPositionY)
        {
            GameOver();
        }

    }

    void FixedUpdate()
    {
        //Raycast Ground, to check whether char is on ground or not
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundRaycastDistance, groundLayerMask);
        if(hit)
        {
            if(!isOnGround && rig.velocity.y <= 0)
            {
                isOnGround = true;
            }
        }
        else
        {
            isOnGround = false;
        }

        //Calculate Velocity Vector
        Vector2 velocityVector = rig.velocity;

        if(isJumping)
        {
            velocityVector.y += jumpAccel;
            isJumping = false;
        }
        velocityVector.x = Mathf.Clamp(velocityVector.x + moveAccel * Time.deltaTime, 0.0f, maxSpeed);

        rig.velocity = velocityVector;
        
    }

    private void GameOver()
    {
        score.FinishScoring();
        //stop camera movement
        gameCamera.enabled = false;
        //turn on game over panel
        gameOverScreen.SetActive(true);
        
        this.enabled = false;
    }

    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, transform.position + (Vector3.down * groundRaycastDistance), Color.white);
    }
}
