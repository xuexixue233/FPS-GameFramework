using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Camera mainCamera;
    public Camera overlayCamera;
    
    public float mouseSensitivity=100f;//鼠标灵敏度
    
    private float xRotation=0f;
    public float yRotation;

    public void OnStart()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        
        xRotation -= mouseY;//将上下旋转的轴值进行累计
        xRotation = Mathf.Clamp(xRotation,-90f,90f);//限制轴值的累计（这里就能发现上90度和下90度角正好相对于了90的轴值）
        transform.localRotation = Quaternion.Euler(xRotation, 0f,0f);
        yRotation = mouseX;
    }
}
