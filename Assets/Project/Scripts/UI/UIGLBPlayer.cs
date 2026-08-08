using UnityEngine;
using UnityEngine.UI;

public class UIGLBPlayer : MonoBehaviour
{
    private GLBAnimationPlayer _player;

    public GLBAnimationPlayer Player => _player;
    
    public RectTransform rectProgress;
    public RectTransform rectMarkContent;
    public Text textFrame;
    public Text textMaxFrame;
    public Button buttonPlay;
    public Button buttonStop;
    public Button buttonPause;
    public Button buttonResume;
    
    public void SetPlayer(GLBAnimationPlayer player)
    {
        this._player = player;
    }
    void Start()
    {
    }

    void Update()
    {
        textFrame.text = $"{_player?.CurrentFrame ?? '-'}";
        textMaxFrame.text = $"/ {_player?.MaxFrame ?? '-'}";

        if (_player)
        {
            if (Input.GetKeyDown(KeyCode.Space)) {
                if (!_player.Playing) _player.Play();
                else if (_player.Paused) _player.Resume();
                else _player.Pause();
            } else if (Input.GetKeyDown(KeyCode.Escape)) {
                if (_player.Playing) _player.Stop();
            }
        }
    
    }

    private void FixedUpdate()
    {
        UpdateProgress();
        buttonPlay.gameObject.SetActive(!_player?.Playing ?? false);
        buttonStop.gameObject.SetActive(_player?.Playing ?? false);
        buttonPause.gameObject.SetActive(!_player?.Paused ?? false);
        buttonResume.gameObject.SetActive(_player?.Paused ?? false);
    }

    void UpdateProgress()
    {
        if (!rectProgress || !_player)
        {
            rectProgress.sizeDelta = new Vector2(0, rectProgress.sizeDelta.y);
            return;
        }
        var maxSizeX = rectProgress.parent.GetComponent<RectTransform>().rect.width;
        rectProgress.sizeDelta = new Vector2(maxSizeX * _player.Progress, rectProgress.sizeDelta.y);
    }

    public void ActionPlay()
    {
        _player?.Play();
    }
    public void ActionStop()
    {
        _player?.Stop();
    }
    public void ActionPause()
    {
        _player?.Pause();
    }
    public void ActionResume()
    {
        _player?.Resume();
    }
}
