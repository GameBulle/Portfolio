# Monster Defence
Wave 형식의 Unity 2D Defence 게임

 ## 💻 프로젝트 소개
Wave 형식의 Defence 게임으로 각 Wave마다 몰려오는 적을 화살을 쏴서 방어선을 방어하여 특정 Wave까지 생존하는 게임

## 🕐 개발 기간
2022.12.28 ~ 2023.04.25(1인 개발)

## 📺 개발 환경
 * C#
 * Unity 2021.3.23f1
 * Visual Studio

## 🎮 [플레이 영상](https://youtu.be/95uqsvj4cJY?si=Tx1JsnOQAPK4USJT)

## 📌 주요 기능
* [오브젝트와의 상호작용](https://github.com/GameBulle/Portfolio/tree/e4ed7863bff8c6a9ae7464c0464d104b4835f008/Project%20L/InteractionObject)
  - 플레이어와 상호작용 가능한 모든 오브젝트들은 [IInteractionable](https://github.com/GameBulle/Portfolio/tree/b97e50391483a3a8aa8251106ee581167b92c521/Project%20L/Interface) 인터페이스를 상속 받아 사용합니다.

* [몬스터 설계](https://github.com/GameBulle/Portfolio/tree/efd4b7c190a7a0b01f7682f5c7843c0992fe29eb/Project%20L/Player)
  - 플레이어의 이동은 가속도에 따라 애니메이션 간의 전환을 부드럽게 하기 위해 **Input Manager**로 구현했습니다.
  - 플레이어의 이동을 제외한 모든 동작은 **Input System**으로 구현했습니다.

* [몬스터가 플레이어를 찾는 방법](https://github.com/GameBulle/Portfolio/tree/76cf2f6ca2a2eac3ab2e297b1c9cb8758df42b62/Project%20L/Monster)
   - **Physics.OverlapSphere** 함수와 **Physics.Raycast** 함수를 이용합니다.

* [아이템의 구조](https://github.com/GameBulle/Portfolio/tree/77b74f3bfe9293a1a8bc7134cc0ae5d2c898b686/Project%20L/Item)
  - ItemData는 **ScriptablObject**로 만들었습니다.
 
* [퀘스트 구조](https://github.com/GameBulle/Portfolio/tree/054b0365d7e074bbe04aa518a74d3b0f4f409740/Project%20L/Manager)
  - QuestManager는 **Singleton** 패턴과 **Observer** 패턴으로 구현했습니다.
 
* [세이브와 로드](https://github.com/GameBulle/Portfolio/tree/07eb6f5b78d449f108974489b93c03c4b5add96d/Project%20L/Option)
  - 세이브 데이터는 **플레이어 데이터**와 **옵션 데이터**가 있습니다.
  - 세이브 파일은 **Json** 파일로 관리합니다.
