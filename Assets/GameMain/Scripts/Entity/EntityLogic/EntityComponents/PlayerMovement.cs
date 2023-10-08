using UnityEngine;

namespace FPS
{
    public class PlayerMovement : MonoBehaviour
    {
        public Player player;
        private CharacterController controller;
        private Vector3 playerVelocity;
        private bool groundedPlayer;
        private float playerSpeed = 2.0f;
        private float jumpHeight = 1.0f;
        private float gravityValue = -9.81f;
        
        [Header("键位设置")]
        [SerializeField] [Tooltip("跳跃按键")] private string jumpInputName = "Jump";
        [SerializeField] [Tooltip("奔跑按键")] private KeyCode runInputName;
        [SerializeField] [Tooltip("下蹲按键")] private KeyCode crouchInputName;


        public void OnStart()
        {
            controller = GetComponent<CharacterController>();
        }

        public void OnUpdate()
        {
            
            groundedPlayer = controller.isGrounded;
            if (groundedPlayer && playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;
            }

            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            controller.Move(move * Time.deltaTime * playerSpeed);

            if (move != Vector3.zero)
            {
                player.transform.position += move;
            }

            // if (player.playerCamera.yRotation!=0)
            // {
            //     player.transform.Rotate(Vector3.up * player.playerCamera.yRotation);
            // }
            // Changes the height position of the player..
            if (Input.GetButtonDown(jumpInputName) && groundedPlayer)
            {
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            }

            playerVelocity.y += gravityValue * Time.deltaTime;
            controller.Move(playerVelocity * Time.deltaTime);
        }
        
    }
}