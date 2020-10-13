using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerLight : MonoBehaviour
{
    private Light _light;

    [SerializeField] private float _minIntensity;

    [SerializeField] private float _maxIntensity;

    [SerializeField] private float _minAngle;

    [SerializeField] private float _maxAngle;

    [SerializeField] private float _minWait;

    [SerializeField] private float _maxWait;

    [SerializeField] private Light _secondaryLight;

    public bool IgnoreAngle;

    public bool ChangeSecondaryLight;



    // Start is called before the first frame update
    void Awake()
    {
        _light = GetComponent<Light>();
        ChangeIntensity();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChangeIntensity()
    {
        float percent = Random.Range(.1f, 1);
        _light.intensity = Mathf.Lerp(_minIntensity, _maxIntensity, percent);

        if(!IgnoreAngle)
            _light.spotAngle = Mathf.Lerp(_minAngle, _maxAngle, percent);

        if (ChangeSecondaryLight)
            _secondaryLight.intensity = _light.intensity / 10;

        Invoke("ChangeIntensity", Random.Range(_minWait, _maxWait));
    }
}
