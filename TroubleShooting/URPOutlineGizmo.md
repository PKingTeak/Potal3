
# [김선우] 트러블슈팅: Unity URP Outline Gizmo

## 🎯 목적

Unity 에디터처럼 선택한 오브젝트에만 외곽선(Outline)을 보여주는 런타임 기즈모 기능을 구현하고자 하였음.

## 📌 1. 배경

런타임에서도 Unity Editor와 유사한 에디터를 구현하기 위해 기즈모 시스템 제작을 시작함.
첫 단계로 선택된 오브젝트에 Outline 효과를 적용하는 기능을 구현하고자 함.
URP 환경 기반으로 Shader 작성 + ScriptableRendererFeature / ScriptableRenderPass 활용.

## 📌 2. 문제 상황

- selectObject에 outline 적용 시도 후, 모든 오브젝트가 마젠타색으로 렌더링됨.
- Outline 대상이 아닌 오브젝트까지 전부 깨져 보이는 현상 발생.
- Material 교체나 Shader 적용 후 씬 전체가 깨지는 듯한 비정상적 렌더링 상태.

## 📌 3. 디버깅 흐름

1. Shader 코드 확인
    - 직접 작성한 URP용 OutlineShader 내부 로직 점검
    - 예상되는 에러 없음
2. Camera 설정 확인
    - ForwardRenderer 설정
    - ScriptableRendererFeature 등록 및 카메라에 적용 여부 확인
3. Material 적용 방식 확인
    - 기존에 사용 중이던 Standard Shader 기반 Material에 커스텀 URP Shader를 적용했던 것이 문제

## 📌 4. 원인 분석

- Unity에서는 Shader가 호환되지 않으면 그 결과를 마젠타색(에러)으로 출력함.
- URP에서는 Built-in(Standard) Material이나 Shader가 호환되지 않음.
- 직접 만든 Outline Shader는 URP용으로 작성되어 있었지만, 기존 Material들이 URP 대응이 안 되어 있었음.

## ✅ 5. 해결 방법

1. 모든 오브젝트의 Material을 URP 호환용으로 교체
    - 새로 작성한 URP 기반 Shader를 이용한 Material 생성
    - 씬 내 모든 오브젝트에 해당 Material을 적용
2. 마젠타 색상 문제 해결 확인
    - 이후 마젠타로 출력되던 문제는 해결됨
    - 씬 렌더링 정상적으로 동작

## 🔄 6. 추가 시도: GL 기반 Outline

- URP 기반 Outline을 구현했지만, **오브젝트 크기에 따라 선 굵기가 달라지는 문제** 발생
- 작은 오브젝트에는 외곽선이 너무 얇아 잘 보이지 않아 목표에 맞지 않다고 판단
- **GL 기반 라인 렌더링으로 방향 전환**
    - 처음에는 GL 방식으로 그린 선의 굵기를 조절할 수 없음을 확인
    - 이후 선을 **여러 번 겹쳐 그리는 방식**으로 굵은 선 구현 시도
    - GL로 그린 선이 오브젝트 크기와 무관하게 일정한 두께로 렌더링되도록 처리

✅ 결과적으로 작은 오브젝트도 잘 보이는 **균일한 두께의 외곽선**을 표현하는 데 성공

## 💡 교훈

- URP 프로젝트에서는 Shader, Material, RenderPass 모두 URP 전용으로 구성해야 함.
- Material이 URP용이 아닐 경우, 커스텀 Shader를 씌워도 렌더링 에러(마젠타) 발생.
- 커스텀 Shader를 사용하려면 Shader뿐만 아니라 Material도 호환되도록 재생성 필요.
- **GL 기반 Outline**은 Shader를 통하지 않고, 씬 뷰 내에서 **일관된 시각 효과를 줄 수 있는 강력한 방법**임.
