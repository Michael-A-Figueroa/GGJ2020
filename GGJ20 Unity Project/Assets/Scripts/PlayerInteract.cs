using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public bool useTool;

    public Animator anim;
    public float animLength;
    public float timer;

    public GameObject cameraObj;
    public CameraFocus cameraFocusScript;

    public GameObject tool;
    public int toolType;
    public bool hasTool;
    public bool dropTool;

    public Transform handTransform;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        cameraFocusScript = cameraObj.GetComponent<CameraFocus>();
    }

    // Update is called once per frame
    void Update()
    {
        //use tool input
        if (Input.GetAxis("Fire1") > 0)
        {
            useTool = true;
        }
        //use tool animation/interaction
        if (useTool)
        {
            anim.SetBool("useTool", useTool);

            if (cameraFocusScript.focusHit != null && cameraFocusScript.focusHit.GetComponent<PartBehavior>()) 
            {
                if (cameraFocusScript.focusHit.GetComponent<PartBehavior>().partType == toolType)
                {
                    Debug.Log("Right part");
                }
                else if (cameraFocusScript.focusHit.GetComponent<PartBehavior>().partType != toolType)
                {
                    Debug.Log("Wrong part");
                }
            }
            //timer for animation
            timer += Time.deltaTime;
            if (timer >= animLength)
            {
                useTool = false;
                anim.SetBool("useTool", useTool);
                timer = 0;
            }
        }
        //pickup/interact input
        if (Input.GetButtonDown("Fire2"))
        {
            //no tool and pickup tool that is focused on
            if (!hasTool && cameraFocusScript.focusHit.tag == "tool")
            {
                tool = cameraFocusScript.focusHit;
                toolType = tool.GetComponent<ToolBehavior>().toolType;
                tool.GetComponent<ToolBehavior>().toolTrigger.enabled = false;
                //tool.GetComponent<ToolBehavior>().toolCollider.isTrigger = true;
                tool.transform.position = handTransform.position;
                hasTool = true;
            }
            //switch tool 
            else if (hasTool && cameraFocusScript.focusHit.tag == "tool")
            {
                DropTool();
                tool = cameraFocusScript.focusHit;
                toolType = tool.GetComponent<ToolBehavior>().toolType;
                tool.GetComponent<ToolBehavior>().toolTrigger.enabled = false;
                //tool.GetComponent<ToolBehavior>().toolCollider.isTrigger = true;
                tool.transform.position = handTransform.position;
            }
            //drop tool
            else if (hasTool)
            {
                DropTool();
            }
        }

        if (hasTool)
        {
            tool.transform.position = handTransform.position;
        }
    }

    private void FixedUpdate()
    {
        if (dropTool)
        {
            
            dropTool = false;
        }
    }

    public void DropTool()
    {
        //tool.transform.position = tool.transform.position + new Vector3(0, 5, 0);
        //tool.GetComponent<ToolBehavior>().toolCollider.isTrigger = false;
        tool.GetComponent<ToolBehavior>().toolTrigger.enabled = true;
        tool = null;
        toolType = 0;
        hasTool = false;
    }
}
