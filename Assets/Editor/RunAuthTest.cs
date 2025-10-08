using UnityEditor;
using UnityEngine;
using System.Threading.Tasks;
using Google.Protobuf;
using System.Text;

public class RunAuthTest
{
    [MenuItem("Tools/Run Auth Test")]
    public static async Task RunTest()
    {
        Debug.Log("Running Auth Test from Editor Script...");
        try
        {
            var credentials = ByteString.CopyFrom(Encoding.UTF8.GetBytes("my-l2-credentials"));

            // Authenticate
            string authToken = await AuthService.AuthenticateAndGetTokenAsync("facu", "1234");
            Debug.Log($"Authenticated successfully. Token: {authToken}");

            // User Exists / Create User
            Debug.Log("Checking if user exists...");
            bool userExists = await User.UserService.UserExistsAsync(credentials.ToByteArray());
            string userId;

            if (userExists)
            {
                // To get the user ID, we need to call GetUser after authentication
                // For simplicity, let's assume we can get the user ID from the token or a separate call
                // For now, we'll just log that the user exists.
                Debug.Log("User exists. Skipping creation.");
                // In a real scenario, you'd have a way to retrieve the userId if it exists
                // For this test, we'll create a new user if it doesn't exist, or assume a default if it does.
                // This part needs refinement based on actual backend logic for existing users.
                userId = "some_existing_user_id"; // Placeholder
            }
            else
            {
                Debug.Log("User does not exist, creating new user...");
                var newUser = await User.UserService.CreateUserAsync(credentials.ToByteArray());
                userId = newUser.UserId;
                Debug.Log($"User created with ID: {userId}");
            }

            // Get User Data
            Debug.Log("Retrieving user data...");
            var userData = await User.UserService.GetUserAsync(userId, authToken);
            Debug.Log("User Data:");
            Debug.Log($"  ID: {userData.UserId}");
            Debug.Log($"  Creation Time: {userData.CreationTime.ToDateTime()}");
            Debug.Log($"  Last Auth Time: {userData.LastAuthTime.ToDateTime()}");
            Debug.Log($"  Money: {userData.Money}");
            Debug.Log($"  Inventory Items: {userData.Inventory.Count}");

            // Echo Service
            string echoMessage = "Hello from Unity!";
            string echoResponse = await Echo.EchoService.SayHelloAsync(echoMessage, authToken);
            Debug.Log($"Echo Service Response: {echoResponse}");

            Debug.Log("Auth Test Succeeded.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Auth Test Failed: {ex.Message}");
        }
        Debug.Log("Auth Test Finished.");
    }

    public static void RunTestBatchMode()
    {
        Task.Run(async () => await RunTest()).Wait();
        EditorApplication.Exit(0);
    }
}