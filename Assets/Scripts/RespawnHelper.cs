using UnityEngine;

/// <summary>
/// Cầu nối nhỏ giữa logic respawn có sẵn của bạn và MazeLogger.
///
/// Bạn ĐÃ có cơ chế: chạm đích -> nghỉ vài giây -> đưa player về điểm spawn
/// chung (nhìn thấy 3 cổng). Script này không tự làm việc respawn; nó chỉ
/// cung cấp một hàm để bạn GỌI vào đúng thời điểm respawn, để run_index tăng
/// và mốc 'respawn' được ghi.
///
/// CÁCH DÙNG: ở đoạn code respawn của bạn, sau khi đã đặt lại vị trí player
/// về điểm spawn, gọi:
///     RespawnHelper.NotifyRespawn(player.transform.position);
///
/// Hoặc nếu tiện hơn, gắn script này vào player và gọi instance.DoRespawn().
/// </summary>
public class RespawnHelper : MonoBehaviour
{
    [Tooltip("Tham chiếu tới transform của player (để lấy vị trí lúc respawn). Nếu để trống sẽ dùng transform của chính object này.")]
    public Transform player;

    /// <summary>Gọi ngay sau khi đã đưa player về điểm spawn.</summary>
    public void DoRespawn()
    {
        Transform t = player != null ? player : transform;
        NotifyRespawn(t.position);
    }

    /// <summary>Hàm static tiện gọi từ bất kỳ đâu trong code respawn của bạn.</summary>
    public static void NotifyRespawn(Vector3 spawnPos)
    {
        if (MazeLogger.I != null)
            MazeLogger.I.StartNewRun(spawnPos);
        else
            Debug.LogWarning("[RespawnHelper] Chưa có MazeLogger trong scene.");
    }
}