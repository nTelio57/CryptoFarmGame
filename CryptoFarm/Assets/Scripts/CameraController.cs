using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector2 oldTouch;
    private Vector2 lastTouch;
    private Vector2 swipeDelta;

    private Vector3 movePosition;

    //B varianto
    [Header("B variantas")]
    public int speed = 4;
    public Camera selectedCamera;
    public float MINSCALE = 2.0F;
    public float MAXSCALE = 5.0F;
    public float minPinchSpeed = 5.0F;
    public float varianceInDistances = 5.0F;
    private float touchDelta = 0.0F;
    private Vector2 prevDist = new Vector2(0, 0);
    private Vector2 curDist = new Vector2(0, 0);
    private float speedTouch0 = 0.0F;
    private float speedTouch1 = 0.0F;
    private float startAngleBetweenTouches = 0.0F;
    private int vertOrHorzOrientation = 0; //this tells if the two fingers to each other are oriented horizontally or vertically, 1 for vertical and -1 for horizontal
    private Vector2 midPoint = new Vector2(0, 0); //store and use midpoint to check if fingers exceed a limit defined by midpoint for oriantation of fingers



    //C varianto
    [Header("C variantas")]
    public float PanSpeed = 0.05f;
    public float PinchSpeed = 0.1f;
    public float MinBoundX = -12.8112f; // Minimum Bondary From Center On Xaxis
    public float MaxBoundX = 12.79478f; // Maximum Bondary From Center On Xaxis
    public float MinBoundY = -10.80061f; // Minimum Bondary From Center On Yaxis
    public float MaxBoundY = 9.691292f; // Maximum Bondary From Center On Yaxis
    private float CamBoundX;
    private float CamBoundY;
    private float CamWorldMinX;
    private float CamWorldMinY;
    private float CamWorldMaxX;
    private float CamWorldMaxY;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Bvariantas();
        //Cvariantas();
    }

    public void Cvariantas()
    {
        if (Camera.main.orthographicSize < 0)
        {
            Camera.main.orthographicSize = Camera.main.orthographicSize * -1;
            transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, 0, transform.rotation.w);
        }
        if (transform.rotation.z != 0)
            transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, 0, transform.rotation.w);
        CamBoundX = Camera.main.OrthographicBounds().size.x / 2;
        CamBoundY = Camera.main.OrthographicBounds().size.y / 2;
        CamWorldMinX = Camera.main.transform.position.x - CamBoundX;
        CamWorldMinY = Camera.main.transform.position.y - CamBoundY;
        CamWorldMaxX = Camera.main.transform.position.x + CamBoundX;
        CamWorldMaxY = Camera.main.transform.position.y + CamBoundY;
        var temporary = new Vector3();
        if ((transform.position.x - CamBoundX) <= MinBoundX)
            temporary.x = MinBoundX + CamBoundX;
        if (transform.position.x == MinBoundX + CamBoundX && transform.position.x == MaxBoundX + CamBoundX)
            if ((transform.position.y - CamBoundY) <= MinBoundY)
                temporary.y = MinBoundY + CamBoundY;
        if ((transform.position.x + CamBoundX) >= MaxBoundX)
            temporary.x = MaxBoundX - CamBoundX;
        if ((transform.position.y + CamBoundY) >= MaxBoundY)
            temporary.y = MaxBoundY - CamBoundY;
        if ((CamWorldMinX <= MinBoundX) && (CamWorldMaxX >= MaxBoundX))
            temporary.x = 0f;
        temporary.z = transform.position.z;
        if ((transform.position.x - CamBoundX) <= MinBoundX || (transform.position.y - CamBoundY) <= MinBoundY || (transform.position.x + CamBoundX) >= MaxBoundX || (transform.position.y + CamBoundY) >= MaxBoundY)
            transform.position = temporary;
        if ((CamWorldMinX < MinBoundX) && (CamWorldMaxX > MaxBoundX))
            Camera.main.orthographicSize -= (CamWorldMaxX - MaxBoundX);
    }

    void FixedUpdate()
    {
        /*// Check if we have one finger down, and if it's moved.
        // You may modify this first portion to '== 1', to only allow pinching or panning at one time.
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            var touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            var temporary = transform.position;
            temporary -= new Vector3(touchDeltaPosition.x * PanSpeed, 0f, touchDeltaPosition.y * PanSpeed);
            if (((temporary.x - CamBoundX) >= MinBoundX) && ((temporary.x + CamBoundX) <= MaxBoundX) &&
                ((temporary.y - CamBoundY) >= MinBoundY) && ((temporary.y + CamBoundY) <= MaxBoundY))
                // Translate along world cordinates. (Done this way so we can angle the camera freely.)
                transform.position = temporary;
        }

        // Check if we have two fingers down.
        if (Input.touchCount == 2)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved)
            {
                Touch touch1 = Input.GetTouch(0);
                Touch touch2 = Input.GetTouch(1);

                // Find out how the touches have moved relative to eachother.
                Vector2 curDist = touch1.position - touch2.position;
                Vector2 prevDist = (touch1.position - touch1.deltaPosition) - (touch2.position - touch2.deltaPosition);

                float touchDelta = -(curDist.magnitude - prevDist.magnitude);

                // Translate along local coordinate space.
                var _value = Camera.main.orthographicSize;
                _value += (touchDelta * PinchSpeed);
                if (_value >= 0.9 && (_value + touchDelta * PinchSpeed) >= 0.9f)
                    if ((CamWorldMinX > MinBoundX) && (CamWorldMaxX < MaxBoundX))
                    {
                        Camera.main.orthographicSize = _value;
                    }
                    else if (_value < Camera.main.orthographicSize)
                    {
                        Camera.main.orthographicSize = _value;
                    }
            }
        }*/
    }

    //VEIKIANTIS ZOOM
        public void Bvariantas()
    {
        if (Input.touchCount == 2 && Input.GetTouch(0).phase == TouchPhase.Began && Input.GetTouch(1).phase == TouchPhase.Began)
        {
            startAngleBetweenTouches = Vector2.Angle(Input.GetTouch(0).position, Input.GetTouch(1).position); //to determine that gesture isnt a rotation
            midPoint = new Vector2((Input.GetTouch(0).position.x - Input.GetTouch(1).position.x), (Input.GetTouch(0).position.y - Input.GetTouch(1).position.y)); //store midpoint from first touches

            if ((Input.GetTouch(0).position.x - Input.GetTouch(1).position.x) > (Input.GetTouch(0).position.y - Input.GetTouch(1).position.y))
            {
                vertOrHorzOrientation = -1;
            }
            if ((Input.GetTouch(0).position.x - Input.GetTouch(1).position.x) < (Input.GetTouch(0).position.y - Input.GetTouch(1).position.y))
            {
                vertOrHorzOrientation = 1;
            }
        }
        if (Input.touchCount == 2 && Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved)
        {

            curDist = Input.GetTouch(0).position - Input.GetTouch(1).position; //current distance between finger touches
            prevDist = ((Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition) - (Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition)); //difference in previous locations using delta positions
            touchDelta = curDist.magnitude - prevDist.magnitude;
            speedTouch0 = Input.GetTouch(0).deltaPosition.magnitude / Time.deltaTime;
            speedTouch1 = Input.GetTouch(1).deltaPosition.magnitude / Time.deltaTime;


            //(curDist < prevDist --- inward pinch) && (if touch0 is fast enough) && (if touch1 is fast enough) && (angle of two touches-using vector2.angle-is less than an angle limit to make sure pinch isnt a rotation)
            if ((touchDelta < 1) && (speedTouch0 > minPinchSpeed) && (speedTouch1 > minPinchSpeed) && (Vector2.Angle(Input.GetTouch(0).position, Input.GetTouch(1).position) < (startAngleBetweenTouches + 10)) && (Vector2.Angle(Input.GetTouch(0).position, Input.GetTouch(1).position) > (startAngleBetweenTouches - 10))) //
            {
                //(if the two touches are oriented vertically) && (if touch0 x is less than right side of midpoint x) or (if touch0 x is greater than left of midpoint x) && (if touch1 x is less than right side of midpoint x) or (if touch1 x is greater than left of midpoint x) (all values are assuming left and right variances of 20 pixels)
                if ((vertOrHorzOrientation == 1) && ((Input.GetTouch(0).position.x < midPoint.x + 20) || (Input.GetTouch(0).position.x > midPoint.x - 20)) && ((Input.GetTouch(1).position.x < midPoint.x + 20) || (Input.GetTouch(1).position.x > midPoint.x - 20)))
                {
                    selectedCamera.fieldOfView = Mathf.Clamp(selectedCamera.fieldOfView + (1 * speed), 15, 90);
                }
                //(same as above, but these checks are for horizontal orientation of touches and the y components are checked for limiting the direction)
                if ((vertOrHorzOrientation == -1) && ((Input.GetTouch(0).position.y < midPoint.y + 20) || (Input.GetTouch(0).position.y > midPoint.y - 20)) && ((Input.GetTouch(1).position.y < midPoint.y + 20) || (Input.GetTouch(1).position.y > midPoint.y - 20)))
                {
                    selectedCamera.fieldOfView = Mathf.Clamp(selectedCamera.fieldOfView + (1 * speed), 15, 90);
                }
            }

            if ((touchDelta > 1) && (speedTouch0 > minPinchSpeed) && (speedTouch1 > minPinchSpeed) && (Vector2.Angle(Input.GetTouch(0).position, Input.GetTouch(1).position) < (startAngleBetweenTouches + 10)) && (Vector2.Angle(Input.GetTouch(0).position, Input.GetTouch(1).position) > (startAngleBetweenTouches - 10)))
            {
                if ((vertOrHorzOrientation == 1) && ((Input.GetTouch(0).position.x < midPoint.x + 20) || (Input.GetTouch(0).position.x > midPoint.x - 20)) && ((Input.GetTouch(1).position.x < midPoint.x + 20) || (Input.GetTouch(1).position.x > midPoint.x - 20)))
                {
                    selectedCamera.fieldOfView = Mathf.Clamp(selectedCamera.fieldOfView - (1 * speed), 15, 90);
                }

                if ((vertOrHorzOrientation == -1) && ((Input.GetTouch(0).position.y < midPoint.y + 20) || (Input.GetTouch(0).position.y > midPoint.y - 20)) && ((Input.GetTouch(1).position.y < midPoint.y + 20) || (Input.GetTouch(1).position.y > midPoint.y - 20)))
                {
                    selectedCamera.fieldOfView = Mathf.Clamp(selectedCamera.fieldOfView - (1 * speed), 15, 90);
                }
            }

        }
    }

    //Pusiau dirba A
    public void ScreenPan(Vector2 curPosition, Vector2 lastPosition)
    {
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                oldTouch = touch.position;
                lastTouch = touch.position;
            }

            if (touch.phase == TouchPhase.Moved)
            {
                lastTouch = touch.position;
                ScreenPan(lastTouch, oldTouch);
                oldTouch = lastTouch;
            }
        }



        // create a ray from the current touch point into the world
        Ray curPosRay = Camera.main.ScreenPointToRay(curPosition);

        // fire that ray out and see if it hits anything 
        RaycastHit curHitInfo;
        if (Physics.Raycast(curPosRay, out curHitInfo))
        {
            // we have a hit, do the same for the last position
            Ray lastPosRay = Camera.main.ScreenPointToRay(lastPosition);
            RaycastHit lastHitInfo;
            if (Physics.Raycast(lastPosRay, out lastHitInfo))
            {
                // get the delta from those positions
                Vector3 deltaPos = curHitInfo.point - lastHitInfo.point;
                Debug.Log(deltaPos);

                // zap y changes, we're only concerned about the x/z plane
                deltaPos.y = 0;

                // reverse the direction to negate the movement such that the touch point
                // stays over the same spot once the camera moves.
                deltaPos *= -1;

                // apply the pan
                transform.position += deltaPos;
            }
        }
    }

    void ControlMovement()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition * Time.deltaTime;
            transform.Translate(-touchDeltaPosition.x * speed, -touchDeltaPosition.y * speed, 0);
            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, -1f, 1f),
                Mathf.Clamp(transform.position.y, 4.5f, 5.5f),
                Mathf.Clamp(transform.position.z, -7.19f, -7.19f)
            );
        }
    }
}
