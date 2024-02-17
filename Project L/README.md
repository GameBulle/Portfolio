# Project L
백 뷰 시점의 Unity 3D 액션 RPG

 ## 💻 프로젝트 소개
 백 뷰 시점의 3D 액션 RPG 게임으로 플레이어는 몬스터를 잡아 레벨업을 하거나 NPC들의 퀘스트를 클리어 하며 성장하면서 새로운 지역을 밝혀 나가 보스 몬스터를 잡는 게임입니다

## 🕐 개발 기간
 2023.05.29 ~ 2023.11.04 (1인 개발)

## 📺 개발 환경
 * C#
 * Unity 2021.3.23f1

## 🎮 [플레이 영상](https://youtu.be/jm4Of-qw4fg?si=Mu-XHQoeK6xqdw9B)

## 📌 주요 기능
* [오브젝트와의 상호작용](https://github.com/GameBulle/Portfolio/tree/e4ed7863bff8c6a9ae7464c0464d104b4835f008/Project%20L/InteractionObject)
  - 플레이어와 상호작용 가능한 모든 오브젝트들은 [IInteractionable](https://github.com/GameBulle/Portfolio/tree/b97e50391483a3a8aa8251106ee581167b92c521/Project%20L/Interface) 인터페이스를 상속 받아 사용합니다.
  - Interaction(Player player) 함수로 플레이어와 상호작용을 합니다.
  - AddInteractionToList() 함수로 오브젝트의 상호작용 정보를 InteractionListUI에 추가하여 보여줍니다.
  - RemoveInteractionToList() 함수로 오브젝트의 상호작용 정보를 InteractionListUI에서 삭제 합니다.

* [플레이어의 동작](https://github.com/GameBulle/Portfolio/tree/efd4b7c190a7a0b01f7682f5c7843c0992fe29eb/Project%20L/Player)
- 플레이어의 이동은 가속도에 따라 애니메이션 간의 전환을 부드럽게 하기 위해 **Input Manager**로 구현했습니다.
- 플레이어의 이동을 제외한 모든 동작은 **Input System**으로 구현했습니다.

*[몬스터가 Player를 찾는 방법]()
