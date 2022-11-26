using Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Utility;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        private PlayerInputActions m_player_actions;
        private InputAction m_move;
        private InputAction m_jump;
        [SerializeField]
        [Range(1f, 10f)]
        private float m_movement_force_factor;
        [SerializeField]
        [Range(1f, 10f)]
        private float m_jump_impluse_factor;
        private Vector2 m_movement_direction;
        private Rigidbody m_rigidbody;
        [SerializeField]
        private bool m_grounded;
        private IPlayerAudioInterface m_player_audio_interface;

        private void Awake()
        {
            m_player_actions = new PlayerInputActions();
            m_rigidbody = GetComponent<Rigidbody>();
            m_grounded = true;
        }

        private void Start()
        {
            m_player_audio_interface = PlayerAudio.GetInstance();
        }

        private void OnEnable()
        {
            m_move = m_player_actions.Player.Move;
            m_jump = m_player_actions.Player.Jump;
            m_jump.performed += Jump;
            m_move.Enable();
            m_jump.Enable();
        }

        private void OnDisable()
        {
            m_move.Disable();
            m_jump.Disable();
        }

        private void Update()
        {
            m_movement_direction = m_move.ReadValue<Vector2>();
        }

        private void Jump(InputAction.CallbackContext context)
        {
            if (!m_grounded)
            {
                m_player_audio_interface.PlaySound(SoundEnum.INVALID_MOVE);
                return;
            }
            m_player_audio_interface.PlaySound(SoundEnum.JUMP);
            m_rigidbody.AddForce(Vector2.up * m_jump_impluse_factor, ForceMode.Impulse);
            m_grounded = false;
        }

        private void FixedUpdate()
        {
            m_rigidbody.AddForce(m_movement_direction * m_movement_force_factor);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer(LayerMaskNames.GROUND_LAYER_MASK_NAME))
            {
                m_grounded = true;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer(LayerMaskNames.VICTORY_LAYER_MASK_NAME))
            {
                IGameManagerInterface game_manager_interface = GameManager.GetInstance();
                game_manager_interface.Win();
            }
        }
    }
}