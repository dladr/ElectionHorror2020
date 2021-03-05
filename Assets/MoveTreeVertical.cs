using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTreeVertical : MonoBehaviour
{
    public float YTargetUp;

    public float YTargetDown;

    public Vector3 YTargetCurrent;

    public float Speed;

    public float UpSpeed;

    public float DownSpeed;

    public bool IsMoving;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(IsMoving)
            MoveTowardsTargetY();
    }

    public void MoveVertical(bool isUp)
    {
        var localPosition = transform.localPosition;
        YTargetCurrent = isUp ? new Vector3(localPosition.x, YTargetUp, localPosition.z): new Vector3(localPosition.x, YTargetDown, localPosition.z);
        Speed = isUp ? UpSpeed : DownSpeed;
        IsMoving = true;
    }

    void MoveTowardsTargetY()
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, YTargetCurrent, Speed * Time.deltaTime);

        if (Vector3.Distance(transform.localPosition, YTargetCurrent) < .01f)
            IsMoving = false;
    }
}
