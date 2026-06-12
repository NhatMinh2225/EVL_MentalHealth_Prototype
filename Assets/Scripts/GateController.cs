using UnityEngine;

public class GateController : MonoBehaviour
{
    public Transform leftHinge;
    public Transform rightHinge;
    public float openAngle = 90f;
    public float speed = 2f;

    private bool isOpen = false;
    private Quaternion leftClosed, rightClosed;
    private Quaternion leftOpen, rightOpen;

    void Start()
    {
        leftClosed = leftHinge.localRotation;
        rightClosed = rightHinge.localRotation;
        leftOpen  = leftClosed  * Quaternion.Euler(0, openAngle, 0);
        rightOpen = rightClosed * Quaternion.Euler(0, -openAngle, 0);
    }

    void Update()
    {
        Quaternion lTarget = isOpen ? leftOpen : leftClosed;
        Quaternion rTarget = isOpen ? rightOpen : rightClosed;
        leftHinge.localRotation  = Quaternion.Slerp(leftHinge.localRotation,  lTarget, Time.deltaTime * speed);
        rightHinge.localRotation = Quaternion.Slerp(rightHinge.localRotation, rTarget, Time.deltaTime * speed);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) isOpen = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) isOpen = false;
    }
}