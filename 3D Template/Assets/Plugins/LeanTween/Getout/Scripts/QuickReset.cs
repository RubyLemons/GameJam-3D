using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuickReset : MonoBehaviour
{
    [Range(1, 60)] [SerializeField] int targetFps = 60;

    bool ctrl;

    bool ignoreCursorState;

    void Update()
    {
        //target fps
        Application.targetFrameRate = targetFps;

        //cursor state

        if (Input.GetMouseButtonDown(2) && ctrl)
            ignoreCursorState = !ignoreCursorState;

        Cursor.lockState = (ignoreCursorState) ? CursorLockMode.None : Tks.cursorState;

        //quick reset
        ctrl = Input.GetKey(KeyCode.LeftControl);

        if (ctrl && Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
