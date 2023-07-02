using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed;         // Speed at which the character moves
    public float jumpForce = 5f;         // Force applied to jump
    public float lerpingSpeed = 5f;      // Speed at which the character lerps its position

    private bool isMoving;               // Flag to check if the character is currently moving
    private bool isJumping;              // Flag to check if the character is currently jumping
    private float targetXPosition;       // The target x position to lerp towards
    private int position;
    public Animator animator;

    private Vector2 touchStartPosition;  // Start position of the touch input
    private Vector2 touchEndPosition;    // End position of the touch input
    private const float swipeThreshold = 50f;  // Minimum distance required for a swipe
    public GameObject gameOver;
    public bool isGameOver;

    public EggScore score;
    private void Start()
    {
        animator = GetComponent<Animator>();
        position = 2;
        isGameOver = false;
    }

    private void Update()
    {
        if (score.eggScore > 10)
        {
            moveSpeed = 15f;
        }
        else if (score.eggScore > 50)
        {
            moveSpeed = 20f;
        }
        
        // Check for input to start or stop character movement
        if (Input.GetKeyDown(KeyCode.W) && !isMoving)
        {
            StartMoving();
        }
        else if (Input.GetKeyDown(KeyCode.S) && isMoving)
        {
            StopMoving();
        }

        // Check for input to jump
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            Jump();
        }

        // Move the character forward if it's currently moving
        if (isMoving)
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }

        // Check for swipe input
        if (Input.touchCount > 0 && !isGameOver && isMoving)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchStartPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                touchEndPosition = touch.position;
                float swipeDistance = (touchEndPosition - touchStartPosition).magnitude;

                if (swipeDistance >= swipeThreshold)
                {
                    Vector2 swipeDirection = touchEndPosition - touchStartPosition;

                    // Check if the swipe is horizontal
                    if (Mathf.Abs(swipeDirection.x) > Mathf.Abs(swipeDirection.y))
                    {
                        if (swipeDirection.x > 0)
                        {
                            SwipeRight();
                        }
                        else
                        {
                            SwipeLeft();
                        }
                    }
                    // Check if the swipe is vertical
                    else if (swipeDirection.y > 0)
                    {
                        SwipeUp();
                    }
                }
            }
        }

        // Apply lerping to the character's position
        Vector3 targetPosition = new Vector3(targetXPosition, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, lerpingSpeed * Time.deltaTime);
    }

    public void StartMoving()
    {

        animator.SetBool("Turn Head", false);
        animator.SetBool("Run", true);
        isMoving = true;
    }

    public void StopMoving()
    {
        isMoving = false;
        animator.SetBool("Run", false);
        animator.SetBool("Turn Head", true);
        gameOver.SetActive(true);
        isGameOver = true;
    }

    private void Jump()
    {
        // Apply upward force to jump
        GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isJumping = true;

        // Set a timer to reset the isJumping flag after a short delay
        Invoke("ResetJump", 0.7f);
    }

    private void ResetJump()
    {
        isJumping = false;
    }

    private void SwipeLeft()
    {
        if (position == 2)
        {
            targetXPosition = -2.5f;
            position = 1;
        }
        else
        {
            targetXPosition = 0f;
            position = 2;
        }
    }

    private void SwipeRight()
    {
        if (position == 2)
        {
            targetXPosition = 2.5f;
            position = 3;
        }
        else
        {
            targetXPosition = 0f;
            position = 2;
        }
    }

    private void SwipeUp()
    {
        if (!isJumping)
        {
            Jump();
        }
    }
}
