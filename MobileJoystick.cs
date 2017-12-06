using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MobileJoystick : MonoBehaviour
{
    public Image joystickTop;

    private bool isActive = false;
    private RectTransform myRect;
    private Vector2 angle;
    private int joystickTouchId;

    private void Start()
    {
        myRect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (isActive)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.fingerId == joystickTouchId)
                {
                    Touch joystickTouch = touch;
                    
                    if (joystickTouch.phase == TouchPhase.Ended || joystickTouch.phase == TouchPhase.Canceled)
                    {
                        joystickTop.transform.localPosition = new Vector2(0, 0);
                        isActive = false;
                        return;
                    }

                    Vector2 myPos = transform.position;
                    angle = (joystickTouch.position - myPos).normalized;

                    if (Vector2.Distance(joystickTouch.position, transform.position) <= myRect.rect.width / 2)
                    {
                        joystickTop.transform.position = joystickTouch.position;
                    }
                    else
                    {
                        joystickTop.transform.localPosition = angle * (myRect.rect.width / 2);
                    }
                }
            }
        }
        else
        {
            if (Input.touchCount > 0)
            {
                foreach (Touch touch in Input.touches)
                {
                    if (touch.phase == TouchPhase.Began && Vector2.Distance(touch.position, transform.position) <= myRect.rect.width / 2 + 20.0f)
                    {
                        joystickTouchId = touch.fingerId;
                        isActive = true;
                    }
                }
            }
        }
    }

    public Vector2 GetAngle()
    {
        if (isActive)
        {
            return angle;
        }
        else
        {
            return new Vector2(0, 0);
        }
    }
}
