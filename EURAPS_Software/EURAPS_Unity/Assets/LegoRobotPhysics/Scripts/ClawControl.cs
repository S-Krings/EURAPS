using UnityEngine;

/* The ClawControl class takes care of the low-level control for the robot claw. Attach it directly to the claw component */
public class ClawControl : MonoBehaviour
{
    public enum clawstates { MOVEUP, MOVEDOWN, HOLD };        /* States of movement in which the claw can be */
    public enum clawpositions { UP, DOWN, INBETWEEN};   /* The positions the claw can be in, for easy external readability */

    public clawstates clawstate = clawstates.MOVEUP;    /* The current movement state of the claw */
    public clawpositions clawposition;    /* The current position of the claw (for debugging)*/
    public int motorForce = 10;                         /* The force the morot can use to move the claw, must be adjusted on size changes due to different claw weight */
    public int motorTargetVelocity = 10;                /* The speed the claw should have when moving */
    private JointMotor motor;                           /* The morot moving the claw (is a feature of the hingejoint attaching it to the rest of the robot */

    public bool enableLoggingMessages = false;          /* If debugguing messages should be logged */

    private AudioSource grabbingAudio;

    /* On start just get motor from hingejoint on claw */
    void Start()
    {
        motor = this.GetComponent<HingeJoint>().motor;  
        grabbingAudio = GetComponent<AudioSource>();
    }

    public void setState(clawstates state)
    {
        clawstate = state;
    }

    private void LateUpdate()
    {
        //Debug.Log("HOLDlate" + this.GetComponent<Rigidbody>().freezeRotation);

    }
    /* Called once per frame by unity, depending on clawstate, set appropriate values of target velocity and force to the motor */
    void FixedUpdate()
    {
        //this.GetComponent<Rigidbody>().freezeRotation = false;
        switch (clawstate)
        {
            case clawstates.HOLD:
                //Debug.Log("HOLDbefore"+ this.GetComponent<Rigidbody>().freezeRotation);
                //this.GetComponent<Rigidbody>().freezeRotation = true;
                //Debug.Log("HOLDafter" + this.GetComponent<Rigidbody>().freezeRotation);
                setMotor(0,motorForce);
                break;
            case clawstates.MOVEDOWN:
                //Debug.Log("MOVEDOWN"+ this.GetComponent<Rigidbody>().freezeRotation);
                //this.GetComponent<Rigidbody>().freezeRotation = false;
                setMotor(motorTargetVelocity, motorForce);
                break;
            case clawstates.MOVEUP:
                //Debug.Log("MOVEUP"+ this.GetComponent<Rigidbody>().freezeRotation);
                //this.GetComponent<Rigidbody>().freezeRotation = false;
                setMotor(-motorTargetVelocity, motorForce);
                break;
        }
        if (enableLoggingMessages) { Debug.Log("Rotation: " + this.transform.rotation.eulerAngles + " localRotation: " + this.transform.localRotation.eulerAngles); }
    }

    /* If the motor does not have the given values for targetVelocity and force, set them */
    public void setMotor(int targetVelocity, int force)
    {
        if (motor.targetVelocity != targetVelocity || motor.force != force)
        {
            motor.targetVelocity = targetVelocity;
            motor.force = force;
            this.GetComponent<HingeJoint>().motor = motor;
        }
    }

    public clawstates getState()
    {
        return clawstate;
    }

    /* Calculate clawposition from the rotation of the claw object and return it */
    public clawpositions getClawPosition()
    {
        if (this.transform.localEulerAngles.x <= 285f)
        {
            return clawpositions.UP;
        }
        else
        {
            if(this.transform.localEulerAngles.x >= 335f)
            {
                return clawpositions.DOWN;
            }
            else
            {
                return clawpositions.INBETWEEN;
            }
        }
    }

    public void setEnableLoggingMessages(bool logging)
    {
        enableLoggingMessages = logging;
    }

    public void playSound()
    {
        grabbingAudio.Play();
    }
}
