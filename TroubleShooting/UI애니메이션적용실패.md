# 🧩 [손유민] 트러블슈팅: UI Panel 애니메이션

## 📌 문제 상황  
UI Panel을 오픈할 때 `AnimationCurve`를 사용하여 **애니메이션 효과**를 주고자 했으나,  
패널이 **즉시 표시되며 애니메이션이 적용되지 않는 현상** 발생.

---

## 🧪 본래 시도
- `OnEnable()` 시점에 스케일 값을 `AnimationCurve`로 점진적으로 증가시키는 방식 사용.
- `Invoke()`로 메서드 호출.
- 애니메이션은 `OpenAnimation()` 함수 내 `while` 루프에서 처리.

```csharp
// 잘못된 방식 (코루틴 없이 while문만 사용)
private void OpenAnimation()
{
    while (true)
        time += Time.deltaTime * timeRate;
    if (time > 1.0f)
    {
        time = 1.0f;
        transform.localScale = new Vector3(curve.Evaluate(time), 1, 1);
        return;
    }
    transform.localScale = new Vector3(curve.Evaluate(time), 1, 1);
}
```

---

## ❗ 문제
- `OpenAnimation()` 함수에서 **`while` 루프가 한 프레임 안에 모두 실행**됨.
- 결과적으로 시간 누적과 스케일 변화가 한 번에 처리되어, **애니메이션이 생략된 것처럼 동작**.

---

## ✅ 최종 해결 방법
- `OpenAnimation()`을 **`IEnumerator`로 변경**하여 **`yield return null`을 통해 매 프레임 처리**.
- 시간 누적을 `Mathf.Clamp()`로 보정하여 **예외 상황 방지**.

```csharp
// 수정된 방식 (정상 작동)
private IEnumerator OpenAnimation()
{
    while (time < 1.0f)
    {
        time += Time.deltaTime;
        float clampedTime = Mathf.Clamp(time, 0f, 1f);
        transform.localScale = new Vector3(curve.Evaluate(clampedTime), 1, 1);
        yield return null;
    }
}
```

---

## 🎯 결과
- 패널 오픈 시 **부드럽게 스케일 애니메이션이 적용**됨.
- UI 연출의 자연스러움 향상 및 코드 안정성 개선.
