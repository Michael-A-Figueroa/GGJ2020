using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool canMove = true;

    public float h;
    public float v;

    public float movementSpeed = 0.1f;
    public Vector3 moveVect;

    public Transform cam;
    public Vector3 camForward;
    public float camAngle;

    public Vector3 lookVect;
    public Vector3 targetDirection;
    public Quaternion lookRotation;
    [Tooltip("The speed at which the player character will rotate to face the same direction as the camera.")]
    public float rotationSpeed = 10f;

    public Animator anim;
    public float animDampTime = 0.1f;


    // Start is called before the first frame update
    void Start()
    {
        //check for and get main camera
        if (Camera.main != null)
        {
            cam = Camera.main.transform;
        }
        else
        {
            Debug.LogWarning("Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\".", gameObject);
        }

        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");
        }

        camForward = Vector3.Scale(cam.forward, new Vector3(1, 0, 1)).normalized;
        moveVect = v * camForward + h * cam.right;

        transform.position += moveVect * movementSpeed;

        #region Face Forward
        //get angle between player character's forward and the camera's forward
        camAngle = Vector3.SignedAngle(transform.forward, cam.forward, Vector3.up);
        //vector going from player towards camera's forward
        lookVect = new Vector3(transform.position.x + cam.forward.x, transform.position.y, transform.position.z + cam.forward.z);
        //the target direction for the player to rotate towards
        targetDirection = (lookVect - transform.position).normalized;
        //quaternion representing the rotation to the targetDirection
        lookRotation = Quaternion.LookRotation(targetDirection);

        //check for input and rotate when player tries to move
        if (h != 0 || v!= 0)
        {
            //smoothly slerp from current rotation to target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }

        #endregion

        #region Animation
        anim.SetFloat("moveX", h, animDampTime, Time.deltaTime);
        anim.SetFloat("moveY", v, animDampTime, Time.deltaTime);
        #endregion
    }
}
