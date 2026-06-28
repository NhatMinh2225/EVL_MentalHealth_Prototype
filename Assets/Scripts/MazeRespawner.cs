using System.Collections;
using UnityEngine;

/// <summary>
/// Đưa player về điểm spawn sau khi chạm đích, có khoảng nghỉ "tĩnh tâm".
///
/// Luồng đầy đủ một vòng:
///   Gate(Goal) ghi 'reach_goal' -> gọi BeginRespawn(player)
///   -> đợi 'restSeconds' giây (khoảng nghỉ, nằm NGOÀI mọi run)
///   -> tắt CharacterController, dời player về 'spawnPosition', bật lại
///   -> RespawnHelper.NotifyRespawn(...) -> run_index++ và ghi 'respawn'
///   -> vòng chơi mới bắt đầu khi player tự đi qua một trong ba cổng.
///
/// LƯU Ý CharacterController: bắt buộc TẮT nó trước khi gán transform.position,
/// nếu không CharacterController sẽ ghi đè vị trí và player bị kéo về chỗ cũ.
/// Script này tự xử lý việc đó.
/// </summary>
public class MazeRespawner : MonoBehaviour
{
    [Header("Điểm spawn (nơi nhìn thấy cả 3 cổng)")]
    [Tooltip("Tọa độ player sẽ xuất hiện lại cho lần chơi mới.")]
    public Vector3 spawnPosition = new Vector3(-228.25f, 0f, -6.25f);

    [Tooltip("Hướng nhìn lúc spawn (Euler góc Y, độ). Để 0 nếu không cần xoay.")]
    public float spawnYaw = 90f;

    [Header("Thời gian nghỉ")]
    [Tooltip("Số giây 'tĩnh tâm' giữa lúc chạm đích và lúc respawn. Khoảng này nằm ngoài mọi run nên không tính vào metric.")]
    public float restSeconds = 5f;

    private bool isRespawning = false;

    /// <summary>Gate(Goal) gọi hàm này, truyền vào transform của player.</summary>
    public void BeginRespawn(Transform player)
    {
        if (isRespawning) return; // tránh kích hoạt trùng nếu chạm goal nhiều lần
        StartCoroutine(RespawnRoutine(player));
    }

    private IEnumerator RespawnRoutine(Transform player)
    {
        isRespawning = true;

        // Khoảng nghỉ. Dùng realtime để không phụ thuộc Time.timeScale.
        yield return new WaitForSecondsRealtime(restSeconds);

        // Tắt CharacterController (nếu có) trước khi dời, rồi bật lại.
        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        player.position = spawnPosition;
        player.rotation = Quaternion.Euler(0f, spawnYaw, 0f);

        if (cc != null) cc.enabled = true;

        // Tăng run_index + ghi mốc 'respawn'. Từ đây là lần chơi mới.
        RespawnHelper.NotifyRespawn(player.position);

        isRespawning = false;
    }
}