using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarController : MonoBehaviour
{
    [System.Serializable]
    public class AxleInfo
    {
        public WheelCollider leftWheel;
        public WheelCollider rightWheel;
        public bool motor;
        public bool steering;
        public bool handbrake;
        public bool rear;
    }
    // Start is called before the first frame update
    public List<AxleInfo> axleInfos;
    public float maxSteeringAngle;
    public float flywheelWeight = 5;
    public float revLimit = 7000f;

    public AnimationCurve torqueCurve;
    public AnimationCurve gearRatios;
    public float finalDrive;

    public float brakingPower = 100;

    private float rpm;
    private float minRpm = 750;
    private float wheelRpm;
    private float gear = 1;

    private float speed = 0;
    private float wheelRadius = 0;

    public Text gearText;
    public Text rpmText;
    public Text speedText;
    public Image handbrakeImage;

    public Sprite handbrakeOn;
    public Sprite handbrakeOff;

    public Speedometer speedometer;
    public Tachometer tachometer;

    public float clutchTimer = 0.2f;

    public Transform steeringWheel;
    public float steeringrack = 900;

    float tmp;


    // finds the corresponding visual wheel
    // correctly applies the transform
    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
        {
            return;
        }

        Transform visualWheel = collider.transform.GetChild(0);

        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }

    private void Start()
    {
        //GetComponent<Rigidbody>().centerOfMass = GameObject.Find("CenterOfMass").transform.position;
    }

    public void Update()
    {
        float accelerate = 0;
        float brake = 0;
        bool handbrake = false;

        if (Input.GetButtonDown("Shift"))
        {
            ChangeGear();
        }

        if (Input.GetButton("Jump"))
        {
            handbrake = true;
            handbrakeImage.sprite = handbrakeOn;
        }
        else
        {
            handbrakeImage.sprite = handbrakeOff;
        }

        if (Input.GetAxisRaw("Throttle") > 0)
        {
            accelerate = Input.GetAxis("Throttle");
        }
        if (Input.GetAxisRaw("Brake") != 0)
        {          
            if(Input.GetAxisRaw("Brake") > 0)
            {
                brake = Input.GetAxisRaw("Brake");
            }
        }

        

        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.motor)
            {
                wheelRpm = (axleInfo.leftWheel.rpm + axleInfo.rightWheel.rpm) / 2;

                wheelRadius = (axleInfo.leftWheel.radius + axleInfo.rightWheel.radius) / 2;
                float wheelCircumference = 2f * Mathf.PI * wheelRadius;
                speed = (wheelCircumference * wheelRpm) * 60 / 1000;
                if (speed < 0)
                {
                    speed = -speed;
                }                  
            }
        }

        if(gear == 1 && rpm < revLimit)
        {
            if (accelerate > 0)
            {
                rpm += accelerate * flywheelWeight * 10;
            }
            else if (rpm >= minRpm)
            {
                rpm -= flywheelWeight * 10;
            }
            else
            {
                rpm = minRpm;
            }
        }
        else
        {
            rpm = Mathf.SmoothDamp(rpm, minRpm + (wheelRpm * finalDrive * gearRatios.Evaluate(gear)), ref tmp, clutchTimer);
        }

        float motor = torqueCurve.Evaluate(rpm) * gearRatios.Evaluate(gear) * finalDrive * accelerate;

        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");
        steeringWheel.localRotation = Quaternion.Euler(-166.374f, 0f, steeringrack * Input.GetAxis("Horizontal"));

        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motor / 2;
                axleInfo.rightWheel.motorTorque = motor / 2;            
            }
            if (axleInfo.rear)
            {
                axleInfo.leftWheel.brakeTorque = brakingPower * brake * 0.3f;
                axleInfo.rightWheel.brakeTorque = brakingPower * brake * 0.3f;
            }
            else 
            {
                axleInfo.leftWheel.brakeTorque = brakingPower * brake * 0.7f;
                axleInfo.rightWheel.brakeTorque = brakingPower * brake * 0.7f;
            }
            if(axleInfo.handbrake && handbrake)
            {
                axleInfo.leftWheel.brakeTorque = brakingPower * 0.3f;
                axleInfo.rightWheel.brakeTorque = brakingPower * 0.3f;
            }            

            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }

        rpmText.text = Mathf.RoundToInt(rpm).ToString();
        speedText.text = Mathf.RoundToInt(speed).ToString();
        speedometer.speed = speed;
        tachometer.rpm = rpm;
        if(gear == 0)
        {
            gearText.text = "R";
        }
        else if(gear == 1)
        {
            gearText.text = "N";
        }
        else
        {
            gearText.text = (gear - 1).ToString();
        }
    }

    private void ChangeGear()
    {
        if(Input.GetAxisRaw("Shift") > 0 && gear < gearRatios.length - 1)
        {
            gear++;
        }
        else if(Input.GetAxisRaw("Shift") < 0 && gear > 0)
        {
            gear--;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Finish")
        {
            other.GetComponent<Timer>().stage = false;
        }
    }
}
