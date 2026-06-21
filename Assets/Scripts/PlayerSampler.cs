using UnityEngine;
 
/// <summary>
/// Gắn trên Player. Mỗi 'interval' giây ghi một dòng 'sample' kèm vị trí.
/// Chưa dùng ngay, nhưng log sẵn từ đầu để sau này phân tích được:
///   - tốc độ (chênh lệch vị trí giữa hai sample / chênh lệch thời gian)
///   - số lần dừng và thời lượng dừng (tốc độ ~ 0)
///   - quãng đường thực tế so với đường tối ưu
///   - mức độ lưỡng lự / đổi ý trong phòng (đường đi loanh quanh)
///
/// Ba metric chính (decision/movement/total) KHÔNG phụ thuộc sample này -
/// chúng tính từ các event của Gate. Sample chỉ là dữ liệu bổ sung.
/// </summary>
public class PlayerSampler : MonoBehaviour
{
    [Tooltip("Khoảng giữa hai lần ghi vị trí, giây. 0.1 = 100ms.")]
    public float interval = 0.1f;
 
    private float timer;
 
    void Update()
    {
        // unscaledDeltaTime để khớp với unscaledTime dùng trong MazeLogger.
        timer += Time.unscaledDeltaTime;
        if (timer >= interval)
        {
            timer -= interval;
            if (MazeLogger.I != null)
                MazeLogger.I.Log("sample", transform.position);
        }
    }
}