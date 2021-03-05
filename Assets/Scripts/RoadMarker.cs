using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadMarker : MonoBehaviour
{
    private bool _isOpen;

    [SerializeField] private GameObject _wall;

    private RoadMarkerTriggerZone _roadMarkerTriggerZone;
    // Start is called before the first frame update
    void Awake()
    {
        _roadMarkerTriggerZone = GetComponentInChildren<RoadMarkerTriggerZone>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenRoad()
    {
        if(!_isOpen)
        {
            _isOpen = true;
            _wall.SetActive(false);
            _roadMarkerTriggerZone.Open();
        }
    }

    public void CloseRoad()
    {
        if (_isOpen)
        {
            _isOpen = false;
            _wall.SetActive(true);
            _roadMarkerTriggerZone.Close();
        }
        
    }

}
