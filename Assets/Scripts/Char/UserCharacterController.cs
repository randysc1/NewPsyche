using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class UserCharacterController : MonoBehaviour
{

    private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
    private Vector3 m_CamForward;             // The current forward direction of the camera
    private Vector3 m_Move;
    private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.

    void Start()
    {
        // get the third person character ( this should never be null due to require component )
        m_Character = GetComponent<ThirdPersonCharacter>();
    }

    // Update is called once per frame
    void Update()
    {

        if (!m_Jump)
        {
            m_Jump = Input.GetKeyDown(GameManager.GM.jump);
        }
    }

    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        m_Move = Vector3.zero;
        // read inputs
        if (Input.GetKey(GameManager.GM.forward))
        {
            m_Move += Vector3.forward;
        }

        if (Input.GetKey(GameManager.GM.backward))
        {
            m_Move += Vector3.back;
        }

        if (Input.GetKey(GameManager.GM.right))
        {
            m_Move += Vector3.right;
        }

        if (Input.GetKey(GameManager.GM.left))
        {
            m_Move += Vector3.left;
        }

        if (Input.GetKey(GameManager.GM.sprint))
        {
#if !MOBILE_INPUT
            m_Move *= 2f;
#endif
        }

        bool crouch = Input.GetKey(GameManager.GM.crouch);

        // pass all parameters to the character control script
        m_Character.Move(m_Move, crouch, m_Jump);
        m_Jump = false;

    }
}
