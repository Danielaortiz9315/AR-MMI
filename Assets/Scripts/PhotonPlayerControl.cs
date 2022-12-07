using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class controls the local player's character and synchronizes its health to all clients.
/// </summary>
public class PhotonPlayerControl : MonoBehaviourPunCallbacks, IPunObservable
{
    /// <summary>
    /// charControl is the refernce to Unity"s CharcterController script, that we put on the player's character to enable easy control of it.
    /// </summary>
    public CharacterController charControl;

    /// <summary>
    /// healthText is the reference to the UI Text element where we show the player's health.
    /// </summary>
    public Text healthText;

    /// <summary>
    /// speed is the variable we can manipulate to make the character faster.
    /// </summary>
    public float speed = 5f;

    /// <summary>
    /// health describes the character's health. This value can be manipulated by the local player, and its value is send and synchonrized on all the other clients.
    /// </summary>
    public float health = 100;

    // Update is called once per frame
    void Update()
    {
        // This sets the text of the UI text element to be the health value. The 'ToString()' converts the number (float) into text (string).
        healthText.text = health.ToString();

        // This is only true, when we are the local player controlling this character. If this is called on any other client, this will be false and they can not control our character.
        if (photonView.IsMine) {

            // This uses the Input Axis predefined by Unity to create a 3D-Vector of where the character should move. Hotizontal is the Axis controlled by Left/Right-Arrow or by the A/D-keys. Vertical is controlled by Up/Down-Arrow or W/S-keys.
            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            charControl.Move(move * Time.deltaTime * speed);

        }
    }

    // This is called every time Photon updates.
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // If we are the owner of this character, we are allowed to write something to the stream that gets send to all other clients.
        if (stream.IsWriting)
        {
            // Here we send the current health to the stream.
            stream.SendNext(health);
        }
        // If we are not the owner, we just want to receive the new value and show it accordingly.
        else {
            // Here we set the local health variable to the one we got back form the stream. What we get from the stream is simple bits/bytes, so we first have to cast it to float with the '(float)' function.
            health = (float)stream.ReceiveNext();
        }

    }
}
