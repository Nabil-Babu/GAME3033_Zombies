using Events;
using Manager;
using Parent;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Character.UI
{
    public class CrosshairScript : InputMonoBehaviour
    {
    
        public Vector2 mouseSensitivity;
        
        public bool inverted = false;
        
        public Vector2 CurrentAimPosition { get; private set; }
        
        [SerializeField, Range(0, 1)] 
        private float crosshairHorizontalPercentage = 0.25f;

        private float _horizontalOffset;
        private float _maxHorizontalDeltaConstrain;
        private float _minHorizontalDeltaConstrain;
        
        [SerializeField, Range(0, 1)] 
        private float crosshairVerticalPercentage = 0.25f;
        
        private float _verticalOffset;
        private float _maxVerticalDeltaConstrain;
        private float _minVerticalDeltaConstrain;
        

        private Vector2 _crosshairStartingPosition;
        private Vector2 _currentLookDeltas;
    
        private void Start()
        {
            if (GameManager.Instance.CursorActive)
            {
                AppEvents.Invoke_OnMouseCursorEnable(false);
            }
        
            _crosshairStartingPosition = new Vector2(Screen.width / 2f, Screen.height / 2f);

            _horizontalOffset = (Screen.width * crosshairHorizontalPercentage) / 2f;
            _minHorizontalDeltaConstrain = -(Screen.width / 2f) + _horizontalOffset;
            _maxHorizontalDeltaConstrain = (Screen.width / 2f) - _horizontalOffset;

            _verticalOffset = (Screen.height * crosshairVerticalPercentage) / 2f;
            _minVerticalDeltaConstrain = -(Screen.height / 2f) + _verticalOffset;
            _maxVerticalDeltaConstrain = (Screen.height / 2f) - _verticalOffset;
        }
    
        private void OnLook(InputAction.CallbackContext delta)
        {
            Vector2 mouseDelta = delta.ReadValue<Vector2>();
            _currentLookDeltas.x += mouseDelta.x * mouseSensitivity.x;
            if (_currentLookDeltas.x >= _maxHorizontalDeltaConstrain || _currentLookDeltas.x <= _minHorizontalDeltaConstrain)
            {
                _currentLookDeltas.x -= mouseDelta.x * mouseSensitivity.x;
            }

            _currentLookDeltas.y += mouseDelta.y * mouseSensitivity.y;
            if (_currentLookDeltas.y >= _maxVerticalDeltaConstrain || _currentLookDeltas.y <= _minVerticalDeltaConstrain)
            {
                _currentLookDeltas.y -= mouseDelta.y * mouseSensitivity.y;
            }
        }

        private void Update()
        {
            float crosshairXPosition = _crosshairStartingPosition.x + _currentLookDeltas.x;
            float crosshairYPosition = inverted
                ? _crosshairStartingPosition.y - _currentLookDeltas.y
                : _crosshairStartingPosition.y + _currentLookDeltas.y;

            CurrentAimPosition = new Vector2(crosshairXPosition, crosshairYPosition);

            transform.position = CurrentAimPosition;
        }

        private new void OnEnable()
        {
            base.OnEnable();
            GameInput.ThirdPerson.Look.performed += OnLook;
        }
        
        private new void OnDisable()
        {
            base.OnDisable();
            GameInput.ThirdPerson.Look.performed -= OnLook;
        }
    }
}
