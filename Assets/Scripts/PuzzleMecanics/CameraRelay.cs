using Cinemachine;
using UnityEngine;

public class CameraRelay : MonoBehaviour
{
    [SerializeField] private new CinemachineVirtualCamera camera;
    [SerializeField] private Transform cameraPos;
    [SerializeField] private float defaultLensSize = 7;
    [SerializeField] private float defaultDamping = 0.3f;
    [SerializeField] private float deadZoneX = 0.1f;
    [SerializeField] private float deadZoneY = 0.3f;
    [SerializeField] private float dampingSpeed = 3.0f;
    [SerializeField] private float lerpSpeed = 0.05f;
    
    private Transform playerPos;
    
    private Transform follow;
    private float targetLensSize;

	public float GetLensSize() { return defaultLensSize; }

	public void SetFollow(Transform trans)
	{
		if (camera.Follow == trans) return;
		follow = trans;
	}
    
	public void SetLensSize(float lensSize) { targetLensSize = lensSize; }

	private void Start()
    {
        playerPos = FindObjectOfType<PlayerController>().transform;
    }

    private void Update()
    {
        if (!follow) return;

		camera.Follow = follow;

		var body              = camera.GetCinemachineComponent<CinemachineFramingTransposer>();
		body.m_XDamping       = dampingSpeed;
		body.m_YDamping       = dampingSpeed;
		body.m_ZDamping       = dampingSpeed;
		body.m_DeadZoneWidth  = 0.0f;
		body.m_DeadZoneHeight = 0.0f;

		camera.m_Lens.OrthographicSize =
			Mathf.Lerp(camera.m_Lens.OrthographicSize, targetLensSize, lerpSpeed);

		if (Mathf.Abs(follow.position.x - cameraPos.position.x) < 1.0f)
		{
			camera.m_Lens.OrthographicSize = targetLensSize;
			camera.Follow                  = follow;
			body.m_XDamping                = defaultDamping;
			body.m_YDamping                = defaultDamping;
			body.m_ZDamping                = defaultDamping;
			body.m_DeadZoneWidth           = deadZoneX;
			body.m_DeadZoneHeight          = deadZoneY;
			follow                         = null;
		}
	}
}
