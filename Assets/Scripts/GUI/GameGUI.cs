using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameGUI : MonoBehaviour
{
	[SerializeField] private Sprite _pauseIcon;
	[SerializeField] private Sprite _playIcon;
	[SerializeField] private Button _pauseButton;
	[SerializeField] private TMP_Text _timerText;
	[SerializeField] private float _timeSeconds = 120.0f;
	[SerializeField] private GameObject _gameOverPanel;
	[SerializeField] private GameObject _WinPanel;

	private bool _isOnPause;
	private Timer _timer;
	bool stop = false;

	void Start()
	{
		_isOnPause = false;
		_timer = new Timer(_timeSeconds);
		Time.timeScale = 1;
	}

	void Update()
	{
		if (!stop)
		{
			UpdateTimer();
		}
	}

	public void WinScreenActive()
	{
		_WinPanel.SetActive(true);
		stop = true;
	}

	public void PauseButton()
	{
		if (!_isOnPause)
		{
			PauseGame();
			_isOnPause = true;
			_pauseButton.image.sprite = _playIcon;
		}
		else
		{
			ResumeGame();
			_isOnPause = false;
			_pauseButton.image.sprite = _pauseIcon;
		}
	}

	private void PauseGame()
	{
		Time.timeScale = 0;
	}

	private void ResumeGame()
	{
		Time.timeScale = 1;
	}

	private void UpdateTimer()
	{
		_timer.RemoveTime(Time.deltaTime);
		if (_timer.CurrentTime >= 0)
		{
			_timerText.text = _timer.ConvertToMinutes(_timer.CurrentTime);
		}
		else
		{
			PauseGame();
			_gameOverPanel.SetActive(true);
		}
	}
}
