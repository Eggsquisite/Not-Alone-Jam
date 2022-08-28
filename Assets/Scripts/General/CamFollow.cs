using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    [Header("Player Targets")]
    [SerializeField] private Transform flashPlayer;
    [SerializeField] private Transform meleePlayer;
    [SerializeField] private float moveSpeed;

    private Transform activePlayer;


    [Header("Camera min/max positions")]
    [SerializeField] private float xMin;
    [SerializeField] private float xMax;
    [SerializeField] private float yMin;
    [SerializeField] private float yMax;
    [SerializeField] private Vector3 cameraOffset;
    private Vector3 velocity = Vector3.zero;
    private Vector3 target;
    private int gameState;

    // Start is called before the first frame update
    void Start()
    {
        gameState = GameManager._instance.GetGameState();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        FollowPlayer();
    }

    private void FollowPlayer() {
        if (gameState == 1) {
            if (flashPlayer.GetComponent<PlayerController>().GetActivePlayer())
                target = flashPlayer.position;
            else if (meleePlayer.GetComponent<PlayerController>().GetActivePlayer())
                target = meleePlayer.position;
        }
        else if (gameState == 2) {
            if (flashPlayer.position.x > meleePlayer.position.x) {
                target = flashPlayer.position - meleePlayer.position;
            } else if (meleePlayer.position.x >= flashPlayer.position.x) {
                target = meleePlayer.position - flashPlayer.position;
            }
        }

        Vector3 targetPos = target + cameraOffset;
        Vector3 clampedPos = new Vector3(Mathf.Clamp(targetPos.x, xMin, xMax), transform.position.y, transform.position.z);
        Vector3 smoothPos = Vector3.SmoothDamp(transform.position, clampedPos, ref velocity, moveSpeed * Time.deltaTime);

        transform.position = smoothPos;
    }
}
