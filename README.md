# Harvest League Core Loop Prototype Spec

## 1. 이번 30초 영상용 프로토타입의 한 줄 콘셉트
`작물 재배 -> 수확 -> 운반 -> 판매 -> 점수 획득`을 30초 안에 두 번 반복해, 느긋한 농사게임이 아니라 바쁘고 명확한 목표 중심의 팀 전략 농장 게임이라는 점을 증명하는 탑다운 2D 프로토타입.

## 2. 프로토타입 목표
- 심사위원이 설명 없이 봐도 핵심 루프를 이해할 수 있어야 한다.
- `씨앗 구매`, `밭 갈기`, `파종`, `성장`, `수확`, `운반`, `판매`, `점수 상승`이 한 화면 흐름 안에서 연결되어야 한다.
- 플레이 감각은 힐링형이 아니라 `짧은 목표`, `빈번한 이동`, `즉각 피드백` 중심이어야 한다.
- 정식 버전의 3 vs 3 팀플레이로 확장 가능한 구조여야 한다.
- 구현 범위는 Unity 2D에서 1인 개발 기준으로 1차 시연 가능 수준이어야 한다.

## 3. 가장 작은 플레이 가능한 구조 (MVP)

| 항목 | 사양 |
| --- | --- |
| 플레이 인원 | 1인 |
| 시점 | 탑다운 2D |
| 한 판 길이 | 45초 |
| 영상 사용 구간 | 마지막 30초 또는 시작부터 30초 |
| 밭 타일 수 | 6칸 (2 x 3) |
| 작물 수 | 2종 |
| 씨앗 소스 | 1개 Seed Shop |
| 물 소스 | 1개 Water Pump |
| 판매 지점 | 1개 Selling Crate |
| 승리 판정 | 제한시간 종료 시 목표 점수 달성 여부 |
| 핵심 감각 | 짧은 동선, 빠른 상태 변화, 큰 점수 피드백 |

### 추천 플레이 공간 배치
- 좌측: `Seed Shop`
- 중앙: `Farm Tiles 6칸`
- 상단: `Water Pump`
- 우측: `Selling Crate`
- 상단 UI: `Timer / Score / Goal`

이 배치는 이동 자체가 플레이가 되도록 만든다. 즉, 한 자리에서 농사만 짓는 구조가 아니라 `스테이션 사이를 오가며 운영하는 구조`를 바로 읽히게 한다.

## 4. Gameplay Loop
1. Seed Shop에서 씨앗 구매
2. 빈 밭 타일 갈기
3. 씨앗 심기
4. Water Pump 또는 물 주기로 성장 가속
5. 햇빛으로 자동 성장 진행
6. 작물 준비 상태 도달
7. 수확 후 손에 들고 이동
8. Selling Crate에 판매
9. 점수 상승 확인
10. 제한시간 종료 시 목표 점수와 비교

## 5. Core Mechanics

| 메커닉 | 최소 구현 방식 | 영상에서 보이는 효과 |
| --- | --- | --- |
| 씨앗 구매 | 상점 앞 상호작용으로 2종 중 선택 구매 | 씨앗 아이콘, 코인 감소 |
| 밭 갈기 | 빈 타일에 상호작용 | 흙 색 변화, 갈린 테두리 표시 |
| 파종 | 갈린 타일에 씨앗 사용 | 씨앗 아이콘 사라짐, 새싹 등장 |
| 성장 | 햇빛 자동 누적 + 물 주기 즉시 보너스 | 성장 바 증가, 스프라이트 단계 변화 |
| 수확 | Ready 상태에서 상호작용 | 작물 뽑기 애니메이션, 손에 들기 |
| 운반 | 수확물 소지 중 이동 | 머리 위 또는 손에 든 작물 표시 |
| 판매 | 판매대 상호작용 | 점수 팝업, 목표 게이지 상승 |
| 승패 확인 | 45초 종료 시 결과 표시 | Victory / Defeat 배너 |

## 6. 플레이어 행동 목록
- 이동
- 상호작용
- 씨앗 구매
- 밭 갈기
- 씨앗 심기
- 물 채우기
- 물 주기
- 수확
- 수확물 운반
- 판매

### 입력 최소 구성
- `WASD` 또는 방향키: 이동
- `E`: 상호작용

## 7. 필요한 게임 오브젝트 목록

| 오브젝트 | 역할 | 필수 컴포넌트/데이터 |
| --- | --- | --- |
| Player | 이동 및 모든 상호작용 수행 | Rigidbody2D, Collider2D, PlayerInteractor, CarrySlot |
| Seed Shop | 씨앗 구매 | ShopInteractable, CropData List |
| Water Pump | 물 보충 | WaterSourceInteractable |
| Farm Tile | 경작, 파종, 성장 표시 | TileState, CropSlot, SpriteRenderer |
| Crop Entity | 성장/수확 가능 작물 | CropData, GrowthController |
| Selling Crate | 수확물 판매 | SellInteractable |
| Match Manager | 시간, 점수, 승패 판정 | Timer, ScoreManager, ResultManager |
| UI Canvas | 정보 전달 | TimerText, ScoreText, GoalText, HeldItemIcon, Prompt |
| CropData x 2 | 작물별 수치 정의 | 이름, 가격, 성장값, 판매점수, 스프라이트 |

## 8. 각 오브젝트의 상태 변화

### Player
`Idle -> Moving -> Interacting -> CarryingCrop -> Selling -> Idle`

### Farm Tile
`Untilled -> Tilled -> Seeded -> Growing -> Ready -> Untilled`

### Crop Entity
`Seed -> Sprout -> Mid -> Mature -> Harvested`

### Seed Shop
`Available -> PurchaseFlash -> Available`

### Water Pump
`Ready -> Refill -> Ready`

### Selling Crate
`Idle -> Selling -> ScorePopup -> Idle`

### Match Flow
`Ready -> Playing -> TimeUp -> Result`

## 9. 작물 성장 시스템 최소 설계

### 설계 원칙
- 성장 공식을 단순하게 유지한다.
- `자동 성장`과 `플레이어 개입 성장`을 동시에 보여준다.
- 스프라이트 단계가 2회 이상 바뀌어야 시각적으로 읽힌다.

### 최소 성장 규칙
- 모든 작물은 `Growth Point`를 가진다.
- `Sunlight`는 초당 자동으로 Growth Point를 올린다.
- `Watering`은 즉시 추가 Growth Point를 준다.
- Growth Point가 임계값을 넘으면 작물 스프라이트 단계가 바뀐다.
- 최대치 도달 시 `Ready` 상태가 된다.

### 추천 작물 2종

| 작물 | 포지션 | 씨앗 가격 | 최대 성장치 | 햇빛 증가량 | 물 보너스 | 필요 물 횟수 체감 | 판매 점수 |
| --- | --- | --- | --- | --- | --- | --- | --- |
| Tomato | 빠른 회전용 | 1 Coin | 100 | 초당 10 | +35 | 1회 | 100 |
| Pumpkin | 고점용 | 2 Coin | 100 | 초당 8 | +30 | 2회 | 200 |

### 성장 단계 기준

| 성장치 구간 | 상태 | 시각 피드백 |
| --- | --- | --- |
| 0-24 | Seed | 작은 씨앗/흙만 보임 |
| 25-49 | Sprout | 초록 새싹 등장 |
| 50-84 | Mid | 줄기/잎 확대, 성장 바 절반 이상 |
| 85-99 | Mature | 열매 형태가 보이기 시작 |
| 100 | Ready | 반짝임, 외곽선, 수확 아이콘 |

### 물 주기 설계
- 플레이어는 Water Pump에서 `물 2회분`을 채운다.
- 물 한 번 사용 시 `waterCharge -1`
- 물 주는 즉시 타일이 젖은 상태로 2초간 표시된다.
- 젖은 상태는 성능 효과보다 `피드백용 표시`가 목적이다.

## 10. 판매 및 점수 시스템 최소 설계

### 점수 구조
- 판매만이 점수 획득 수단이다.
- 씨앗 구매용 Coin과 승패용 Score를 분리한다.
- 판매 시 `Score`와 `Coin`을 동시에 얻는다.

| 행동 | Score | Coin |
| --- | --- | --- |
| Tomato 판매 | +100 | +1 |
| Pumpkin 판매 | +200 | +2 |

### 추천 초기값
- 시작 Coin: `3`
- 시작 Score: `0`
- 목표 Score: `300`

이 값이면 `Tomato 1개 + Pumpkin 1개 판매`로 목표 달성이 가능해 30초 영상에서 루프 두 번만 보여도 결과가 나온다.

## 11. 승리조건 / 실패조건

### 승리조건
- 45초 종료 시 `Score >= Goal Score`

### 실패조건
- 45초 종료 시 `Score < Goal Score`

### 왜 이 방식이 적합한가
- 1인 프로토타입에서도 `경쟁적 목표`를 유지할 수 있다.
- 정식 버전에서는 같은 시스템을 `팀 점수 비교`로 바로 치환할 수 있다.

## 12. UI 최소 구성

| UI 요소 | 위치 | 목적 |
| --- | --- | --- |
| Timer | 상단 중앙 | 경기 압박감 전달 |
| Score | 상단 좌측 | 현재 성과 확인 |
| Goal Score | 상단 우측 | 명확한 목표 제시 |
| Coin | Score 아래 | 씨앗 구매 가능 여부 표시 |
| Held Item Icon | 하단 중앙 | 현재 손에 든 씨앗/작물 확인 |
| Water Charge | 하단 우측 | 물 주기 가능 횟수 표시 |
| Interaction Prompt | 오브젝트 근처 | 설명 없는 조작 유도 |
| Growth Bar | 작물 위 | 성장 상태를 한눈에 전달 |
| Score Popup | 판매대 위 | 즉각적 보상 전달 |

### 무자막 영상에서도 반드시 보여야 하는 UI
- Timer
- Score
- Goal Score
- Growth Bar
- Held Item Icon
- Score Popup

## 13. Recommended Variable Values

| 항목 | 권장값 | 이유 |
| --- | --- | --- |
| Match Length | 45초 | 짧고 압박감 있는 한 판 |
| Video Segment | 30초 | 핵심 루프 2회 시연 가능 |
| Player Move Speed | 4.5 | 좁은 맵에서 분주한 인상 |
| Interaction Time | 0.25초 | 템포 유지 |
| Tile Count | 6 | 확장성 + 화면 가독성 균형 |
| Water Capacity | 2 | 물 공급 이동을 강제하되 답답하지 않음 |
| Tomato Grow Time | 약 7초 | 첫 성공 경험 빠름 |
| Pumpkin Grow Time | 약 10초 | 고점 선택의 의미 부여 |
| Carry Speed Multiplier | 0.9 | 운반 행동이 보이게 함 |
| Result Screen Hold | 2초 | 심사 영상 마무리 컷 확보 |

## 14. Pseudocode for Interaction Flow

```csharp
OnInteract(target):
    if target is SeedShop:
        if player.coin >= selectedSeed.cost and player.heldItem == None:
            player.coin -= selectedSeed.cost
            player.heldItem = selectedSeed
            UI.ShowHeldItem(selectedSeed)

    else if target is FarmTile:
        if target.state == Untilled and player.heldItem == None:
            target.SetState(Tilled)

        else if target.state == Tilled and player.heldItem is Seed:
            target.Plant(player.heldItem.cropData)
            player.heldItem = None

        else if target.state == Growing and player.waterCharge > 0:
            target.crop.AddGrowth(target.cropData.waterBonus)
            player.waterCharge -= 1
            target.ShowWetFeedback()

        else if target.state == Ready and player.heldItem == None:
            player.heldItem = target.Harvest()
            target.SetState(Untilled)

    else if target is WaterPump:
        player.waterCharge = 2

    else if target is SellingCrate:
        if player.heldItem is Crop:
            score += player.heldItem.sellScore
            coin += player.heldItem.sellCoin
            UI.ShowScorePopup(player.heldItem.sellScore)
            player.heldItem = None

UpdateCrop(dt):
    if crop.state is not Ready:
        crop.growth += cropData.sunlightRate * dt
        crop.UpdateVisualStage()
        if crop.growth >= cropData.maxGrowth:
            crop.SetReady()

OnTimeUp():
    if score >= goalScore:
        ShowResult("Victory")
    else:
        ShowResult("Defeat")
```

## 15. 30초 영상에서 가장 잘 보이는 시연 시나리오

### 권장 촬영 조건
- 45초 경기의 `남은 시간 30초부터 0초까지`를 사용
- 고정 카메라 또는 아주 약한 추적 카메라
- 오브젝트 외곽선과 상태 색 변화 강조

### 30초 데모 이벤트 플로우

| 시간 | 플레이어 행동 | 화면에서 읽히는 정보 |
| --- | --- | --- |
| 00-03초 | Tomato 씨앗 구매 | Seed Shop, Coin 감소, 씨앗 아이콘 |
| 03-05초 | 타일 1칸 갈기 | 흙 색 변화 |
| 05-06초 | Tomato 파종 | 씨앗 소모, 밭에 심김 |
| 06-08초 | Water Pump에서 물 채우기 후 물 주기 | 물 게이지 감소, 새싹 등장 |
| 08-11초 | Pumpkin 씨앗 구매 | 다른 작물 선택, 전략 차이 암시 |
| 11-13초 | 타일 2칸 갈기 + Pumpkin 파종 | 병렬 운영 시작 |
| 13-15초 | Pumpkin 첫 물 주기 | 성장 바 즉시 증가 |
| 15-17초 | Tomato Ready 도달 | 반짝임, 수확 가능 아이콘 |
| 17-19초 | Tomato 수확 후 판매대 이동 | 손에 든 작물, 운반 동선 |
| 19-20초 | Tomato 판매 | +100 Score, Goal 진행 |
| 20-22초 | Water Pump 재보충 후 Pumpkin 두 번째 물 주기 | 성장 가속 |
| 22-25초 | Pumpkin Ready 도달 및 수확 | 더 큰 작물, 고가 작물 인지 |
| 25-27초 | Pumpkin 판매 | +200 Score, 총점 300 도달 |
| 27-30초 | 타이머 종료 및 Victory 표시 | 목표 달성 결과 확인 |

이 시나리오는 `빠른 작물 1개`와 `고점 작물 1개`만으로도 선택, 운영, 우선순위의 차이를 보여준다.

## 16. 이 프로토타입이 기존 농사게임과 어떻게 다른지 비교 설명

| 항목 | 일반 힐링형 농사게임 | Harvest League 프로토타입 |
| --- | --- | --- |
| 플레이 템포 | 느리고 장기적 | 짧고 압축적 |
| 핵심 감정 | 안정, 수집, 꾸미기 | 압박, 우선순위 판단, 운영 |
| 목표 구조 | 개인 성장 중심 | 제한시간 내 점수 달성 |
| 동선 | 밭 중심 정착형 | 스테이션 왕복형 |
| 피드백 속도 | 분 단위 누적 | 초 단위 즉시 피드백 |
| 확장 방향 | 생활 시뮬레이션 | 팀 역할 분담과 경쟁 |

핵심 차이는 `농사 행위가 휴식이 아니라 경기 운영의 재료`라는 점이다. 이 프로토타입은 그 차이를 `짧은 타이머`, `운반`, `판매`, `목표 점수`, `물/씨앗 스테이션 이동`으로 증명한다.

## 17. 향후 정식 버전에서 역할군/온실 경쟁/방해 요소로 어떻게 확장되는지

### 역할군 확장
- 농사꾼: 갈기, 파종, 물 주기 속도 보너스
- 운반가: 이동속도 보너스, 판매 효율 보너스
- 지원가: 성장 가속, 아군 버프, 급수 범위 지원
- 방해꾼: 적 타일 방해, 운반 차단, 중앙 오브젝트 쟁탈

### 중앙 온실 확장
- 현재 `Goal Score` 시스템을 정식 버전에서는 `온실 황금 작물 점유 점수`로 확장
- 필드 농사는 기본 점수 수급
- 온실은 고위험 고보상 목표
- 즉, 지금의 판매 루프는 정식 버전의 하위 경제 루프로 유지된다

### 방해 요소 확장
- 적의 물통 비우기
- 밭 일시 봉쇄
- 운반 경로 차단
- 수확물 탈취 또는 판매 방해

현재 프로토타입에서 중요한 것은 이 확장을 전부 넣는 것이 아니라, `현재 오브젝트 구조만 유지해도 역할과 경쟁 시스템을 얹을 수 있다`는 점이다.

## 18. 구현 우선순위
1. 플레이어 이동과 단일 상호작용 시스템
2. Farm Tile 상태머신 `Untilled / Tilled / Growing / Ready`
3. Seed Shop 구매와 Held Item 처리
4. CropData 2종과 성장 수치 적용
5. Water Pump와 물 보너스 처리
6. Harvest / Carry / Sell 루프
7. Score / Coin / Timer UI
8. 승패 결과 화면
9. 상태별 스프라이트 교체와 점수 팝업
10. 영상용 카메라/연출 polish

## 19. 반드시 제외해야 할 과잉 기능
- NPC
- 퀘스트
- 제작
- 인테리어/꾸미기
- 계절 시스템
- 날씨 시스템의 실제 시뮬레이션
- 작물 품질 등급
- 인벤토리 다중 슬롯
- 도구 업그레이드
- 맵 확장
- 적 AI
- 멀티플레이 네트워크
- 스토리/대사
- 튜토리얼 팝업 다량 추가

이 프로토타입의 목적은 `세계관 소개`가 아니라 `핵심 루프 증명`이다.

## 20. Best Final Version for a Judging Video

### 가장 추천하는 최종 형태
- 1인 플레이
- 6타일 농장
- Tomato / Pumpkin 2종
- Seed Shop, Water Pump, Selling Crate 각 1개
- 45초 경기
- 목표 점수 300
- 30초 영상 안에 판매 2회와 최종 Victory까지 포함

### 연출 권장
- 자막 포함 버전: `Buy -> Till -> Plant -> Water -> Harvest -> Sell -> Score`를 1단어씩만 넣기
- 무자막 버전: UI 아이콘, 성장 바, 점수 팝업을 크게
- 내레이션 버전: "Harvest League는 농사를 경기처럼 운영하는 팀 전략 게임입니다" 한 문장으로 시작

## 21. 최종 추천 프로토타입 사양
- 1인용 탑다운 Unity 2D 프로토타입으로 제작한다.
- 맵은 `Seed Shop / Farm 6칸 / Water Pump / Selling Crate` 4구역만 둔다.
- 작물은 `Tomato`와 `Pumpkin` 2종만 사용한다.
- 플레이어는 씨앗 구매, 밭 갈기, 파종, 물 주기, 수확, 운반, 판매만 수행한다.
- 성장 시스템은 `햇빛 자동 증가 + 물 주기 즉시 보너스`의 2축만 사용한다.
- 판매 시 `Score`와 `Coin`을 함께 획득한다.
- 45초 경기 종료 시 `Score >= 300`이면 승리한다.
- 30초 영상 안에 최소 2회 판매와 점수 상승을 보여준다.
- 모든 핵심 상태는 스프라이트 변화, 성장 바, 점수 팝업으로 즉시 보이게 한다.
- 정식 버전 확장은 역할 분담, 중앙 온실 경쟁, 방해 요소를 같은 루프 위에 추가하는 방식으로 이어간다.

## 22. Unity Scene 구성안

### 씬 구조 원칙
- 모든 핵심 오브젝트는 한 화면 안에서 보이게 배치한다.
- 카메라는 맵 전체를 담되 플레이어 위치를 약하게 따라간다.
- 심사용 빌드는 `읽기 쉬움`이 우선이므로 레벨 장식은 최소화한다.

### 추천 Hierarchy
```text
MainGame
├─ GameRoot
│  ├─ MatchManager
│  ├─ ScoreManager
│  ├─ UIManager
│  └─ AudioManager (optional)
├─ Level
│  ├─ Ground
│  ├─ SeedShop
│  ├─ WaterPump
│  ├─ SellingCrate
│  └─ FarmGrid
│     ├─ Tile_01
│     ├─ Tile_02
│     ├─ Tile_03
│     ├─ Tile_04
│     ├─ Tile_05
│     └─ Tile_06
├─ Player
├─ CameraRoot
│  ├─ Main Camera
│  └─ CameraTarget
├─ UI
│  ├─ HUD
│  ├─ WorldSpacePrompts
│  └─ ResultPanel
└─ EventSystem
```

### 배치 기준
- `SeedShop -> FarmGrid -> SellingCrate`가 좌에서 우로 읽히게 둔다.
- `WaterPump`는 FarmGrid 상단에 둬서 플레이어가 위아래 왕복하게 만든다.
- 타일은 2 x 3 배열로 두고, 화면 중심에 배치한다.

## 23. Unity 프리팹 목록

| 프리팹 | 용도 | 포함 요소 |
| --- | --- | --- |
| `Player.prefab` | 플레이어 본체 | SpriteRenderer, Rigidbody2D, Collider2D, PlayerController, PlayerInteractor |
| `FarmTile.prefab` | 타일 상태 관리 | SpriteRenderer, Collider2D, FarmTile |
| `SeedShop.prefab` | 씨앗 구매 지점 | SpriteRenderer, Collider2D, SeedShopInteractable |
| `WaterPump.prefab` | 물 보충 지점 | SpriteRenderer, Collider2D, WaterPumpInteractable |
| `SellingCrate.prefab` | 판매 지점 | SpriteRenderer, Collider2D, SellStationInteractable |
| `CropVisual.prefab` | 작물 표시 | SpriteRenderer, GrowthBarAnchor, ReadyEffect |
| `Prompt.prefab` | 상호작용 안내 | World Space Canvas, TMP_Text |
| `FloatingScore.prefab` | 점수 팝업 | Animator, TMP_Text |
| `ResultPanel.prefab` | 승패 표시 | CanvasGroup, TMP_Text |

## 24. 필수 스크립트 목록과 책임 분리

| 스크립트 | 책임 | 비고 |
| --- | --- | --- |
| `PlayerController.cs` | 이동 입력, 이동 속도, 방향 갱신 | 입력과 이동만 담당 |
| `PlayerInteractor.cs` | 가장 가까운 상호작용 대상 탐색 및 실행 | `E` 키 처리 |
| `PlayerCarry.cs` | 손에 든 씨앗/작물 관리 | 단일 슬롯만 유지 |
| `PlayerWater.cs` | 물 충전량 관리 | `waterCharge` 전용 |
| `FarmTile.cs` | 타일 상태머신, 파종, 성장, 수확 | 핵심 루프 중심 스크립트 |
| `CropInstance.cs` | 성장 수치와 단계 계산 | 스프라이트 교체 담당 |
| `CropData.cs` | 작물 데이터 정의 ScriptableObject | Tomato, Pumpkin 2개 생성 |
| `SeedShopInteractable.cs` | 씨앗 구매 처리 | 코인 차감, 씨앗 지급 |
| `WaterPumpInteractable.cs` | 물 보충 처리 | 물 2회분 충전 |
| `SellStationInteractable.cs` | 판매 처리 | 점수/코인 지급 |
| `MatchManager.cs` | 타이머, 시작/종료, 승패 판정 | 전역 경기 흐름 |
| `ScoreManager.cs` | Score, Coin, Goal 값 관리 | UI와 분리 |
| `HUDController.cs` | 상단 UI 갱신 | Timer/Score/Coin/Goal |
| `WorldPromptUI.cs` | 상호작용 문구 표시 | 오브젝트 근처 표시 |
| `FloatingText.cs` | 판매 점수 팝업 | 재사용 가능 |

### 책임 분리 원칙
- `Player`는 입력과 소지 상태만 알고, 타일 내부 규칙은 모르게 한다.
- `FarmTile`은 자신의 상태와 작물 상태를 직접 관리한다.
- `ScoreManager`는 숫자만 관리하고, UI는 `HUDController`가 표시한다.
- 상호작용 지점은 모두 `IInteractable` 또는 공통 베이스 클래스를 사용한다.

## 25. 추천 데이터 구조

### Enum 정의
```csharp
public enum TileState
{
    Untilled,
    Tilled,
    Growing,
    Ready
}

public enum CropStage
{
    Seed,
    Sprout,
    Mid,
    Mature,
    Ready
}

public enum CarryItemType
{
    None,
    Seed,
    Crop
}
```

### CropData ScriptableObject 필드
```csharp
[CreateAssetMenu(menuName = "HarvestLeague/Crop Data")]
public class CropData : ScriptableObject
{
    public string cropId;
    public string displayName;
    public int seedCost;
    public int sellScore;
    public int sellCoin;
    public float maxGrowth;
    public float sunlightRate;
    public float waterBonus;
    public Sprite seedSprite;
    public Sprite sproutSprite;
    public Sprite midSprite;
    public Sprite matureSprite;
    public Sprite readySprite;
}
```

### CarryItem 최소 모델
```csharp
[System.Serializable]
public class CarryItem
{
    public CarryItemType type;
    public CropData cropData;
}
```

## 26. 상호작용 시스템 상세 규칙

### 대상 판정 우선순위
1. 플레이어와 가장 가까운 오브젝트
2. 시야 방향 앞에 있는 오브젝트
3. 현재 소지 아이템과 맞는 오브젝트

### 왜 필요한가
- 같은 키 `E` 하나로 모든 행동을 처리해야 하기 때문이다.
- 심사용 빌드에서는 입력 복잡도보다 판정 안정성이 더 중요하다.

### 상호작용 규칙 표

| 현재 상태 | 상호작용 대상 | 결과 |
| --- | --- | --- |
| 빈손 | Untilled Tile | 갈기 |
| 씨앗 소지 | Tilled Tile | 파종 |
| 물 보유 | Growing Tile | 물 주기 |
| 빈손 | Ready Tile | 수확 |
| 빈손 | Seed Shop | 씨앗 구매 |
| 어떤 상태든 | Water Pump | 물 보충 |
| 작물 소지 | Selling Crate | 판매 |

### 권장 예외 처리
- 작물 소지 중에는 씨앗 구매 불가
- 씨앗 소지 중에는 다른 씨앗 구매 불가
- Tilled 상태 타일에는 다시 갈기 불가
- Growing 상태에서는 수확 불가

## 27. FarmTile 상태머신 구현 메모

### 상태 흐름
`Untilled -> Tilled -> Growing -> Ready -> Untilled`

### 상태별 처리

| 상태 | 진입 조건 | 유지 조건 | 종료 조건 |
| --- | --- | --- | --- |
| Untilled | 초기 상태, 수확 직후 | 씨앗 없음 | 갈기 시 Tilled |
| Tilled | 갈기 완료 | 씨앗 대기 | 파종 시 Growing |
| Growing | 씨앗 심김 | 성장치 누적 | 성장치 100 도달 시 Ready |
| Ready | 성장 완료 | 수확 대기 | 수확 시 Untilled |

### 시각 피드백 규칙
- `Untilled`: 밝은 갈색 흙
- `Tilled`: 진한 갈색 + 경작 테두리
- `Growing`: 작물 스프라이트 + 성장 바
- `Ready`: 반짝임 + 상호작용 아이콘

## 28. 제작 순서 기준 작업 티켓 분해

### Phase 1. 이동과 상호작용 뼈대
- 플레이어 이동
- `E` 상호작용
- 상호작용 가능한 오브젝트 하이라이트
- Held Item 아이콘 표시

### Phase 2. 밭 루프 구현
- FarmTile 상태머신
- 갈기, 파종, 성장, 수확
- CropData 2종 생성
- 성장 바와 스프라이트 단계 연결

### Phase 3. 경제와 경기화
- Score/Coin 분리
- Seed Shop 구매 처리
- Selling Crate 판매 처리
- Match Timer와 Goal 판정

### Phase 4. 영상용 polish
- 점수 팝업
- Ready 반짝임
- 상호작용 프롬프트
- 결과 패널
- 카메라 미세 추적

## 29. 개발 우선 검증 체크리스트

| 체크 항목 | 통과 기준 |
| --- | --- |
| 씨앗 구매 | 1회 입력으로 코인 차감과 아이템 지급이 동시에 보인다 |
| 갈기 | 타일 외형이 확실히 달라진다 |
| 파종 | 씨앗이 소모되고 새싹이 생성된다 |
| 성장 | 5초 내 최소 1회 이상 시각 단계가 변한다 |
| 물 주기 | 물 사용 즉시 성장 바가 오른다 |
| 수확 | 작물이 손에 들린 상태로 명확히 보인다 |
| 판매 | 점수 팝업과 상단 점수가 동시에 갱신된다 |
| 승패 | 45초 종료 시 결과 패널이 자동 표시된다 |

## 30. 심사용 빌드 연출 체크리스트
- 16:9 해상도 기준으로 UI가 가장자리에서 잘리지 않게 한다.
- HUD 숫자는 작은 장식 폰트보다 굵고 읽기 쉬운 폰트를 쓴다.
- 플레이어와 상호작용 오브젝트는 색을 명확히 분리한다.
- 성장 바는 녹색, Ready 상태는 노란색 또는 금색으로 통일한다.
- 배경 디테일은 낮추고 오브젝트 콘트라스트를 높인다.
- 사운드는 선택 사항이지만 `구매`, `물 주기`, `수확`, `판매` 4개 효과음은 있으면 좋다.
- 30초 시연 녹화 전용 빌드에서는 시작 자원을 `Coin 3`으로 고정한다.
- 데모 촬영 시 플레이어 시작 위치는 `Seed Shop` 앞에 둔다.

## 31. 다음 단계 추천
1. `CropData` ScriptableObject 2종부터 만든다.
2. `FarmTile` 단일 프리팹 하나로 상태 변화가 완성되는지 먼저 검증한다.
3. 그 다음 `Seed Shop`, `Water Pump`, `Selling Crate`를 연결한다.
4. 마지막에 HUD와 결과 패널을 얹고 45초 루프를 촬영한다.

## 32. 현재 구현된 스크립트 연결 체크리스트

### 생성된 스크립트 경로
- `Assets/Scripts/Data/CropData.cs`
- `Assets/Scripts/Runtime/PlayerController.cs`
- `Assets/Scripts/Runtime/PlayerInteractor.cs`
- `Assets/Scripts/Runtime/PlayerCarry.cs`
- `Assets/Scripts/Runtime/PlayerWater.cs`
- `Assets/Scripts/Runtime/FarmTile.cs`
- `Assets/Scripts/Runtime/SeedShopInteractable.cs`
- `Assets/Scripts/Runtime/WaterPumpInteractable.cs`
- `Assets/Scripts/Runtime/SellStationInteractable.cs`
- `Assets/Scripts/Runtime/ScoreManager.cs`
- `Assets/Scripts/Runtime/MatchManager.cs`
- `Assets/Scripts/UI/HUDController.cs`
- `Assets/Scripts/UI/WorldPromptUI.cs`
- `Assets/Scripts/UI/FloatingText.cs`
- `Assets/Scripts/UI/ResultPanelController.cs`

### Scene 최소 연결 순서
1. 빈 오브젝트 `GameRoot`를 만들고 `ScoreManager`, `MatchManager`를 붙인다.
2. 플레이어 오브젝트에 `Rigidbody2D`, `Collider2D`, `PlayerCarry`, `PlayerWater`, `PlayerController`, `PlayerInteractor`를 붙인다.
3. 밭 타일 프리팹 하나에 `SpriteRenderer`, `Collider2D`, `FarmTile`을 붙이고 6개 복제한다.
4. 상점, 펌프, 판매대 오브젝트에 각각 `Collider2D`와 대응 Interactable 스크립트를 붙인다.
5. Canvas에 HUD 텍스트와 아이콘을 만들고 `HUDController`를 붙인다.
6. Canvas에 월드 프롬프트용 UI를 만들고 `WorldPromptUI`를 붙인 뒤 `PlayerInteractor`에 연결한다.
7. 결과 패널을 만들고 `CanvasGroup`, `ResultPanelController`를 붙인다.
8. `CropData` 두 개를 만들어 Tomato / Pumpkin 수치를 넣고 Seed Shop의 `stock` 배열에 연결한다.

### 오브젝트별 Inspector 연결

| 오브젝트 | 필수 연결 |
| --- | --- |
| GameRoot | `ScoreManager`, `MatchManager` |
| Player | `Rigidbody2D`는 Dynamic, Gravity Scale `0`, Rotation Z 고정 |
| PlayerInteractor | `worldPromptUI`, `playerCarry`, `playerWater`, `playerController` |
| FarmTile | `soilRenderer`, `cropRenderer`, `growthFillImage`, `growthBarRoot`, `readyIndicator` |
| SeedShopInteractable | `stock[0]=Tomato`, `stock[1]=Pumpkin`, `previewRenderer` |
| WaterPumpInteractable | 필요 시 `promptAnchor`만 연결 |
| SellStationInteractable | `floatingTextPrefab`, `popupSpawnPoint` |
| HUDController | `timerText`, `scoreText`, `coinText`, `goalText`, `waterText`, `heldItemIcon` |
| WorldPromptUI | `root`, `promptText`, `targetCamera` |
| ResultPanelController | `canvasGroup`, `resultText` |

### 첫 실행 전 권장값
- `MatchManager.matchDuration = 45`
- `MatchManager.startingCoin = 3`
- `MatchManager.goalScore = 300`
- `PlayerController.moveSpeed = 4.5`
- `PlayerWater.maxCharge = 2`
- `SeedShopInteractable.autoAdvanceSelection = true`
- `ScoreManager.initializeOnStart = false`

### 빠른 플레이 테스트 순서
1. Tomato 구매
2. 타일 갈기
3. 파종
4. 물 채우기 후 1회 물 주기
5. 성장 바 상승 확인
6. 수확
7. 판매
8. Score / Coin 증가 확인

## 33. Unity Editor에서 수행해야 하는 일들

아래 순서는 `MainGame` 씬을 기준으로, 현재 추가된 스크립트를 실제 플레이 가능한 회색상자 프로토타입으로 연결하는 작업 절차다.

### 33-1. 프로젝트 열기와 초기 확인
1. Unity Hub에서 이 프로젝트를 연다.
2. 에디터가 컴파일을 마칠 때까지 기다린다.
3. Console 창을 열고 빨간 에러가 없는지 먼저 확인한다.
4. `Assets/Scenes/MainGame.unity`를 연다.
5. `Main Camera`가 있는지 확인하고, 없으면 새로 생성한다.

### 33-2. 폴더 정리
1. Project 창에서 `Assets` 아래에 다음 폴더를 만든다.
2. `Assets/Art`
3. `Assets/Art/Sprites`
4. `Assets/Prefabs`
5. `Assets/Prefabs/Tiles`
6. `Assets/Prefabs/Stations`
7. `Assets/Prefabs/UI`
8. `Assets/ScriptableObjects`
9. `Assets/ScriptableObjects/Crops`

이 단계는 필수는 아니지만, 이후 Inspector 연결과 프리팹 재사용 속도를 크게 높여준다.

### 33-3. 임시 스프라이트 준비
아트가 아직 없으면 Unity 기본 사각형 스프라이트로 충분하다.

1. 빈 `SpriteRenderer` 오브젝트를 만들 때 기본 `Sprite`를 `Square`로 설정한다.
2. 다음 색 구분만 먼저 맞춘다.
3. 플레이어: 파란색
4. Seed Shop: 초록색
5. Water Pump: 하늘색
6. Selling Crate: 주황색
7. Farm Tile Untilled: 연갈색
8. Farm Tile Tilled: 진갈색
9. Ready Indicator: 노란색

### 33-4. CropData 두 개 만들기
1. Project 창에서 `Assets/ScriptableObjects/Crops` 폴더를 연다.
2. 우클릭 후 `Create > HarvestLeague > Crop Data`를 선택한다.
3. 첫 번째 에셋 이름을 `Crop_Tomato`로 바꾼다.
4. 두 번째 에셋 이름을 `Crop_Pumpkin`으로 바꾼다.

#### Tomato 권장값
- `cropId = tomato`
- `displayName = Tomato`
- `seedCost = 1`
- `sellScore = 100`
- `sellCoin = 1`
- `maxGrowth = 100`
- `sunlightRate = 10`
- `waterBonus = 35`

#### Pumpkin 권장값
- `cropId = pumpkin`
- `displayName = Pumpkin`
- `seedCost = 2`
- `sellScore = 200`
- `sellCoin = 2`
- `maxGrowth = 100`
- `sunlightRate = 8`
- `waterBonus = 30`

#### 스프라이트 연결
아트가 없으면 같은 사각형 스프라이트를 연결하고 색만 다르게 써도 된다.
- `seedSprite`: 작은 점 또는 작은 사각형
- `sproutSprite`: 초록색 작은 새싹
- `midSprite`: 더 큰 초록색
- `matureSprite`: 열매가 보이는 중간 크기
- `readySprite`: 가장 크고 밝은 색

### 33-5. GameRoot 만들기
1. Hierarchy에서 우클릭 후 `Create Empty`를 누른다.
2. 이름을 `GameRoot`로 바꾼다.
3. `GameRoot`에 `ScoreManager`를 추가한다.
4. `GameRoot`에 `MatchManager`를 추가한다.

#### MatchManager 값
- `matchDuration = 45`
- `startingCoin = 3`
- `goalScore = 300`
- `autoStart = true`

#### ScoreManager 값
- `initializeOnStart = false`

`ScoreManager`는 수치를 보관하고, 실제 초기화는 `MatchManager`가 담당하게 두는 구성이 가장 안전하다.

### 33-6. Main Camera 설정
1. `Main Camera`를 선택한다.
2. `Projection`이 `Orthographic`인지 확인한다.
3. `Size`를 `5` 또는 `6`으로 맞춘다.
4. Position을 대략 `X 0 / Y 0 / Z -10`으로 둔다.
5. 카메라가 밭 6칸, 상점, 펌프, 판매대를 한 화면에 담는지 확인한다.

### 33-7. Player 만들기
1. Hierarchy에서 `2D Object > Sprites > Square`를 만든다.
2. 이름을 `Player`로 바꾼다.
3. `Transform Scale`을 대략 `X 0.6 / Y 0.6 / Z 1`로 맞춘다.
4. `SpriteRenderer` 색을 파란색으로 바꾼다.
5. `Rigidbody2D`를 추가한다.
6. `Gravity Scale = 0`으로 설정한다.
7. `Freeze Rotation Z`를 켠다.
8. `BoxCollider2D`를 추가한다.
9. `PlayerCarry`, `PlayerWater`, `PlayerController`, `PlayerInteractor`를 추가한다.

#### Player 자식 오브젝트 만들기
1. `Player` 아래에 빈 자식 오브젝트 `HeldItem`을 만든다.
2. `HeldItem`에 `SpriteRenderer`를 붙인다.
3. `HeldItem` 위치를 `Y 0.6` 정도로 올린다.
4. Sorting Order를 플레이어보다 높게 준다.
5. `PlayerCarry.heldItemRenderer`에 이 `SpriteRenderer`를 연결한다.

#### Player 기본값
- `PlayerController.moveSpeed = 4.5`
- `PlayerController.carrySpeedMultiplier = 0.9`
- `PlayerWater.maxCharge = 2`
- `PlayerWater.startingCharge = 0`
- `PlayerInteractor.interactionRadius = 1.2`

### 33-8. FarmTile 프리팹 만들기
1. Hierarchy에서 `2D Object > Sprites > Square`를 만든다.
2. 이름을 `FarmTile`로 바꾼다.
3. Scale을 `X 1 / Y 1 / Z 1`로 둔다.
4. `SpriteRenderer` 색을 연갈색으로 바꾼다.
5. `BoxCollider2D`를 추가한다.
6. `FarmTile` 스크립트를 추가한다.

#### FarmTile 자식 오브젝트
1. `CropVisual` 자식을 만든다.
2. `CropVisual`에 `SpriteRenderer`를 추가한다.
3. Sorting Order를 타일보다 높게 둔다.
4. 위치를 타일 중앙보다 약간 위로 올린다.

#### Growth Bar 만들기
1. `FarmTile` 아래에 `Canvas`를 하나 만든다.
2. `Render Mode`는 `World Space`로 둔다.
3. Scale을 작게 줄인다. 예: `0.01, 0.01, 0.01`
4. 이름을 `GrowthBarRoot`로 바꾼다.
5. `GrowthBarRoot` 아래에 배경용 `Image`를 하나 만든다. 이름은 `BarBackground`로 둔다.
6. `BarBackground`의 크기를 가로로 긴 막대처럼 만든다. 예: `Width 80 / Height 12`
7. `BarBackground` 아래에 Fill용 `Image`를 하나 더 만든다. 이름은 `BarFill`로 둔다.
8. `BarFill`도 `BarBackground`와 같은 위치와 크기로 맞춘다.
9. `BarFill`을 선택한 뒤 Inspector의 `Image` 컴포넌트를 본다.
10. 만약 `Source Image`가 `None (Sprite)`라면, 먼저 여기에 아무 `Sprite` 하나를 넣는다.
11. `Source Image` 오른쪽의 작은 원 버튼을 눌러 스프라이트를 고른다.
12. 프로젝트에 쓸 스프라이트가 없다면, 임시로 흰색 사각형 스프라이트 하나를 프로젝트에 넣어서 사용한다.
13. `Source Image`가 들어가면 그 아래에 `Type` 항목이 나타난다.
14. `Type`을 `Simple`에서 `Filled`로 바꾼다.
15. 바로 아래에 새로 나타나는 `Fill Method`를 `Horizontal`로 바꾼다.
16. `Fill Origin`은 왼쪽 시작으로 둔다.
17. `Fill Amount`는 Inspector에서 일단 `1`로 둔다. 실제 플레이 중에는 스크립트가 이 값을 자동으로 바꾼다.
18. `BarFill` 색을 녹색으로 둔다.

#### `Image Type`이 무엇인지
- Unity UI의 `Image` 컴포넌트에는 이미지를 어떻게 그릴지 정하는 `Type` 항목이 있다.
- 기본값 `Simple`은 이미지를 그냥 통째로 보여주는 방식이다.
- `Filled`는 이미지가 왼쪽에서 오른쪽, 아래에서 위처럼 일정 방향으로 차오르게 보여주는 방식이다.
- 지금 성장 바는 코드에서 `growthFillImage.fillAmount` 값을 바꾸므로, `BarFill`의 `Type`이 반드시 `Filled`여야 한다.
- 즉, `Type = Filled`를 켜야 성장 게이지가 `0% -> 100%`로 차오르는 막대처럼 보인다.
- 단, `Source Image`가 비어 있으면 Unity가 `Type` 항목 자체를 숨길 수 있다.

#### 어디서 바꾸는지
1. Hierarchy에서 `BarFill` 오브젝트를 클릭한다.
2. Inspector에서 `Image` 컴포넌트를 찾는다.
3. `Source Image`가 `None`이면 먼저 스프라이트를 하나 넣는다.
4. 그러면 `Source Image` 아래쪽에 `Type` 드롭다운이 나타난다.
5. `Simple`을 `Filled`로 바꾼다.
6. `Fill Method`를 `Horizontal`로 바꾼다.

#### 잘 안 보이면 확인할 것
- `BarFill` 오브젝트에 `Image` 컴포넌트가 붙어 있는지 확인한다.
- `Source Image`가 `None`이면 `Type`이 안 보일 수 있으니 먼저 스프라이트를 넣는다.
- `BarFill`의 색 알파값이 0이 아닌지 확인한다.
- `growthFillImage` 필드에 `BarFill`의 `Image` 컴포넌트를 연결했는지 확인한다.
- `GrowthBarRoot`가 꺼져 있지 않은지 확인한다.

#### Ready Indicator 만들기
1. `FarmTile` 아래에 `ReadyIndicator` 자식을 만든다.
2. `SpriteRenderer`를 붙이고 노란색 또는 금색으로 둔다.
3. 위치를 타일 위쪽에 둔다.
4. Scale을 작게 둔다.

#### FarmTile Inspector 연결
- `soilRenderer` -> 타일 본체 SpriteRenderer
- `cropRenderer` -> `CropVisual` SpriteRenderer
- `growthFillImage` -> Fill Image
- `growthBarRoot` -> `GrowthBarRoot`
- `readyIndicator` -> `ReadyIndicator`

#### Prefab 저장
1. `FarmTile`을 `Assets/Prefabs/Tiles`로 드래그해서 프리팹으로 만든다.
2. Hierarchy의 원본은 지워도 된다.

### 33-9. 밭 6칸 배치
1. `FarmTile` 프리팹을 Hierarchy에 6번 배치한다.
2. 이름을 `Tile_01`부터 `Tile_06`으로 바꾼다.
3. 2 x 3 배열로 둔다.

#### 권장 좌표 예시
- 1행: `(-1.2, 0.6)`, `(0, 0.6)`, `(1.2, 0.6)`
- 2행: `(-1.2, -0.6)`, `(0, -0.6)`, `(1.2, -0.6)`

### 33-10. Seed Shop 만들기
1. `2D Object > Sprites > Square`를 만든다.
2. 이름을 `SeedShop`으로 바꾼다.
3. 위치를 밭 왼쪽으로 둔다. 예: `X -4 / Y 0`
4. Scale을 `1.2, 1.2, 1` 정도로 둔다.
5. `SpriteRenderer` 색을 초록색으로 둔다.
6. `BoxCollider2D`를 추가한다.
7. `SeedShopInteractable`을 추가한다.

##### `BoxCollider2D`의 `Is Trigger`는 켜야 하나
- 필수는 아니다.
- 현재 상호작용 코드는 `PlayerInteractor`에서 `Physics2D.OverlapCircleAll`로 주변 Collider를 찾기 때문에, `Is Trigger`가 꺼져 있어도 상호작용 대상은 잡힌다.
- 즉, `SeedShop`은 `Is Trigger`가 꺼져 있어도 구매가 가능하다.
- 다만 상점을 통과 가능한 오브젝트로 만들고 싶으면 `Is Trigger`를 켜고,
- 상점을 벽처럼 막는 오브젝트로 쓰고 싶으면 `Is Trigger`를 끄면 된다.
- 심사용 회색상자 프로토타입에서는 플레이어 동선이 덜 막히도록 `Is Trigger = On`을 권장한다.

#### Seed Shop 자식 오브젝트
`SeedShop`에는 자식 오브젝트 2개를 두는 것을 권장한다. 하나는 상호작용 문구가 뜰 기준 위치이고, 하나는 현재 판매 중인 씨앗을 시각적으로 보여주는 표시용 오브젝트다.

##### `PromptAnchor` 만드는 법
1. `SeedShop` 오브젝트를 선택한다.
2. Hierarchy에서 우클릭 후 `Create Empty`를 누른다.
3. 이름을 `PromptAnchor`로 바꾼다.
4. `PromptAnchor`가 `SeedShop`의 자식인지 확인한다.
5. `Transform`의 Position을 부모 기준으로 위쪽에 올린다. 예: `X 0 / Y 0.9 / Z 0`
6. 이 오브젝트에는 컴포넌트를 따로 붙이지 않아도 된다.

##### `PromptAnchor`가 필요한 이유
- 플레이어가 상점 가까이 갔을 때 `Tomato 씨앗 구매 (1)` 같은 문구를 어디에 띄울지 정하는 기준점이다.
- 이 오브젝트가 없으면 프롬프트가 상점 중심이나 어색한 위치에 뜰 수 있다.
- 따라서 상점 머리 위쯤에 두는 것이 가장 읽기 쉽다.

##### `Preview` 만드는 법
1. `SeedShop` 오브젝트를 선택한 상태에서 우클릭 후 `Create Empty`를 누른다.
2. 이름을 `Preview`로 바꾼다.
3. `Preview`가 `SeedShop`의 자식인지 확인한다.
4. `Preview`에 `SpriteRenderer`를 추가한다.
5. 위치를 상점 중앙 또는 약간 위쪽에 둔다. 예: `X 0 / Y 0.2 / Z 0`
6. Scale은 작게 둔다. 예: `X 0.4 / Y 0.4 / Z 1`
7. `Sorting Order`를 상점 본체보다 높게 둔다. 예: 상점이 `0`이면 `Preview`는 `1`

##### `Preview`가 필요한 이유
- 현재 상점에서 어떤 씨앗을 살 수 있는지 시각적으로 보여주는 역할이다.
- 지금 스크립트는 씨앗을 한 번 살 때마다 `Tomato -> Pumpkin -> Tomato` 식으로 자동 전환할 수 있다.
- 그래서 `Preview`를 두면 현재 선택된 씨앗이 무엇인지 자막 없이도 바로 보인다.
- 이 오브젝트의 스프라이트는 `SeedShopInteractable`이 자동으로 바꿔준다.

#### Seed Shop Inspector 연결
`SeedShop` 오브젝트를 클릭한 뒤 Inspector에서 `SeedShopInteractable` 컴포넌트를 설정한다.

##### 1. `stock` 배열 연결
1. `SeedShopInteractable`의 `stock` 항목을 찾는다.
2. `Size`를 `2`로 바꾼다.
3. `Element 0`에 `Crop_Tomato`를 드래그한다.
4. `Element 1`에 `Crop_Pumpkin`를 드래그한다.

이 배열은 "이 상점이 판매할 수 있는 작물 목록"이다.
- `stock[0] = Crop_Tomato`
- `stock[1] = Crop_Pumpkin`

##### 2. `promptAnchor` 연결
1. Hierarchy에서 `SeedShop > PromptAnchor`를 찾는다.
2. 이것을 Inspector의 `promptAnchor` 칸에 드래그한다.

이 연결이 있어야 상점 프롬프트가 상점 위쪽에 뜬다.

##### 3. `previewRenderer` 연결
1. Hierarchy에서 `SeedShop > Preview`를 연다.
2. `Preview` 오브젝트의 `SpriteRenderer`를 확인한다.
3. `Preview` 오브젝트 자체를 `previewRenderer` 칸에 드래그한다.

Unity가 자동으로 그 오브젝트의 `SpriteRenderer` 컴포넌트를 참조한다.

이 연결이 있어야 현재 선택된 씨앗의 아이콘이 상점 위에 보인다.

##### 4. `autoAdvanceSelection` 설정
- `autoAdvanceSelection = true`

이 옵션이 켜져 있으면:
1. 처음에는 `stock[0]`인 Tomato가 보인다.
2. Tomato를 구매하면 다음에는 Pumpkin이 보인다.
3. Pumpkin을 구매하면 다시 Tomato로 돌아간다.

즉, 버튼 하나만 눌러도 두 종류 작물이 번갈아 보이게 되므로, 짧은 심사 영상에서 2종 작물을 모두 보여주기 쉽다.

##### 5. 선택 인덱스 이해
- `selectedIndex = 0`이면 처음 시작 씨앗은 Tomato다.
- `selectedIndex = 1`이면 처음 시작 씨앗은 Pumpkin이다.

일반적으로는 `0`으로 두는 것을 권장한다.

##### 6. 연결 완료 후 화면에서 확인할 것
1. 상점 본체 위나 중앙에 작은 씨앗 스프라이트가 보이는지 확인한다.
2. 플레이어가 가까이 갔을 때 상점 위쪽에 구매 프롬프트가 뜨는지 확인한다.
3. 씨앗을 1회 구매한 뒤, `Preview` 스프라이트가 다음 작물로 바뀌는지 확인한다.
4. 코인이 부족하면 `코인 부족` 문구가 뜨는지 확인한다.

##### 연결이 안 될 때 확인할 것
- `stock` 배열 Size가 `0`이 아닌지 확인한다.
- `Crop_Tomato`, `Crop_Pumpkin` 에셋이 실제로 존재하는지 확인한다.
- `Crop_Tomato.seedSprite` 또는 `Crop_Pumpkin.seedSprite`가 비어 있지 않은지 확인한다.
- `Preview`에 `SpriteRenderer`가 붙어 있는지 확인한다.
- `previewRenderer` 필드가 비어 있지 않은지 확인한다.
- `PromptAnchor`가 `SeedShop` 자식인지 확인한다.
- `Preview`의 Sorting Order가 너무 낮아서 상점 뒤에 가려지지 않는지 확인한다.

##### 플레이 시작 후 `Preview`가 사라지면
- 가장 먼저 `CropData`의 `seedSprite`가 비어 있는지 확인한다.
- 현재 스크립트는 `seedSprite`가 없으면 `readySprite`를 대신 보여주도록 되어 있다.
- 둘 다 비어 있으면 `Preview`는 보이지 않는다.
- 따라서 최소한 `seedSprite` 또는 `readySprite` 중 하나는 반드시 넣어야 한다.

### 33-11. Water Pump 만들기
1. `2D Object > Sprites > Square`를 만든다.
2. 이름을 `WaterPump`로 바꾼다.
3. 위치를 밭 위쪽으로 둔다. 예: `X 0 / Y 2.2`
4. 색을 하늘색으로 둔다.
5. `BoxCollider2D`를 추가한다.
6. `WaterPumpInteractable`을 추가한다.
7. `PromptAnchor` 빈 오브젝트를 자식으로 만들고 위쪽에 둔다.
8. `promptAnchor` 필드에 연결한다.

### 33-12. Selling Crate 만들기
1. `2D Object > Sprites > Square`를 만든다.
2. 이름을 `SellingCrate`로 바꾼다.
3. 위치를 밭 오른쪽으로 둔다. 예: `X 4 / Y 0`
4. 색을 주황색으로 둔다.
5. `BoxCollider2D`를 추가한다.
6. `SellStationInteractable`을 추가한다.
7. `PromptAnchor`와 `PopupSpawnPoint` 자식을 만든다.
8. 두 오브젝트 모두 본체보다 약간 위에 둔다.

### 33-13. FloatingText 프리팹 만들기
1. Hierarchy에서 `Canvas`를 하나 임시로 만든다.
2. 그 아래에 `Text - TextMeshPro`를 만든다.
3. 이름을 `FloatingText`로 바꾼다.
4. `FloatingText` 스크립트를 붙인다.
5. 폰트 크기를 크게 하고 중앙 정렬로 둔다.
6. 이 오브젝트를 `Assets/Prefabs/UI`로 드래그해 프리팹으로 만든다.
7. 임시 Canvas는 삭제한다.
8. `SellingCrate`의 `floatingTextPrefab`에 이 프리팹을 연결한다.
9. `popupSpawnPoint`에 `PopupSpawnPoint`를 연결한다.

### 33-14. HUD Canvas 만들기
1. Hierarchy에서 `UI > Canvas`를 만든다.
2. 이름을 `UI`로 바꾼다.
3. 그 아래에 `HUD` 빈 오브젝트를 만든다.
4. `HUDController`를 `HUD`에 붙인다.

#### HUD 내부 요소
1. 좌상단 `ScoreText`
2. 좌상단 아래 `CoinText`
3. 중앙 상단 `TimerText`
4. 우상단 `GoalText`
5. 우하단 `WaterText`
6. 하단 중앙 `HeldItemRoot`
7. `HeldItemRoot` 아래 `HeldItemIcon` Image

#### HUD 연결
- `timerText` -> `TimerText`
- `scoreText` -> `ScoreText`
- `coinText` -> `CoinText`
- `goalText` -> `GoalText`
- `waterText` -> `WaterText`
- `heldItemRoot` -> `HeldItemRoot`
- `heldItemIcon` -> `HeldItemIcon`
- `playerCarry` -> Player의 `PlayerCarry`
- `playerWater` -> Player의 `PlayerWater`

### 33-15. World Prompt UI 만들기
1. `UI` 아래에 빈 오브젝트 `WorldPrompt`를 만든다.
2. `RectTransform` 기준 중앙 정렬로 둔다.
3. 그 아래에 `PromptText`를 만든다.
4. `WorldPromptUI`를 `WorldPrompt`에 붙인다.
5. `root`에 `WorldPrompt`의 RectTransform을 연결한다.
6. `promptText`에 `PromptText`를 연결한다.
7. `targetCamera`에 `Main Camera`를 연결한다.
8. Player의 `PlayerInteractor.worldPromptUI`에 이 `WorldPromptUI`를 연결한다.

### 33-16. Result Panel 만들기
`ResultPanel`은 아래처럼 부모-자식 관계를 만드는 것을 권장한다.

```text
UI
├─ HUD
├─ WorldPrompt
└─ ResultPanel
   ├─ Background
   └─ ResultText
```

핵심은 이렇다.
- `ResultPanel`은 전체 결과 UI를 묶는 부모 오브젝트다.
- `Background`는 반투명 검은 배경 역할을 하는 자식 오브젝트다.
- `ResultText`는 `VICTORY`, `DEFEAT`를 보여주는 자식 오브젝트다.
- `CanvasGroup`과 `ResultPanelController`는 부모인 `ResultPanel`에 붙인다.

#### 1. 부모 오브젝트 `ResultPanel` 만들기
1. Hierarchy에서 `UI` 오브젝트를 선택한다.
2. 우클릭 후 `Create Empty`를 누른다.
3. 이름을 `ResultPanel`로 바꾼다.
4. `ResultPanel`이 반드시 `UI`의 자식인지 확인한다.
5. `RectTransform`을 화면 전체를 덮게 만든다.

#### `ResultPanel` RectTransform 권장 설정
- Anchor Min = `(0, 0)`
- Anchor Max = `(1, 1)`
- Left / Right / Top / Bottom = `0`

이렇게 하면 해상도가 바뀌어도 결과 패널이 화면 전체를 덮는다.

#### 2. `ResultPanel`에 붙일 컴포넌트
1. `ResultPanel`에 `CanvasGroup`을 붙인다.
2. `ResultPanel`에 `ResultPanelController`를 붙인다.

여기서 중요한 점:
- `CanvasGroup`은 `Background`가 아니라 부모 `ResultPanel`에 붙인다.
- `ResultPanelController`도 부모 `ResultPanel`에 붙인다.
- 즉, 결과창의 표시/숨김 제어는 부모가 담당한다.

#### 3. 자식 오브젝트 `Background` 만들기
1. `ResultPanel`을 선택한 상태에서 우클릭한다.
2. `UI > Image`를 만든다.
3. 이름을 `Background`로 바꾼다.
4. `Background`가 `ResultPanel`의 자식인지 확인한다.
5. `RectTransform`을 화면 전체로 늘린다.
6. 색을 검은색 계열로 두고 알파값을 낮춘다. 예: `A 120~180`

#### `Background` 역할
- 게임 화면을 반투명하게 덮는다.
- 결과 텍스트가 더 잘 읽히게 만든다.
- 컨트롤러는 없고, 그냥 시각용 자식 오브젝트다.

#### 4. 자식 오브젝트 `ResultText` 만들기
1. `ResultPanel`을 선택한 상태에서 우클릭한다.
2. `UI > Text - TextMeshPro`를 만든다.
3. 이름을 `ResultText`로 바꾼다.
4. `ResultText`가 `ResultPanel`의 자식인지 확인한다.
5. 화면 중앙으로 배치한다.
6. Alignment를 가운데 정렬로 둔다.
7. 폰트 크기를 크게 둔다. 예: `60 ~ 90`
8. 기본 텍스트는 임시로 `VICTORY` 또는 `RESULT`로 넣어도 된다.

#### `ResultText` 권장 설정
- Anchor는 중앙
- Pivot은 중앙
- Width는 넓게
- Height는 120 이상
- 색은 흰색 또는 밝은 노란색

#### 5. Inspector 연결
1. 부모 `ResultPanel`을 선택한다.
2. `ResultPanelController` 컴포넌트를 본다.
3. `canvasGroup` 칸에 부모 `ResultPanel`의 `CanvasGroup`을 넣는다.
4. `resultText` 칸에 자식 `ResultText`의 `TextMeshProUGUI` 컴포넌트를 넣는다.

즉 연결 구조는 아래와 같다.
- `ResultPanelController.canvasGroup` -> 부모 `ResultPanel`의 `CanvasGroup`
- `ResultPanelController.resultText` -> 자식 `ResultText`의 `TextMeshProUGUI`

#### 6. 잘못 연결하기 쉬운 부분
- `Background`에 `CanvasGroup`을 붙이지 않는다.
- `Background`에 `ResultPanelController`를 붙이지 않는다.
- `ResultText`에 `ResultPanelController`를 붙이지 않는다.
- 컨트롤러와 `CanvasGroup`은 둘 다 부모 `ResultPanel`에 붙인다.

#### 7. 완료 후 확인할 것
1. Hierarchy가 `UI > ResultPanel > Background / ResultText` 구조인지 확인한다.
2. Play 전에는 화면에 보이지 않아도 정상이다.
3. 45초가 끝나면 반투명 배경과 결과 텍스트가 함께 뜨면 정상이다.

### 33-17. Inspector 최종 연결 점검
아래 항목은 Play를 누르기 전에 반드시 한 번 더 확인한다.

#### GameRoot
- `ScoreManager` 존재
- `MatchManager` 존재

#### Player
- `Rigidbody2D` Gravity Scale `0`
- `Rigidbody2D` Rotation Z 고정
- `PlayerCarry.heldItemRenderer` 연결
- `PlayerInteractor.playerCarry` 연결
- `PlayerInteractor.playerWater` 연결
- `PlayerInteractor.playerController` 연결
- `PlayerInteractor.worldPromptUI` 연결

#### 각 FarmTile
- `soilRenderer` 연결
- `cropRenderer` 연결
- `growthFillImage` 연결
- `growthBarRoot` 연결
- `readyIndicator` 연결

#### SeedShop
- `stock` 배열 2개 연결
- `previewRenderer` 연결
- `promptAnchor` 연결

#### WaterPump
- `promptAnchor` 연결

#### SellingCrate
- `floatingTextPrefab` 연결
- `popupSpawnPoint` 연결
- `promptAnchor` 연결

#### HUD
- 텍스트 5개 연결
- Held Item UI 연결
- PlayerCarry / PlayerWater 연결

#### ResultPanel
- `canvasGroup` 연결
- `resultText` 연결

### 33-18. 첫 Play 테스트
1. Play 버튼을 누른다.
2. 상단 UI에 `Timer`, `Score`, `Coin`, `Goal`이 보이는지 확인한다.
3. 플레이어를 `SeedShop` 앞에 이동시킨다.
4. `E`를 눌렀을 때 씨앗 구매 프롬프트가 보이는지 확인한다.
5. 씨앗 구매 후 Held Item 아이콘이 바뀌는지 확인한다.
6. 빈 타일에서는 바로 갈기만 되는지 확인한다.
7. 갈린 타일에서는 파종이 되는지 확인한다.
8. `WaterPump`에서 물을 채운 뒤 `Growing` 타일에 물을 주면 성장 바가 즉시 상승하는지 확인한다.
9. 시간이 지나면 작물 스프라이트가 단계적으로 바뀌는지 확인한다.
10. `Ready`가 되면 수확 가능한 아이콘이 뜨는지 확인한다.
11. 수확 후 `SellingCrate`에 가서 판매가 되는지 확인한다.
12. 판매 시 Score 팝업과 상단 Score/Coin이 함께 갱신되는지 확인한다.
13. 45초 종료 시 `VICTORY` 또는 `DEFEAT`가 표시되는지 확인한다.

### 33-19. 플레이가 안 될 때 우선 확인할 것
1. Console에 에러가 있는지 먼저 본다.
2. Collider2D가 빠져 있지 않은지 본다.
3. `PlayerInteractor.interactionRadius`가 너무 작지 않은지 본다.
4. `PlayerInteractor.worldPromptUI`가 비어 있지 않은지 본다.
5. `CropData`가 `SeedShop.stock`에 연결되어 있는지 본다.
6. `FarmTile`의 `cropRenderer`, `growthFillImage`, `readyIndicator`가 비어 있지 않은지 본다.
7. `MatchManager`와 `ScoreManager`가 씬에 하나씩만 있는지 본다.
8. HUD Text 연결이 끊어져 있지 않은지 본다.

### 33-20. 심사 영상용 마지막 정리
1. 플레이어 시작 위치를 `SeedShop` 앞에 둔다.
2. 카메라가 모든 핵심 오브젝트를 한 화면에 담는지 다시 본다.
3. 배경 장식은 줄이고 오브젝트 색 대비를 높인다.
4. `Goal = 300`, `Coin = 3`, `Match Duration = 45`가 맞는지 다시 본다.
5. 실제 녹화 전 1회는 `Tomato -> Pumpkin` 순서로 루프가 30초 안에 들어오는지 리허설한다.
