using System;
using System.Security.Cryptography;
using Unity.Mathematics;
using UnityEngine;
using static UnityEngine.LowLevelPhysics2D.PhysicsShape;

public class BallMovement : MonoBehaviour
{
    int rand;
    int direction = 1;

    [SerializeField] int speedUpCounter;

    float timeElapsed;

    [SerializeField] float power;
    [SerializeField] float firstPower = 12.0f;
    [SerializeField] float powerUp = 2.0f;
    [SerializeField] float powerUpTimer;


    [SerializeField] Rigidbody ballRB;
    [SerializeField] GameObject ball;
    [SerializeField] GameObject gameManager;

    Vector3 directionNomalizer = new Vector3(1, 1, 0);
    Vector3 lastPosition;    

    private void OnEnable()
    {
        speedUpCounter = 0;
        power = firstPower;
        rand = 0;
        direction = 1;

        int playerOneScore = gameManager.GetComponent<GameManager>().scoreOne;
        int playerTwoScore = gameManager.GetComponent<GameManager>().scoreTwo;
        bool winnerPlayer = gameManager.GetComponent<GameManager>().WinnerPlayer;

        if (playerOneScore == 0 && playerTwoScore == 0)
        {
            rand = UnityEngine.Random.Range(0, 2);
        }

        if (rand == 1 || winnerPlayer)
        {
            direction *= -1;
        }

        ballRB.linearVelocity = Vector3.zero;
        ballRB.angularVelocity = Vector3.zero;
        ballRB.AddForce(Vector3.right * direction * power, ForceMode.VelocityChange);
    }

    // Update is called once per frame
    void Update()
    {
        BallSpeedUp(timeElapsed);
        timeElapsed = gameManager.GetComponent<GameTimer>().timeElapsed;
        if (ballRB != null)
        {
            Vector3 startPos = transform.position;
            Vector3 velocityDirection = ballRB.linearVelocity;
            Debug.DrawRay(startPos, velocityDirection, Color.green);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PlayerOne") || collision.gameObject.CompareTag("PlayerTwo"))
        {
            Transform childCollision = collision.transform.Find("PlayerReferencePoint");

            Vector3 referencePoint = childCollision.position;
            Vector3 ballDirection = (ball.transform.position - referencePoint).normalized;

            ballDirection.Scale(directionNomalizer);

            foreach (ContactPoint contact in collision.contacts)
            {
                Debug.DrawRay(referencePoint, ballDirection * 5, Color.blue, 10);
            }

            ballRB.linearVelocity = Vector3.zero;
            ballRB.angularVelocity = Vector3.zero;
            ballRB.AddForce(ballDirection * power, ForceMode.VelocityChange);

            lastPosition = ball.transform.position;
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            ContactPoint contactPoint = collision.GetContact(0);

            Vector3 ballDirection = ball.transform.position - lastPosition;
            Vector3 wallHitNormal = collision.GetContact(0).normal;

            Debug.DrawRay(contactPoint.point, ballDirection * 10, Color.red, 10);
            Debug.DrawRay(contactPoint.point, wallHitNormal, Color.green, 10);

            ballDirection.Scale(directionNomalizer);

            Vector3 reflectDirection = Vector3.Reflect(ballDirection, wallHitNormal).normalized;

            ballRB.linearVelocity = Vector3.zero;
            ballRB.angularVelocity = Vector3.zero;
            ballRB.AddForce(reflectDirection * power, ForceMode.VelocityChange);

            Debug.DrawRay(contactPoint.point, reflectDirection * 10, Color.blue, 10);

            lastPosition = ball.transform.position;

        }


    }

    void BallSpeedUp(float timer)
    {
        if (timer > 7 && (powerUpTimer * speedUpCounter) + 7 < timer)
        {
            power += powerUp;
            speedUpCounter++;
        }
    }

}
