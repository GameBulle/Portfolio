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
* [몬스터 설계](https://github.com/GameBulle/Portfolio/tree/9d7dcf5c5d7855b75152de63ad86817eb01a9375/Monster%20Defence/Monster)
  - 슬라임을 제외한 모든 몬스터는 머리, 상체, 하체 각각 Collider2D를 가집니다.
  - 충돌된 Collider의 부모의 IDamageable Interface를 참고하여 OnDamage 함수 호출합니다.

* [발사 가능한 화살의 궤적](https://github.com/GameBulle/Portfolio/tree/9d7dcf5c5d7855b75152de63ad86817eb01a9375/Monster%20Defence/Player)
  - ScreenToWorldPoint 함수로 화면내 마우스 클릭 좌표를 월드 좌표로 변환합니다.
  - 발사위치와 마우스 클릭 좌표로 벡터를 구한 뒤 Vector2.right로 각도를 구합니다.

* [오브젝트 풀링](
https://github.com/GameBulle/Portfolio/tree/9d7dcf5c5d7855b75152de63ad86817eb01a9375/Monster%20Defence/PoolingObject)
  - 몬스터와 화살은 오브젝트 풀링으로 관리합니다.
 
* [NPC의 몬스터 인식 및 공격](https://github.com/GameBulle/Portfolio/tree/9d7dcf5c5d7855b75152de63ad86817eb01a9375/Monster%20Defence/NPC)
  - Physics2D.OverlapCircleAll 함수로 제일 가까운 몬스터를 탐색합니다.
  - 탐색된 몬스터가 활의 최대 각도에 벗어나면 그 몬스터의 Y축 으로 이동합니다.
  - 해당 NPC가 소지한 화살의 최소 관통력과 최대 관통력 사이의 랜덤한 관통력까지 차지 후 화살 발사합니다.
