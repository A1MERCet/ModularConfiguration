using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGLBPlayer : MonoBehaviour
{
    private GLBAnimationPlayer _player;
    
    public GLBAnimationPlayer Player => _player;

    private UIGLBPlayerTimeline _timeline;
    
    public RectTransform rectProgress;
    
    public Text textFrame;
    public Text textMaxFrame;
    public Button buttonPlay;
    public Button buttonStop;
    public Button buttonPause;
    public Button buttonResume;
    
    public void SetPlayer(GLBAnimationPlayer player)
    {
        if (_player) _player.onSetBehaviourMWFGun -= OnSetBehaviourMWFGun;
        _player = player;
        if (_player) _player.onSetBehaviourMWFGun += OnSetBehaviourMWFGun;
    }

    private void OnSetBehaviourMWFGun(BehaviourMWFGun behaviour)
    {
        _timeline.SetMaxFrame(_player?.MaxFrame ?? 0);
        UpdateTimelineMarks();
    }

    private void UpdateTimelineMarks()
    {
        if (_player)
        {
            List<UIGLBPlayerTimeline.Mark> marks = new();
            foreach (var (k, v) in _player.GetAnimaStates())
                marks.Add(new UIGLBPlayerTimeline.Mark() {
                    id = k,
                    name = $"{ModularConfiguration.instance.mwfProperty.GetLang($"anima.{v.name}")}\n{v.name}",
                    start = v.startTime,
                    end = v.endTime,
                    color = ModularConfiguration.instance.mwfProperty.GetAnimaStageColor(v.name)
                });
            _timeline.SetMarks(marks);
        }
    }

    private void Awake()
    {
        _timeline = GetComponentInChildren<UIGLBPlayerTimeline>();
        _timeline.parent = this;
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

    public void UpdateAnimaStageMarks()
    {
        
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
        rectProgress.sizeDelta = new Vector2(_timeline.showFrame == 0F ? 0 : maxSizeX * Mathf.Clamp(_player.CurrentFrame / _timeline.showFrame, 0F, 1F), rectProgress.sizeDelta.y);
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
