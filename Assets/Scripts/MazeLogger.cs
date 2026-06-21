using System.IO;
using UnityEngine;

/// <summary>
/// Bộ ghi log trung tâm cho mê cung nghiên cứu.
///
/// NGUYÊN TẮC CỐT LÕI: chỉ ghi RAW EVENT + timestamp. KHÔNG tự cộng trừ
/// timer trong game. Mọi metric (decision time, movement time, total time)
/// đều tính OFFLINE từ file CSV sau khi chơi xong. Nhờ vậy nếu giáo sư đổi
/// định nghĩa một metric nào đó, chỉ cần chạy lại script phân tích, không
/// phải chơi lại.
///
/// Đây là singleton, sống xuyên suốt session (DontDestroyOnLoad). Mọi script
/// khác (Gate, PlayerSampler) chỉ gọi vào hàm Log(...) duy nhất ở đây.
/// </summary>
public class MazeLogger : MonoBehaviour
{
    public static MazeLogger I { get; private set; }

    [Tooltip("Mã người chơi / phiên, ghi vào tên file. Để trống sẽ tự sinh theo thời gian.")]
    public string playerId = "";

    // run_index: lần chơi thứ mấy trong cùng một session.
    // Tăng tại thời điểm RESPAWN (theo yêu cầu: respawn mới là lúc thật sự
    // bắt đầu một lần chơi mới). Run đầu tiên là 0.
    private int runIndex = 0;

    private StreamWriter writer;
    private float startTime;

    void Awake()
    {
        // Đảm bảo chỉ tồn tại một instance.
        if (I != null && I != this)
        {
            Destroy(gameObject);
            return;
        }
        I = this;
        DontDestroyOnLoad(gameObject);

        string id = string.IsNullOrEmpty(playerId) ? "anon" : playerId;
        string fileName = $"maze_{id}_{System.DateTime.Now:yyyyMMdd_HHmmss}.csv";
        string path = Path.Combine(Application.persistentDataPath, fileName);

        writer = new StreamWriter(path, false);
        // Header. from_node/to_node/edge_id để trống với những event không liên quan.
        writer.WriteLine("run_index,time_s,event_type,node_id,edge_id,pos_x,pos_y,pos_z");
        writer.AutoFlush = true; // ghi xuống đĩa ngay mỗi dòng -> crash vẫn còn data

        startTime = Time.unscaledTime;

        Debug.Log("[MazeLogger] Đang ghi log tại: " + path);
    }

    /// <summary>
    /// Ghi một event. time_s là thời gian tương đối từ lúc khởi động (giây),
    /// dùng Time.unscaledTime để không bị ảnh hưởng bởi pause / Time.timeScale.
    /// </summary>
    public void Log(string eventType, Vector3 pos, string nodeId = "", string edgeId = "")
    {
        if (writer == null) return;
        float t = Time.unscaledTime - startTime;
        writer.WriteLine(
            $"{runIndex},{t:F3},{eventType},{nodeId},{edgeId}," +
            $"{pos.x:F3},{pos.y:F3},{pos.z:F3}");
    }

    /// <summary>
    /// Gọi khi player RESPAWN về điểm xuất phát để bắt đầu lần chơi mới.
    /// Tăng run_index rồi ghi mốc 'respawn'. Khoảng nghỉ "tĩnh tâm" trước đó
    /// (giữa reach_goal và respawn) nằm ngoài mọi run nên không nhiễu metric.
    /// </summary>
    public void StartNewRun(Vector3 spawnPos)
    {
        runIndex++;
        Log("respawn", spawnPos);
    }

    /// <summary>Cho script khác đọc run hiện tại nếu cần (vd hiển thị debug).</summary>
    public int CurrentRun => runIndex;

    void OnApplicationQuit()
    {
        writer?.Flush();
        writer?.Close();
        writer = null;
    }

    void OnDestroy()
    {
        if (I == this)
        {
            writer?.Flush();
            writer?.Close();
            writer = null;
        }
    }
}