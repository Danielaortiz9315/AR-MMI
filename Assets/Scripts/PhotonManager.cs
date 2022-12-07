using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles connecting to the Photon cloud.
/// </summary>
public class PhotonManager : MonoBehaviourPunCallbacks
{
    /// <summary>
    /// This is a reference to the playerPrefab, that we have saved in our Resources folder. This will be spawned into the scene for each new client connecting.
    /// </summary>
    public GameObject playerPrefab;

    // Called when we start the application/scene.
    void Start()
    {
        // Trying to connect to the cloud
        PhotonNetwork.ConnectUsingSettings();
    }

    // If the connection to the cloud was successful:
    public override void OnConnectedToMaster()
    {
        // Try to join a random room. If there is none, this will fail.
        PhotonNetwork.JoinRandomRoom();
    }

    // If joining a random room failed:
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // Log something to the console, so we know whats going on when debugging.
        Debug.Log("Joining a room failed");

        // This creates a new room without a name and joins it immediately.
        PhotonNetwork.CreateRoom(null);
    }

    // Once we joined a room:
    public override void OnJoinedRoom()
    {
        // This spawns the playerPrefab (our character) into the scene at the origin of the scene (0,0,0). 
        PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
    }

}
