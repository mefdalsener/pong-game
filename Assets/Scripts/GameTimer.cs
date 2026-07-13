using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public float timeElapsed = 0.0f;
    void Update()
    {
        timeElapsed += Time.deltaTime;
        Debug.Log("Time Elapsed: " + timeElapsed);
    }
}
