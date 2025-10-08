# How to run the testbackend functionality

This document explains how to run the `testbackend` scene in Unity to test the gRPC connection.

## Prerequisites

*   You must have the Unity Editor installed.
*   You must know the path to the Unity Editor executable.

## Running from the command line

1.  Open a terminal or command prompt.
2.  Navigate to the root directory of this project.
3.  Execute the following command, replacing `/path/to/Unity` with the actual path to your Unity Editor executable:

    ```bash
/home/david/Unity/Hub/Editor/6000.2.6f1/Editor/Unity -batchmode -projectPath . -openScene /home/david/dev/rasta/RastaEmpire2D/Assets/Scene/testBacken.unity -logfile unity.log
    ```

4.  The command will open the `testBacken` scene, and the `testBackend.cs` script will attempt to connect to the gRPC service.
5.  You can view the output of the process in the `unity.log` file.

## Expected output

If the connection is successful, the `unity.log` file should contain a line with the authentication token, similar to this:

```
Token: [your_auth_token]
```

If there is an error, the `unity.log` file will contain an error message.
