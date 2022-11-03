using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MilkShake;

public class PlayerControls : MonoBehaviour
{
    #region moveVars
    [SerializeField]
    private float moveSpeed = 3f;
    [SerializeField]
    private float jumpForce = 10;
    [SerializeField]
    private float airMoveSpeedOffset = 2;
    //For ground checking
    [SerializeField]
    private Vector3 feet;
    [SerializeField]
    private Vector3 feetExtents;

    // Momentum
    public Vector3 localVelocity;
    private Vector3 momentum;
    private float momentumLerp;
    // How long player maintains momentum
    [SerializeField]
    private float inertia = 0.5f;
    #endregion

    public ShakePreset gunfire;
    private Animator anim;
    private GameManager gm;

    private bool isReloading = false;

    [SerializeField] private LayerMask terrain;

    #region cameraVars
    private float rotX = 0;
    private float rotY = 0;
    [SerializeField]
    private float cameraSensitivity = 1f;
    [SerializeField]
    private float minY = 90;
    [SerializeField]
    private float maxY = 76;
    public GameObject cameraAnchor;
    private GameObject currentWeapon;
    #endregion



    
    private Vector3 inputs;
    private Rigidbody rb;

    public bool debugGrounded = false;

    private void Awake() {
        currentWeapon = ChangeWeapon();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        cameraAnchor = GetComponentInChildren<PlayerCamera>().gameObject;
        gm = FindObjectOfType<GameManager>();
    }
       
    // Start is called before the first frame update
    void Start()
    {
        //DEBUG
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        
        CameraMove();
        PlayerInput();
        //DEBUG
        debugGrounded = CheckGrounded();
        localVelocity = transform.InverseTransformDirection(rb.velocity);

        //Handle momentum lerp
        if (momentumLerp != 1) {
            momentumLerp += Time.deltaTime * inertia;
            momentumLerp = Mathf.Clamp01(momentumLerp);
        }
    }

    private void FixedUpdate() {
        Movement();
    }

    private void PlayerInput() {
        inputs = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        if (CheckGrounded()) {
            if (Input.GetButtonDown("Jump")) rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        // Catching momentum here to minimize ground checks
        else {
            momentum = transform.TransformDirection(localVelocity);
        }

        if (!isReloading) {
            if (Input.GetButtonDown("Primary")) {
                Fire();

            }

            if (Input.GetButtonDown("Reload")) {
                Reload();
            }
        }
        

        //DEBUG
        if (Input.GetKeyDown(KeyCode.K)) {
            if (Time.timeScale == 1)
                Time.timeScale = 0.2f;
            else Time.timeScale = 1;
        }

    }

    private void Movement() {
        Vector3 moveVector = transform.TransformDirection(inputs) * moveSpeed;
        if (!CheckGrounded()) {
            // Player is in the air!
            // Record speed reached
            rb.AddForce(moveVector * airMoveSpeedOffset);
            momentumLerp = 0;
        }
        else {
            // Player is on the floor :(
            if (inputs != Vector3.zero) {
                //get local velocity
                if (momentumLerp != 1) {
                    Vector3 finalMove = Vector3.Lerp(momentum, moveVector, momentumLerp);
                    rb.velocity = new Vector3(finalMove.x, rb.velocity.y, finalMove.z);
                }else
                rb.velocity = new Vector3(moveVector.x, rb.velocity.y, moveVector.z);
            }
        }
        
    }

    private void CameraMove() {
        rotX += cameraSensitivity * Input.GetAxis("Mouse X");
        rotY -= cameraSensitivity * Input.GetAxis("Mouse Y");
        rotY = Mathf.Clamp(rotY, minY, maxY);
        transform.eulerAngles = new Vector3(0, rotX, 0.0f);
        cameraAnchor.transform.localEulerAngles = new Vector3(rotY, 0, 0);
    }


    private bool CheckGrounded() {
        if (Physics.CheckBox(transform.position + feet, feetExtents, transform.rotation, terrain)) return true;
        else return false;
    }

    private void Fire() {
        if (gm.currentAmmo > 0) {
            gm.currentAmmo -= 1;
            cameraAnchor.GetComponentInChildren<Shaker>().Shake(gunfire);
            anim.SetTrigger("Fire");
            currentWeapon.GetComponent<Weapon>().Fire();
        }
        else {
            if (gm.CanReload())
            Reload();
        }
        
    }

    private IEnumerator ReloadTime(float seconds) {
        isReloading = true;
        yield return new WaitForSeconds(seconds);
        gm.Reload();
        isReloading = false;
    }
    private void Reload() {
        StartCoroutine(ReloadTime(gm.reloadLength));
        anim.SetTrigger("Reload");
    }

    private GameObject ChangeWeapon() {
        return GetComponentInChildren<Weapon>().gameObject;
    }

    // DEBUG STUFF
    void OnDrawGizmosSelected() {
        // Draw a semitransparent blue cube at the transforms position
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(transform.position + feet, feetExtents);
    }
}
