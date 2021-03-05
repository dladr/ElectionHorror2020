using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] private Animator _faderAnimator;
    private bool _hasHitSpace;
    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 75;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_hasHitSpace && Input.GetButtonDown("Action"))
        {
            _hasHitSpace = true;
            _faderAnimator.Play("FadeOutTitleScreen");
            Invoke("LoadMainScene", 1);
        }
    }

    void LoadMainScene()
    {
        SceneManager.LoadScene("Main");
    }
}
