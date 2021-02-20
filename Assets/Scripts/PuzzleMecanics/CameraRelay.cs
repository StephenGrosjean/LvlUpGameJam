using Cinemachine;
using UnityEngine;

public class CameraRelay : MonoBehaviour
{
    [SerializeField] private new CinemachineVirtualCamera camera;
    [SerializeField] private Transform cameraPos;
    [SerializeField] private float defaultLensSize;
    
    private Transform playerPos;
    
    private Transform follow;
    private float targetLensSize;

	public float GetLensSize() { return defaultLensSize; }

	public void SetFollow(Transform trans) { follow = trans; }
    
	public void SetLensSize(float lensSize) { targetLensSize = lensSize; }

	private void Start()
    {
        playerPos = FindObjectOfType<PlayerController>().transform;
    }

    private void Update()
    {
        if (!follow) return;
		
        if (follow == playerPos)
        {
            camera.Follow = null;
            
            var position = cameraPos.position;
            position = Vector3.Lerp(position, follow.position, 0.1f);
            position = new Vector3(position.x, position.y, -10.0f);
            cameraPos.position = position;
            
            camera.m_Lens.OrthographicSize =
                Mathf.Lerp(camera.m_Lens.OrthographicSize, defaultLensSize, 0.1f);
        }
        else
        {
            camera.Follow = null;
            
            var position = follow.position;
            cameraPos.position = position;
            camera.m_Lens.OrthographicSize = -position.z;
        }

        if (Mathf.Abs(follow.position.x - cameraPos.position.x) < 0.5f &&
            Mathf.Abs(follow.position.y - cameraPos.position.y) < 0.5f)
        {
            camera.Follow = follow;
            camera.m_Lens.OrthographicSize = targetLensSize;
            follow = null;
        }
    }
}
