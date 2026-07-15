using System;
using UnityEngine;

public class PathNode : MonoBehaviour
{
    // The ID assigned to this node in the Inspector.
    public string nodeID;

    // Runs when another collider enters this node's trigger.
    private void OnTriggerEnter(Collider other)
    {
        // Ignore anything that is not the player.
        if (!other.CompareTag("Player"))
        {
            return;
        }

        // Check whether this is a Start node.
        bool isStartNode = nodeID.StartsWith(
            "Start",
            StringComparison.OrdinalIgnoreCase
        );

        // Check whether this is a Red exit node.
        bool isRedNode = nodeID.StartsWith(
            "Red",
            StringComparison.OrdinalIgnoreCase
        );

        // Start nodes are only used by the score system.
        // They are not sent to PathManager, so there will be no
        // "Start_1 -> P10_A" missing-path warning.
        if (isStartNode)
        {
            PlayerScoreManager.Instance.StartScore();
            return;
        }

        // Normal, Blue, and Red nodes are sent to PathManager.
        PathManager.Instance.EnterNode(nodeID);

        // Red nodes also finish the score.
        if (isRedNode)
        {
            PlayerScoreManager.Instance.FinishScore();
        }
    }
}