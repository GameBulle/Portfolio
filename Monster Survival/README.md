# Monster Survival
탑 뷰 시점의 Unity 2D 로그라이크 게임

 ## 💻 프로젝트 소개
 탑 뷰 시점의 2D 로그라이크 게임으로 플레이어는 판마다 캐릭터를 선택 후 계속 몰려오는 몬스터를 잡아 성장하면서 최대한 오래 버텨서 얻은 골드로 캐릭터를 업그레이드하는 게임입니다.

## 🕐 개발 기간
 2024.03.17 ~ 2024.04.23 (1인 개발)

## 📺 개발 환경
 * C#
 * Unity 2021.3.23f1
 * Visual Studio

## 🎮 [플레이 영상](https://youtu.be/85Ao4Fnz07Q?si=we41TdZzw4ykdpDR)
## 🎮 [로그인 및 세이브, 로드 영상](https://youtu.be/qmDYC-1YDS4?si=C582eVutzZ3xAmLj)

## 📌 주요 기능
* 랜덤 스킬 뽑기 및 스킬 레벨업, 새로운 스킬 추가
  - [WeaponDataManager](https://github.com/GameBulle/Portfolio/tree/main/Monster%20Survival/Managers)에서 모든 스킬을 관리합니다.

* 캐릭터 해금
  - [AchievementManager](https://github.com/GameBulle/Portfolio/tree/main/Monster%20Survival/Managers)에서 캐릭터 해금 조건을 계속 체크합니다.
  - 캐릭터 해금 조건을 만족 했다면 갱신된 정보를 [CharacterManager](https://github.com/GameBulle/Portfolio/tree/main/Monster%20Survival/Managers)로 전달하여 캐릭터를 해금합니다.  [(옵저버 패턴)](https://github.com/GameBulle/Portfolio/tree/main/Monster%20Survival/Interface)
  - 해금된 캐릭터의 정보를 [InterfaceManager](https://github.com/GameBulle/Portfolio/tree/main/Monster%20Survival/Managers)를 통해 [Alarm UI](https://github.com/GameBulle/Portfolio/tree/main/Monster%20Survival/UI)로 전달하여 캐릭터가 해금된걸 플레이어에게 알립니다.

* [플레이어와 가장 가까운 몬스터 찾기 및 경험치 흡수](https://github.com/GameBulle/Portfolio/tree/main/Monster%20Survival/Player)
   - 몬스터와 경험치는 Scanner Class에서 **Physics2D.CircleCastAll** 함수로 찾습니다.
   - 경험치 흡수는 **Vector3.Lerp** 함수를 이용합니다.

* Firebase를 이용한 Login 기능과 Realtime Database 관리
   - [FirebaseManager](https://github.com/GameBulle/Portfolio/tree/main/Monster%20Survival/Managers)에서 계정 생성, 로그인, Realtime Database 읽기 및 쓰기를 관리합니다.
   - [CreateAccountUI](https://github.com/GameBulle/Portfolio/tree/main/Monster%20Survival/LoginUI)에서 계정 생성을 하기 위한 인증 코드 전송 및 확인과 비밀번호 일치 여부를 확인합니다.
