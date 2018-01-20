using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

enum State
{
    ACTIVE,
    EXITING
}

public class AppStateController : MonoBehaviour {

    #region Attrbitues
    State appState;
    AndroidController androidController;
    #endregion

    #region Behaviour Method
    void Awake()
    {
        ConnectAndroid();
    }

	// Use this for initialization
	void Start () {
        InitializeState();
	}
	
	// Update is called once per frame
	void Update () {
        CheckExitCondition();
    }
    #endregion

    #region Setup
    void InitializeState()
    {
        appState = State.ACTIVE;
    }

    void ConnectAndroid()
    {
        androidController = GetComponent<androidController>();
    }
    #endregion

    #region Action Check
    bool IsForceExit()
    {
        return Input.GetKey(KeyCode.Escape);
    }
    #endregion

    #region State Check
    bool IsExiting()
    {
        return appState == State.EXITING;
    }
    #endregion

    #region Error Check
    bool HasCameraPermission()
    {
        return Session.ConnectionState == SessionConnectionState.UserRejectedNeededPermission;
    }

    bool IsFailToConnect()
    {
        return Session.ConnectionState == SessionConnectionState.ConnectToServiceFailed;
    }
    #endregion

    #region Action
    void Exit()
    {
        Application.Quit();
    }

    void ToastAndExit(string msg)
    {
        androidController.ToastMessage(msg);
        appState = State.EXITING;
        Invoke("Exit", 0.5f);
    }
    #endregion

    #region Check & Execute on Update
    private void CheckExitCondition()
    {
        CheckForceExitCondition();
        CheckErrorExitCondition();
    }

    void CheckForceExitCondition()
    {
        if (IsForceExit()) Exit();
    }

    void CheckErrorExitCondition()
    {
        if (!IsExiting())
            if (HasCameraPermission()) ToastAndExit("Camera permission is needed to run this application.");
            else if (IsFailToConnect()) ToastAndExit("ARCore encountered a problem connecting.  Please start the app again.");
    }
    #endregion

}
