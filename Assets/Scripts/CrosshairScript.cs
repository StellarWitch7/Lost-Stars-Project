using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairScript : MonoBehaviour
{
    public float MaxSpread = 4;
    public float MinSpread = 0.1f;
    public float CurrentSpread = 1;
    public RectTransform CrosshairLineWest;
    public RectTransform CrosshairLineNorth;
    public RectTransform CrosshairLineEast;
    public RectTransform CrosshairLineSouth;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCrosshair();
    }

    public void UpdateCrosshair()
    {
        CurrentSpread = CurrentSpread < MinSpread ? MinSpread : CurrentSpread;
        CurrentSpread = CurrentSpread > MaxSpread ? MaxSpread : CurrentSpread;

        CrosshairLineWest.SetLocalPositionAndRotation(new Vector3(CurrentSpread * -1, 0), Quaternion.Euler(0, 0, 180));
        CrosshairLineNorth.SetLocalPositionAndRotation(new Vector3(0, CurrentSpread * 1), Quaternion.Euler(0, 0, 90));
        CrosshairLineEast.SetLocalPositionAndRotation(new Vector3(CurrentSpread * 1, 0), Quaternion.Euler(0, 0, 0));
        CrosshairLineSouth.SetLocalPositionAndRotation(new Vector3(0, CurrentSpread * -1, 0), Quaternion.Euler(0, 0, 270));
    }
}
