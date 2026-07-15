using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawnManager : MonoBehaviour
{
    public static PlayerRespawnManager Instance;

    // The player that will be teleported.
    public Transform player;

    // The location inside the start room.
    public Transform startRoomPoint;



    private void Awake()
    {
        Instance = this;
    }

    // Teleports the player back to the start room.
    public void TeleportPlayerToStartRoom()
    {
        if (startRoomPoint == null)
        {
            Debug.LogError(
                "Start Room Point is not assigned in PlayerRespawnManager."
            );

            return;
        }

        TeleportPlayer(startRoomPoint);

        Debug.Log(
            "Player teleported to start room: " +
            startRoomPoint.name
        );
    }


    // Handles the actual player movement.
    private void TeleportPlayer(Transform destination)
    {
        if (player == null)
        {
            Debug.LogError(
                "Player is not assigned in PlayerRespawnManager."
            );

            return;
        }

        CharacterController controller =
            player.GetComponent<CharacterController>();

        // CharacterController must be disabled before changing position.
        if (controller != null)
        {
            controller.enabled = false;

            player.position = destination.position;
            player.rotation = destination.rotation;

            controller.enabled = true;
        }
        else
        {
            player.position = destination.position;
            player.rotation = destination.rotation;
        }
    }
}