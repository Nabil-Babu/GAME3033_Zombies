using Character.UI;
using UnityEngine;

namespace Character
{
    public class PlayerController : MonoBehaviour
    {
        public CrosshairScript CrossHair => CrossHairComponent;
        [SerializeField] private CrosshairScript CrossHairComponent;
        
        public bool isFiring;
        public bool isReloading;
        public bool isJumping;
        public bool isRunning;
    }
}
