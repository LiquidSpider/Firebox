using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motor : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 3f;
    [SerializeField]
    private float rotSpeed = 10f;


    private CharacterController _characterController = null;

    // Start is called before the first frame update
    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    private void Update()
    {
        Move();
    }

    private void Move() {
        float vertical = Input.GetAxisRaw("Vertical");
        float horizontal = Input.GetAxisRaw("Horizontal");

        Vector3 next = new Vector3(0f, 0f, vertical * Time.deltaTime * moveSpeed);
        next += Physics.gravity * Time.deltaTime;

        Quaternion rot = Quaternion.Euler(new Vector3(0f, horizontal * Time.deltaTime * rotSpeed, 0f));
        transform.Rotate(new Vector3(0f, horizontal * Time.deltaTime * rotSpeed, 0f));
        _characterController.Move(transform.TransformDirection(next));
    }
}
