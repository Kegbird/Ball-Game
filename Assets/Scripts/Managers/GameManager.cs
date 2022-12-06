using Interfaces;
using Managers;
using System.Collections;
using UnityEngine;
using Utility;

namespace Player
{
    public class GameManager : MonoBehaviour, IGameManagerInterface
    {
        private static IGameManagerInterface instance;
        public Transform m_player_transform;
        public Transform m_player_spawn_transform;
        private IMapGeneratorInterface m_map_generator_interface;
        private IUIManagerInterface m_ui_manager_interface;
        private IPlayerAudioInterface m_player_audio_interface;
        [SerializeField]
        private bool m_win;
        [SerializeField]
        private bool m_gameover;
        [SerializeField]
        private float m_timer;

        private void Awake()
        {
            instance = this;
            m_win = false;
            m_player_transform = GameObject.Find(GameObjectNames.PLAYER_GAMEOBJECT_NAME).gameObject.transform;
            m_player_spawn_transform = GameObject.Find(GameObjectNames.PLAYER_SPAWN_GAMEOBJECT_NAME).gameObject.transform;
        }

        private void Start()
        {
            m_player_audio_interface = PlayerAudio.GetInstance();
            m_map_generator_interface = MapGenerator.GetInstance();
            m_ui_manager_interface = UIManager.GetInstance();
            m_map_generator_interface.GenerateMap();
            m_player_transform.position = m_player_spawn_transform.position;

            IEnumerator Intro()
            {
                m_ui_manager_interface.HideBlackScreen();
                yield return new WaitForSeconds(5f);
                m_ui_manager_interface.HideObjectiveText();
            }
            StartCoroutine(Intro());
            StartCoroutine(Timer());
        }

        private IEnumerator Timer()
        {
            m_ui_manager_interface.ShowTimerText(m_timer);
            float time = m_timer;
            while(time>0)
            {
                yield return new WaitForSeconds(1f);
                time--;
                m_ui_manager_interface.UpdateTimerText(time);
            }

            if (!m_win)
            {
                m_ui_manager_interface.ShowEndText("You lose!");
                m_gameover = true;
                StartCoroutine(EndGame());
            }
        }

        public static IGameManagerInterface GetInstance()
        {
            return instance;
        }

        public void Win()
        {
            if (m_win)
                return;
            else if (m_gameover)
                return;

            m_win = true;

            StopAllCoroutines();
            m_ui_manager_interface.ShowEndText("You win!");
            m_player_audio_interface.PlaySound(SoundEnum.VICTORY);
            StartCoroutine(EndGame());
        }

        IEnumerator EndGame()
        {
            yield return new WaitForSeconds(5f);
            m_ui_manager_interface.ShowBlackScreen();
            yield return new WaitForSeconds(2f);
            Application.Quit();
        }
    }
}
