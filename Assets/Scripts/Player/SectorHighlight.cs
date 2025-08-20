using UnityEngine;
using static ShipCannonMultiSide;

public class SectorHighlight : MonoBehaviour
{
    // ���������
    public LineRenderer sectorRenderer;
    public float innerRadius = 2f;
    public float outerRadius = 5f;
    public float arcWidth = 60f;
    public int resolution = 30;
    public float animationSpeed = 3f;

    // ������ �� ��������� �������
    public Transform shipTransform;

    private float currentWidth;
    private float targetWidth;
    private Vector3[] points;
    private CannonSide currentSide;

    void Start()
    {
        sectorRenderer.positionCount = (resolution + 1) * 2 + 2;
        points = new Vector3[(resolution + 1) * 2 + 2];

        sectorRenderer.material.renderQueue = 3000; // ��������� ������ �����
        sectorRenderer.generateLightingData = false;

        // ��������� ��������
        var collider = sectorRenderer.GetComponent<Collider>();
        if (collider != null) Destroy(collider);
    }

    void Update()
    {
        currentWidth = Mathf.Lerp(currentWidth, targetWidth, Time.deltaTime * animationSpeed);
        UpdateSectorPoints();
    }

    public void SetActive(Transform shipTransform, CannonSide side, float width)
    {
        this.shipTransform = shipTransform;
        this.currentSide = side;
        this.targetWidth = width;
    }

    void UpdateSectorPoints()
    {
        if (currentWidth <= 0 || shipTransform == null)
        {
            sectorRenderer.positionCount = 0;
            return;
        }

        float halfWidth = currentWidth * 0.5f;
        float angleStep = currentWidth / resolution;

        // ������� ����������� �������
        Vector3 shipForward = shipTransform.forward;
        Vector3 shipRight = shipTransform.right;

        // ����������� ����� (� ��������� �������)
        points[0] = shipTransform.position;

        // ���������� ����
        for (int i = 0; i <= resolution; i++)
        {
            float angle = Mathf.Deg2Rad * (-halfWidth + angleStep * i);
            Vector3 dir = GetDirectionForSide(currentSide, angle, shipForward, shipRight);
            points[i + 1] = shipTransform.position + dir * innerRadius;
        }

        // ������� ����
        for (int i = 0; i <= resolution; i++)
        {
            float angle = Mathf.Deg2Rad * (-halfWidth + angleStep * (resolution - i));
            Vector3 dir = GetDirectionForSide(currentSide, angle, shipForward, shipRight);
            points[resolution + 2 + i] = shipTransform.position + dir * outerRadius;
        }

        // �������� ������
        points[points.Length - 1] = points[1];

        sectorRenderer.positionCount = points.Length;
        sectorRenderer.SetPositions(points);
    }

    Vector3 GetDirectionForSide(CannonSide side, float angle, Vector3 shipForward, Vector3 shipRight)
    {
        // ���������� ������� ����������� ��� �������
        Vector3 baseDirection = shipForward;
        switch (side)
        {
            case CannonSide.Rear: baseDirection = -shipForward; break;
            case CannonSide.Right: baseDirection = shipRight; break;
            case CannonSide.Left: baseDirection = -shipRight; break;
        }

        // ������������ ����������� �� ������ ����
        return Quaternion.AngleAxis(Mathf.Rad2Deg * angle, Vector3.up) * baseDirection;
    }
}