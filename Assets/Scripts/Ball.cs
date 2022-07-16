using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private Vector3 startingPosition;


    [Header("Inputs")]
    [SerializeField] private KeyCode moveLeft = KeyCode.LeftArrow;
    [SerializeField] private KeyCode moveRight = KeyCode.RightArrow;
    [SerializeField] private float moveSpeed = 3;

    [SerializeField]private int ballPositionOnFloor = 1;

    void Start()
    {

    }

    void Update()
    {
        if (transform.localPosition.y < -5)
            transform.localPosition = startingPosition;

        MoveBall();
    }

    private void MoveBall()
    {
        transform.Translate(new Vector3(GetInputs(), 0, 0) * moveSpeed * Time.deltaTime, Space.World);
    }

    private int GetInputs()
    {
        if (Input.GetKey(moveLeft))
            return -1;
        else if (Input.GetKey(moveRight))
            return 1;
        else return 0;
    }


    public int GetBallPosition()
    {
        return ballPositionOnFloor;
    }

    public void UpdateBallPosition(int _posiiton)
    {
        ballPositionOnFloor = _posiiton;
    }


    private void OnCollisionEnter(Collision collision)
    {
        ballPositionOnFloor = collision.transform.GetSiblingIndex() + 1;
        MainController.Instance.UpdateRowNumber(ballPositionOnFloor);
    }

}
