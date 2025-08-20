using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Отрисовка сектора активности.
/// Контроллер располагает этот объект в позиции корабля и поворачивает:
/// sectorHighlight.transform.rotation = ship.transform.rotation * GetSideRotation(side)
/// </summary>
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SectorHighlightMesh : MonoBehaviour
{
    [Header("Base Settings")]
    public float innerRadius = 0.1f;
    public float outerRadius = 6f;
    [Range(6, 180)] public int resolution = 40;
    public float animationSpeed = 6f;

    [Header("Visuals")]
    public Color sectorColor = new Color(1f, 1f, 1f, 0.25f);
    public Material sectorMaterial;

    private Mesh mesh;
    private float currentWidthDeg = 0f;
    private float targetWidthDeg = 0f;

    private Transform shipTransform;
    private ShipCannonMultiSide.CannonSide currentSide;
    private ShipCannonMultiSide controller;

    void Awake()
    {
        mesh = new Mesh { name = "SectorHighlightMesh" };
        GetComponent<MeshFilter>().mesh = mesh;
        SetupMaterial();
    }

    void Update()
    {
        if (!ShouldUpdate())
        {
            if (mesh.vertexCount > 0) mesh.Clear();
            return;
        }

        float desired = GetEffectiveSectorWidthDegrees();
        targetWidthDeg = desired;
        currentWidthDeg = Mathf.Lerp(currentWidthDeg, targetWidthDeg, Time.deltaTime * animationSpeed);

        if (currentWidthDeg <= 0.01f)
        {
            mesh.Clear();
            return;
        }

        UpdateMesh();
    }

    bool ShouldUpdate()
    {
        return controller != null && shipTransform != null && controller.HasCannonsOnSide(currentSide);
    }

    float GetEffectiveSectorWidthDegrees()
    {
        if (currentSide == ShipCannonMultiSide.CannonSide.Front || currentSide == ShipCannonMultiSide.CannonSide.Rear)
            return controller.arcAngle;
        else
            return controller.GetMaxAttackAngle(currentSide);
    }

    /// <summary>
    /// Контроллер вызывает это, чтобы подать данные.
    /// </summary>
    public void SetActive(Transform shipTransform, ShipCannonMultiSide.CannonSide side, ShipCannonMultiSide controller)
    {
        this.shipTransform = shipTransform;
        this.currentSide = side;
        this.controller = controller;
    }

    void UpdateMesh()
    {
        int seg = Mathf.Max(3, resolution);
        float half = currentWidthDeg * 0.5f;

        Vector3[] vertices = new Vector3[(seg + 1) * 2];
        int[] tris = new int[seg * 6];

        for (int i = 0; i <= seg; i++)
        {
            float t = (float)i / seg;
            float angle = -half + t * currentWidthDeg;
            Vector3 dir = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;
            vertices[i] = dir * innerRadius;
            vertices[i + seg + 1] = dir * outerRadius;
        }

        int triIndex = 0;
        for (int i = 0; i < seg; i++)
        {
            int innerCurrent = i;
            int innerNext = i + 1;
            int outerCurrent = i + seg + 1;
            int outerNext = i + seg + 2;

            tris[triIndex++] = innerCurrent;
            tris[triIndex++] = outerNext;
            tris[triIndex++] = outerCurrent;

            tris[triIndex++] = innerCurrent;
            tris[triIndex++] = innerNext;
            tris[triIndex++] = outerNext;
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = tris;
        mesh.RecalculateNormals();
    }

    /// <summary>
    /// Настраивает материал, чтобы сектор всегда рисовался поверх воды и не перекрывался пеной.
    /// </summary>
    void SetupMaterial()
    {
        var renderer = GetComponent<MeshRenderer>();
        renderer.material = sectorMaterial != null
            ? new Material(sectorMaterial) { color = sectorColor }
            : new Material(Shader.Find("Unlit/Color")) { color = sectorColor };

        // Рисовать поверх всего
        renderer.material.renderQueue = 5000;

        // Не записывать в depth buffer
        if (renderer.material.HasProperty("_ZWrite"))
            renderer.material.SetInt("_ZWrite", 0);

        // Без теней
        renderer.shadowCastingMode = ShadowCastingMode.Off;
        renderer.receiveShadows = false;
    }
}
