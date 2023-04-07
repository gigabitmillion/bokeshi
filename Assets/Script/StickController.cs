using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// DOTweenを使用した揺れ
/// </summary>
public class StickController : MonoBehaviour
{
    private Tweener _shakeTweener;
    private Vector3 _initPosition;

    [SerializeField] private GameObject stickWhite;

    //振動時の効果音
    public AudioClip sound1;
    AudioSource audioSource;

    /// <summary>
    /// 最初に呼ばれる
    /// </summary>
    void Start()
    {
        // 初期位置を保持
        _initPosition = transform.position;

        // AudioSource コンポーネントの初期化 // 修正箇所
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

    }

    // 他のオブジェクトに接触したときに呼ばれるメソッドを追加
    void OnCollisionEnter2D(Collision2D collision)
    {
        // 衝突したオブジェクトが Line タグを持っている場合のみ、振動を開始する // 修正箇所
        if (collision.gameObject.CompareTag("Line"))
        {
            StartShake(0.5f, 1.0f, 10, 90, true); // 振動を開始する（パラメータはお好みで調整してください）
        }
    }

    /// <summary>
    /// 揺れ開始
    /// </summary>
    /// <param name="duration">時間</param>
    /// <param name="strength">揺れの強さ</param>
    /// <param name="vibrato">どのくらい振動するか</param>
    /// <param name="randomness">ランダム度合(0〜180)</param>
    /// <param name="fadeOut">フェードアウトするか</param>
    public void StartShake(float duration, float strength, int vibrato, float randomness, bool fadeOut)
    {
        // 前回の処理が残っていれば停止して初期位置に戻す
        if (_shakeTweener != null)
        {
            _shakeTweener.Kill();
            gameObject.transform.position = _initPosition;
        }


        // 揺れ開始
        audioSource.PlayOneShot(sound1);
        _shakeTweener = gameObject.transform.DOShakePosition(duration, strength, vibrato, randomness, fadeOut)
        .OnComplete(() => // _shakeTweenerの完了時に以下の処理を追加
              {
                  // StickBlackを削除
                  Destroy(gameObject);

                  // 同じ位置にStickWhiteを生成
                  Instantiate(stickWhite, _initPosition, Quaternion.identity);
              });

    }

}
