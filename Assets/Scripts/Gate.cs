using UnityEngine;

/// <summary>
/// Một GATE đặt tại MỖI ngưỡng cửa trong mê cung. Đây là vật thu thập dữ liệu
/// duy nhất ngoài PlayerSampler. Không dùng box-phòng nữa: mọi mốc thời gian
/// đều do các gate ở ngưỡng cửa đánh dấu, nên mốc luôn sắc nét và luôn biết
/// player đi qua đúng cửa nào.
///
/// Mê cung LUÔN có hành lang giữa hai phòng, nên mỗi phòng có:
///   - các gate "ENTRY" ở cửa VÀO  (mở decision time của node)
///   - các gate "EXIT"  ở cửa RA   (đóng decision time + ghi nhánh đã chọn)
/// Cửa ra phòng A và cửa vào phòng B là HAI gate khác nhau, cách nhau bằng
/// hành lang -> khoảng giữa chúng chính là movement time trên edge.
///
/// Suy ra metric (offline, trong cùng một run_index):
///   decision time tại node N = time(EXIT của N) - time(ENTRY của N)
///   movement time trên edge  = time(ENTRY phòng sau) - time(EXIT phòng trước)
///   total time của run        = time(reach_goal) - time(enter_maze)
/// </summary>
[RequireComponent(typeof(Collider))]
public class Gate : MonoBehaviour
{
    public enum GateKind
    {
        Entry,      // cửa VÀO một phòng thường -> ghi enter_node
        Exit,       // cửa RA một phòng thường   -> ghi exit_node + edge
        MazeEntrance, // một trong ba cổng vào mê cung (S0/S1/S2) -> bắt đầu total time
        Goal        // đích -> ghi reach_goal
    }

    [Header("Loại gate")]
    public GateKind kind = GateKind.Entry;

    [Header("Định danh (theo quy ước tool: S0, L1_0, L2_1, ...)")]
    [Tooltip("ID của node mà gate này thuộc về. Vd 'L1_0'. Với MazeEntrance là 'S0'/'S1'/'S2'. Để trống với Goal nếu không cần.")]
    public string nodeId = "";

    [Tooltip("Chỉ dùng cho Exit / MazeEntrance: nhánh mà cửa này dẫn tới. Vd 'L1_0->L2_1'. Quy ước: from->to.")]
    public string edgeId = "";

    [Header("Chỉ dùng cho gate Goal")]
    [Tooltip("Tham chiếu tới MazeRespawner trong scene. Khi player chạm đích, gate này báo cho respawner bắt đầu đếm giờ nghỉ rồi đưa player về spawn. Để trống nếu gate này không phải Goal.")]
    public MazeRespawner respawner;

    [Header("Chống bắn trùng")]
    [Tooltip("Mỗi gate chỉ ghi một lần cho tới khi reset (khi sang run mới). Vì feed-forward không quay lại nên không cần qua gate hai lần trong một run.")]
    public bool fireOncePerRun = true;

    private int lastFiredRun = -1;

    void Reset()
    {
        // Tự bật Is Trigger khi gắn script, đỡ quên trong Inspector.
        GetComponent<Collider>().isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (MazeLogger.I == null) return;

        if (fireOncePerRun && lastFiredRun == MazeLogger.I.CurrentRun)
            return; // gate này đã bắn trong run hiện tại rồi
        lastFiredRun = MazeLogger.I.CurrentRun;

        Vector3 p = other.transform.position;

        switch (kind)
        {
            case GateKind.MazeEntrance:
                // Player vừa chọn một trong ba cổng -> bắt đầu "thời gian trong mê cung".
                // edgeId kiểu "START->S0" nếu muốn, hoặc để trống.
                MazeLogger.I.Log("enter_maze", p, nodeId, edgeId);
                break;

            case GateKind.Entry:
                // Vào một phòng -> bắt đầu đếm decision time tại node này,
                // đồng thời đóng movement time của edge vừa đi xong.
                MazeLogger.I.Log("enter_node", p, nodeId);
                break;

            case GateKind.Exit:
                // Rời một phòng qua một nhánh cụ thể -> kết thúc decision time,
                // ghi nhánh đã chọn, bắt đầu movement time của edge sắp đi.
                MazeLogger.I.Log("exit_node", p, nodeId, edgeId);
                break;

            case GateKind.Goal:
                MazeLogger.I.Log("reach_goal", p, nodeId);
                // Báo cho respawner: bắt đầu đếm giờ nghỉ "tĩnh tâm" rồi đưa
                // player về spawn (việc tăng run_index xảy ra trong respawner).
                if (respawner != null)
                    respawner.BeginRespawn(other.transform);
                else
                    Debug.LogWarning("[Gate] Goal chưa được gán MazeRespawner.");
                break;
        }
    }
}