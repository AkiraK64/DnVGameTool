using PrimeTween;
using UnityEngine;

namespace DnVCorp
{
    namespace Tweening
    {
        public static class TweenHelper
        {
            public static Sequence TweenJump(Transform target, Vector3 endValue, float height, float duration, Ease Ease = Ease.Default, int Loops = 1, CycleMode LoopMode = CycleMode.Restart, bool useUnscaledTime = false, bool useFixedUpdate = false)
            {
                Vector3 aPoint = target.position;
                Vector3 bPoint = 0.5f * (aPoint + endValue) + Vector3.up * 2f * height;
                Vector3 sPoint = Vector3.zero;
                Vector3 ePoint = Vector3.zero;
                float time = 0f;
                Sequence sequence = Sequence.Create(Loops, LoopMode, Ease, useUnscaledTime, useFixedUpdate)
                                            .Group(Tween.Custom(aPoint, bPoint, duration, onValueChange: newVal => sPoint = newVal))
                                            .Group(Tween.Custom(bPoint, endValue, duration, onValueChange: newVal => ePoint = newVal))
                                            .Group(Tween.Custom(0f, 1f, duration, onValueChange: newVal => { time = newVal; target.position = sPoint * (1f - time) + ePoint * time; }));
                return sequence;
            }

            public static Sequence TweenLocalJump(Transform target, Vector3 endValue, float height, float duration, Ease Ease = Ease.Default, int Loops = 1, CycleMode LoopMode = CycleMode.Restart, bool useUnscaledTime = false, bool useFixedUpdate = false)
            {
                Vector3 aPoint = target.position;
                Vector3 bPoint = 0.5f * (aPoint + endValue) + Vector3.up * 2f * height;
                Vector3 sPoint = Vector3.zero;
                Vector3 ePoint = Vector3.zero;
                float time = 0f;
                Sequence sequence = Sequence.Create(Loops, LoopMode, Ease, useUnscaledTime, useFixedUpdate)
                                            .Group(Tween.Custom(aPoint, bPoint, duration, onValueChange: newVal => sPoint = newVal))
                                            .Group(Tween.Custom(bPoint, endValue, duration, onValueChange: newVal => ePoint = newVal))
                                            .Group(Tween.Custom(0f, 1f, duration, onValueChange: newVal => { time = newVal; target.localPosition = sPoint * (1f - time) + ePoint * time; }));
                return sequence;
            }

            public static Sequence TweenRectJump(RectTransform target, Vector2 startValue, Vector2 endValue, float height, float duration, Ease Ease = Ease.Default, int Loops = 1, CycleMode LoopMode = CycleMode.Restart, bool useUnscaledTime = false, bool useFixedUpdate = false)
            {
                target.anchoredPosition = startValue;
                Vector2 aPoint = startValue;
                Vector2 bPoint = 0.5f * (aPoint + endValue) + Vector2.up * 2f * height;
                Vector2 sPoint = Vector3.zero;
                Vector2 ePoint = Vector3.zero;
                float time = 0f;
                Sequence sequence = Sequence.Create(Loops, LoopMode, Ease, useUnscaledTime, useFixedUpdate)
                                            .Group(Tween.Custom(aPoint, bPoint, duration, onValueChange: newVal => sPoint = newVal))
                                            .Group(Tween.Custom(bPoint, endValue, duration, onValueChange: newVal => ePoint = newVal))
                                            .Group(Tween.Custom(0f, 1f, duration, onValueChange: newVal => { time = newVal; target.anchoredPosition = sPoint * (1f - time) + ePoint * time; }));
                return sequence;
            }

            public static Sequence TweenRect3DJump(RectTransform target, Vector3 endValue, float height, float duration, Ease Ease = Ease.Default, int Loops = 1, CycleMode LoopMode = CycleMode.Restart, bool useUnscaledTime = false, bool useFixedUpdate = false)
            {
                Vector3 aPoint = target.anchoredPosition3D;
                Vector3 bPoint = 0.5f * (aPoint + endValue) + Vector3.up * 2f * height;
                Vector3 sPoint = Vector3.zero;
                Vector3 ePoint = Vector3.zero;
                float time = 0f;
                Sequence sequence = Sequence.Create(Loops, LoopMode, Ease, useUnscaledTime, useFixedUpdate)
                                            .Group(Tween.Custom(aPoint, bPoint, duration, onValueChange: newVal => sPoint = newVal))
                                            .Group(Tween.Custom(bPoint, endValue, duration, onValueChange: newVal => ePoint = newVal))
                                            .Group(Tween.Custom(0f, 1f, duration, onValueChange: newVal => { time = newVal; target.anchoredPosition3D = sPoint * (1f - time) + ePoint * time; }));
                return sequence;
            }

            public static Sequence TweenPath(Transform target, Vector3[] points, float duration, Ease Ease = Ease.Default, int Loops = 1, CycleMode LoopMode = CycleMode.Restart, bool useUnscaledTime = false, bool useFixedUpdate = false)
            {
                if (points.Length < 2) return Sequence.Create(Loops, LoopMode, Ease, useUnscaledTime, useFixedUpdate);
                int index = 0;
                float totalDistance = 0f;
                float speed = 0.1f;

                Vector3[] vectors = new Vector3[points.Length - 1];
                for (int i = 1; i < points.Length; i++)
                {
                    vectors[i - 1] = points[i] - points[i - 1];
                    totalDistance += Vector3.Distance(points[i - 1], points[i]);
                }
                speed = duration > 0f ? totalDistance / duration : 0.1f;

                target.position = points[0];
                target.rotation = Quaternion.LookRotation((points[1] - points[0]).normalized);

                Sequence sequence = Sequence.Create(Loops, LoopMode, Ease, useUnscaledTime, useFixedUpdate);
                for (int i = 0; i < points.Length - 1; i++)
                {
                    sequence.Chain(Tween.Position(target, points[i] + vectors[i], vectors[i].magnitude / speed, Ease));
                    sequence.ChainCallback(() =>
                    {
                        index += 1;
                        if (index < vectors.Length)
                        {
                            target.rotation = Quaternion.LookRotation(vectors[index].normalized);
                        }
                    });
                }
                return sequence;
            }

            public static Sequence TweenLocalPath(Transform target, Vector3[] points, float duration, Ease Ease = Ease.Default, int Loops = 1, CycleMode LoopMode = CycleMode.Restart, bool useUnscaledTime = false, bool useFixedUpdate = false)
            {
                if (points.Length < 2) return Sequence.Create(Loops, LoopMode, Ease, useUnscaledTime, useFixedUpdate);
                int index = 0;
                float totalDistance = 0f;
                float speed = 0.1f;

                Vector3[] vectors = new Vector3[points.Length - 1];
                for (int i = 1; i < points.Length; i++)
                {
                    vectors[i - 1] = points[i] - points[i - 1];
                    totalDistance += Vector3.Distance(points[i - 1], points[i]);
                }
                speed = duration > 0f ? totalDistance / duration : 0.1f;

                target.localPosition = points[0];
                target.rotation = Quaternion.LookRotation((points[1] - points[0]).normalized);

                Sequence sequence = Sequence.Create(Loops, LoopMode, Ease, useUnscaledTime, useFixedUpdate);
                for (int i = 0; i < points.Length - 1; i++)
                {
                    sequence.Chain(Tween.LocalPosition(target, points[i] + vectors[i], vectors[i].magnitude / speed, Ease));
                    sequence.ChainCallback(() =>
                    {
                        index += 1;
                        if (index < vectors.Length)
                        {
                            target.rotation = Quaternion.LookRotation(vectors[index].normalized);
                        }
                    });
                }
                return sequence;
            }
        }
    }
}
