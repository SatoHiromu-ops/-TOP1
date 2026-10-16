using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class EnemySearchArea : MonoBehaviour
{
    [SerializeField] private float range = 5f;
    [SerializeField] private float angle = 90f;
    [SerializeField] private int segments = 30;

    [SerializeField]
    private Color areaColor =
        new Color(1f, 1f, 0f, 0.25f);

    private Mesh mesh;
    private MeshRenderer meshRenderer;

    void Awake()
    {
        mesh = new Mesh();
        mesh.name = "Enemy Search Area";

        GetComponent<MeshFilter>().mesh = mesh;
        meshRenderer = GetComponent<MeshRenderer>();

        // 半透明マテリアルを作成
        Shader shader = Shader.Find("Sprites/Default");

        if (shader != null)
        {
            Material mat = new Material(shader);
            mat.color = areaColor;
            meshRenderer.material = mat;
        }

        // 床に表示するため、影を無効化
        meshRenderer.shadowCastingMode =
            UnityEngine.Rendering.ShadowCastingMode.Off;

        meshRenderer.receiveShadows = false;

        CreateMesh();
    }

    void CreateMesh()
    {
        segments = Mathf.Max(segments, 3);

        Vector3[] vertices = new Vector3[segments + 2];
        int[] triangles = new int[segments * 3];

        // 扇形の中心
        vertices[0] = Vector3.zero;

        float halfAngle = angle / 2f;

        for (int i = 0; i <= segments; i++)
        {
            float currentAngle =
                -halfAngle + angle * i / segments;

            float rad = currentAngle * Mathf.Deg2Rad;

            vertices[i + 1] = new Vector3(
                Mathf.Sin(rad) * range,
                0f,
                Mathf.Cos(rad) * range
            );
        }

        for (int i = 0; i < segments; i++)
        {
            int index = i * 3;

            triangles[index] = 0;
            triangles[index + 1] = i + 1;
            triangles[index + 2] = i + 2;
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    // Enemy.csから索敵設定を反映する
    public void SetSearchArea(float newRange, float newAngle)
    {
        range = newRange;
        angle = newAngle;

        if (mesh != null)
        {
            CreateMesh();
        }
    }
}
