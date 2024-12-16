using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 5f;
    public float rotationSpeed = 90f;
    public float zoomSpeed = 1f;
    public float interactionDelay = 0.2f; 

    private float lastInteractionTime = 0f;

    void Update()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * movementSpeed * Time.deltaTime;
        transform.Translate(movement);

        if (Input.GetButtonDown("A") && Time.time - lastInteractionTime >= interactionDelay)
        {
            Interact();
            lastInteractionTime = Time.time;
        }
        else if (Input.GetButtonDown("B")) Rotate();
        else if (Input.GetButtonDown("X")) ZoomIn();
        else if (Input.GetButtonDown("Y")) ZoomOut();
        else if (Input.GetButtonDown("Start")) PauseGame();
        else if (Input.GetButtonDown("Back")) QuitGame();
    }

    void Interact() { Debug.Log("Interacting with object"); }
    void Rotate() { transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime); }
    void ZoomIn() { Camera.main.orthographicSize -= zoomSpeed; }
    void ZoomOut() { Camera.main.orthographicSize += zoomSpeed; }
    void PauseGame() { Debug.Log("Pausing game"); }
    void QuitGame() { Debug.Log("Quitting game"); }
}