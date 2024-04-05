using UnityEngine;

public class CameraEffector : MonoBehaviour
{
    [SerializeField] private float _fovChangingSpeed;
    [SerializeField] private float _walkFovIncreasing;
    [SerializeField] private float _runFovIncreasing;
    [SerializeField] private float _tackleFovIncreasing;
    [SerializeField] private float _tiltAngle;
    [SerializeField] private float _tiltSpeed;
    [SerializeField] private Camera _camera;
    [SerializeField] private Vector2 _cameraXRotationBounds;

    private float _deffaultFov;
    private float _targetFov;
    private float _cameraXRotation;
    private float _cameraZRotation;

    private const string horizontal = "Horizontal";
    private const string mouseY = "Mouse Y";

    public void StandEffect()
    {
        _targetFov = _deffaultFov;
    }

    public void WalkEffect()
    {
        _targetFov = _deffaultFov + _walkFovIncreasing;
    }

    public void RunEffect()
    {
        _targetFov = _deffaultFov + _runFovIncreasing;
    }

    public void TackleEffect()
    {
        _targetFov = _deffaultFov + _tackleFovIncreasing;
    }

    private void Start()
    {
        _deffaultFov = _camera.fieldOfView;
        _targetFov = _deffaultFov;
    }

    private void Update()
    {
        _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, _targetFov, _fovChangingSpeed * Time.deltaTime);
        _cameraZRotation = Mathf.Lerp(_cameraZRotation, _tiltAngle * Input.GetAxis(horizontal), _tiltSpeed * Time.deltaTime);
        _cameraXRotation = Mathf.Clamp(_cameraXRotation + Input.GetAxis(mouseY), _cameraXRotationBounds.x, _cameraXRotationBounds.y);
        _camera.transform.localRotation = Quaternion.Euler(-_cameraXRotation, 0, _cameraZRotation);
    }
}
