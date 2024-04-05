using UnityEngine;

public class PlayerFirstPersonMover : MonoBehaviour
{
    [SerializeField] private float _sensivity;
    [SerializeField] private Player _player;
    [SerializeField] private Camera _camera;
    [SerializeField] private Vector2 _cameraXRotationBounds;

    private float _cameraXRotation;

    private const string horizontal = "Horizontal";
    private const string vertical = "Vertical";
    private const string mouseX = "Mouse X";
    private const string mouseY = "Mouse Y";

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.LeftControl))
            _player.TrySit();

        if (Input.GetKey(KeyCode.LeftShift))
            _player.TryRunning();

        if (Input.GetKeyUp(KeyCode.LeftControl))
            _player.TryWalk();

        if(Input.GetKeyUp(KeyCode.LeftShift))
            _player.TryWalk();

        if (Input.GetKeyDown(KeyCode.Space))
            _player.TryJump();

        if (Input.GetAxis(horizontal) == 0 && Input.GetAxis(vertical) == 0)
            _player.Stand();
        else
            _player.Move(transform.right * Input.GetAxis(horizontal) + transform.forward * Input.GetAxis(vertical));

        _player.Rotate(Vector3.up, Input.GetAxis(mouseX) * _sensivity);
        _cameraXRotation = Mathf.Clamp(_cameraXRotation + Input.GetAxis(mouseY), _cameraXRotationBounds.x, _cameraXRotationBounds.y);
        _camera.transform.localRotation = Quaternion.Euler(-_cameraXRotation * _sensivity, _camera.transform.localRotation.eulerAngles.y, _camera.transform.localRotation.eulerAngles.z);
    }
}