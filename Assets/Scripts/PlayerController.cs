using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float speed = 5.0f;

    float areaLimit = 3.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (player.transform.position.y > -areaLimit && (Input.GetKey(KeyCode.S) && player.CompareTag("PlayerOne") || Input.GetKey(KeyCode.DownArrow) && player.CompareTag("PlayerTwo")))
        {
            player.transform.position += Vector3.down * speed * Time.deltaTime;

        }
        if (player.transform.position.y < areaLimit && (Input.GetKey(KeyCode.W) && player.CompareTag("PlayerOne") || Input.GetKey(KeyCode.UpArrow) && player.CompareTag("PlayerTwo")))
        {
            player.transform.position += Vector3.up * speed * Time.deltaTime;
        }
    }
}
