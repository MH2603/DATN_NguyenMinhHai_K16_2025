using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace MH.UISystem
{
    [CreateAssetMenu(fileName = "FadeAnimation", menuName = "MH_SO/UIAnimation/Create Fade Animation", order = 0)]
    public class FadeAnimation : BaseViewAnimation
    {
        [SerializeField] private float _from;
        [SerializeField] private float _to;
        [SerializeField] private float _startDelay;
        [SerializeField] private float _duration = 0.25f;
        [SerializeField] private Ease _ease = Ease.Linear;

        public override async UniTask AnimateAsync(RectTransform rectTransform, CanvasGroup canvasGroup)
        {
            canvasGroup.alpha = _from;

            canvasGroup.DOFade(_to, _duration).SetDelay(_startDelay).SetUpdate(true).SetEase(_ease);
            float awaitDur = _startDelay + _duration;

            await UniTask.Delay((int)(awaitDur * 1000), DelayType.UnscaledDeltaTime);

            //await canvasGroup.DOFade(_to, _duration).SetDelay(_startDelay).SetEase(_ease).SetUpdate(true).ToUniTask();
        }
    }
}
