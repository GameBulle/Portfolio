using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : Character, IFightable
{
    [Header("Function")]
    [SerializeField] PlayerAnimation anim;

    [Header("Player State")]
    [SerializeField] PlayerIdleState idleState;
    [SerializeField] PlayerRunState runState;
    [SerializeField] PlayerEvadeState evadeState;
    [SerializeField] PlayerGuardState guardState;
    [SerializeField] PlayerAttackState attackState;
    [SerializeField] PlayerDamagedState damagedState;
    [SerializeField] PlayerDieState dieState;
    [SerializeField] PlayerGuardBreakState guardBreakState;
    [SerializeField] PlayerSkillState skillState;
    [SerializeField] PlayerExecuteState executeState;
    [SerializeField] PlayerChargeState chargeState;
    [SerializeField] PlayerChargeAttackState chargeAttackState;
    [SerializeField] PlayerRushState rushState;

    [Header("Player Data")]
    [SerializeField] PlayerData data;

    [Header("Skill Pos")]
    [SerializeField] Transform skillPos;
    [SerializeField] Transform guardPos;

    [Header("Execute Pos")]
    [SerializeField] Transform executePos;

    [Header("Weapon Pos")]
    [SerializeField] Transform weaponPos;

    [SerializeField] Collider attackCollider;
    [SerializeField] Collider playerCollider;

    [SerializeField] ParticleSystem rushEffect;

    [SerializeField] GameObject minimapIcon;
    [SerializeField] Transform meshTr;
    [SerializeField] Transform mainCameraPos;
    [SerializeField] ParticleSystem levelUpEffect;

    public Vector3 SkillPos => skillPos.position;
    public Vector3 ExecutePos => executePos.position;
    float needNextLevelEXP;
    float currEXP;

    EquipableItemData[] equipItems;
    Status equipmentItemStatus;
    Status levelStatus;

    float selectInteractionTimer;
    float maxSpeed;
    float reverseWalkSpeed;
    float guardPoint;
    float playTime = 1f;

    public int Level { get; set; }
    public float SP { get; private set; }
    public float MP { get;  set; }

    float maxCameraDistance;

    FSM<Player> fsm = null;
    PlayerStateData stateData = null;
    Inventory inventory;
    QuickSlot quickSlot;
    SkillSlot skillSlot;

    public bool IsGuardSuccess { get; set; }
    public bool CheckStaminaGuard { get { return SP <= 0; } }
    bool interaction;
    bool IsLockOn;
    bool IsGuardBreak;
    bool IsGuard;
    bool runTowalk;
    bool guardToEvade;
    bool IsInteraction;
    Vector2 animVelocity;
    Vector2 inputDir;
    Coroutine coroutine = null;
    Transform LockOnTarget;
    List<Collider> lockOnTargets = new();
    int lockOnIndex;
    

    float horizontalInput;
    float verticalInput;

    public float HPMax { get; set; }
    public float HP { get; set; }
    public bool isDead { get { return HP <= 0; } }

    public enum AttackCombo { attack1 = 1, attack2, attack3, attack4 }
    public AttackCombo attactCombo { get; set; }
    public enum Act { idle, run, evade, guard, attack, damaged, die, guardBreak, skill, Execute, Charge,ChargeAttack }
    public Act act { get; set; }

    float angleY = .0f;
    float angleX = .0f;
    float sensivity = 0f;

    private void Update()
    {
        if (CheckScene())
            Destroy(gameObject);

        minimapIcon.transform.forward = new Vector3(-this.transform.forward.x, minimapIcon.transform.position.y, -this.transform.forward.z);

        if (act != Act.die && act != Act.guardBreak && !IsGuardSuccess && !IsInteraction)
        {
            if (act == Act.idle || act == Act.run || act == Act.guard && !UnityEngine.Cursor.visible)
                PlayerInput();

            if (act != Act.attack && !UnityEngine.Cursor.visible) 
                this.Move();

            if (CheckLockOn())
            {
                this.transform.LookAt(LockOnTarget);
                InterfaceMgr.Instance.LockOnImagePosUpdate(lockOnTargets[lockOnIndex].transform.position);
            }
            else
                LockOnEnd();
        }
        
        if (Input.GetKey(KeyCode.LeftControl) || GameMgr.Instance.OpenUICount > 0)
        {
            Stop();
            anim.Stop();
            UnityEngine.Cursor.visible = true;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
        }
        else if(GameMgr.Instance.OpenUICount == 0)
        {
            UnityEngine.Cursor.visible = false;
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        }

        playTime += Time.deltaTime * 1.0f;
        playTime = Mathf.Floor(playTime * 100f) / 100f;
    }

    public void SetSensivity(float value)
    {
        sensivity = value;
    }

    bool CheckScene()
    {
        if (SceneManager.GetActiveScene().name == "Main")
            return true;
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(meshTr.position, mainCameraPos.position );
    }

    void CheckCameraPos()
    {
        
        if (Physics.Raycast(meshTr.position, (mainCameraPos.position - meshTr.position).normalized, out RaycastHit hit, maxCameraDistance,LayerMask.GetMask("Default"))) 
            GameMgr.Instance.mainCam.transform.position = hit.point;
        else
            GameMgr.Instance.mainCam.transform.position = mainCameraPos.transform.position;
    }

    public void Initialize(PlayerSaveData saveData, float sensivity)
    {
        maxCameraDistance = Vector2.Distance(transform.position, mainCameraPos.position);
        equipItems = new EquipableItemData[(int)ItemMgr.EquipType.Shield + 1];
        this.sensivity = sensivity;

        if(saveData == null)
        {
            Level = 1;
            currEXP = 0;
            HP = data.HP;
            SP = data.SP;
            MP = data.MP;
            UpdateEquipmentItemStatue();
            InterfaceMgr.Instance.Initailize(this);
            inventory = new Inventory(this);
            quickSlot = new QuickSlot(this);
            skillSlot = new SkillSlot(this);
        }
        else
        {
            Level = saveData.Level;
            currEXP = saveData.EXP;
            HP = saveData.HP;
            SP = saveData.SP;
            MP = saveData.MP;
            playTime = saveData.PlayTime;

            InterfaceMgr.Instance.Initailize(this);
            for (int i=0;i<equipItems.Length;i++)
            {
                if (saveData.equipItems[i] == -1)
                    continue;
                EquipItem((EquipableItemData)ItemMgr.Instance.GetItem(saveData.equipItems[i]));
            }
            inventory = new Inventory(this, saveData.Items, saveData.Gold);
            quickSlot = new QuickSlot(this, saveData.itemSlot);
            skillSlot = new SkillSlot(this, saveData.SkillSlots);
        }

        SetNextLevelEXP();
        UpdateLevelStatus();

        guardPoint = data.GP;

        selectInteractionTimer = 0.5f;
        InterfaceMgr.Instance.SetPlayerInfo(this);

        if(lockOnTargets == null)
            lockOnTargets = new List<Collider>();
        lockOnIndex = 0;

        interaction = false;
        IsLockOn = false;
        IsGuardBreak = false;
        IsGuardSuccess = false;
        IsGuard = false;
        runTowalk = false;
        guardToEvade = false;
        IsInteraction = false;

        accel = 0f;
        dir = Vector3.zero;
        maxSpeed = data.WalkSpeed;
        reverseWalkSpeed = 1 / data.WalkSpeed;
        act = Act.idle;
        animVelocity = Vector3.zero;

        stateData = ScriptableObject.CreateInstance<PlayerStateData>();
        stateData.SetData(idleState, runState, evadeState, guardState, attackState, damagedState, dieState, guardBreakState, skillState, executeState, chargeState, chargeAttackState, rushState);
        SetData();

        InterfaceMgr.Instance.UpdateHP(HP);
        InterfaceMgr.Instance.UpdateMP(MP);
        InterfaceMgr.Instance.UpdateSP(SP);
        InterfaceMgr.Instance.UpdatePlayerLevelInfo(Level, currEXP, needNextLevelEXP);
        InterfaceMgr.Instance.UpdatePlayerStatusInfo(levelStatus, equipmentItemStatus);

        DontDestroyOnLoad(gameObject);
    }

    public override void Stop()
    {
        base.Stop();
        anim.Stop();
    }

    void PlayerInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        float horizontalDir = Input.GetAxis("HorizontalDir");
        float verticalDir = Input.GetAxis("VerticalDir");

        inputDir = new Vector2(horizontalDir, verticalDir);
        dir = GetDir(inputDir);
        velocity = accel * dir;
        if(horizontalInput != 0f || verticalInput != 0f)
            SoundMgr.Instance.PlayMoveSound();
        else
            SoundMgr.Instance.StopMoveSound();
    }

    bool SetData()
    {
        if (stateData == null)
            return false;

        if (fsm == null)
            fsm = new FSM<Player>(this);

        if (!fsm.SetCurrState(stateData.IdleState))
            return false;

        StartCoroutine(OnUpdate());
        return true;
    }

    IEnumerator OnUpdate()
    {
        while (true)
        {
            fsm.Update();

            if (!IsGuard && guardPoint <= data.GP)
            {
                guardPoint += updateTime * 2f;
                guardPoint = Mathf.Clamp(guardPoint, 0, data.GP);
                InterfaceMgr.Instance.GuardGaugeUpdate(guardPoint);
                if (guardPoint == data.GP)
                    InterfaceMgr.Instance.GuardGaugeOnOff(false);
            }

            if (IsGuardBreak && guardPoint == data.GP)
                IsGuardBreak = false;

            if (selectInteractionTimer > 0f)
                selectInteractionTimer -= updateTime;

            MP += Level * levelStatus.recoveryMP + equipmentItemStatus.recoveryMP;
            MP = Mathf.Clamp(MP, 0f, data.MP);
            InterfaceMgr.Instance.UpdateMP(MP);

            if (a != null)
                a.transform.position = this.transform.position;

            yield return new WaitForSeconds(updateTime);
        }
    }

    public void StaminaPlus()
    {
        if (SP < data.SP)
        {
            SP += Level * levelStatus.recoverySP + equipmentItemStatus.recoverySP;
            SP = Mathf.Clamp(SP, 0f, data.SP);
            InterfaceMgr.Instance.UpdateSP(SP);
        }
    }

    public void StaminaMinus()
    {
        if (SP > 0)
        {
            SP -= updateTime;
            InterfaceMgr.Instance.UpdateSP(SP);
        }
    }

    Vector3 GetDir(Vector2 InputDir)
    {
        return (transform.forward * InputDir.y + transform.right * InputDir.x).normalized;
    }


    public override void Move()
    {
        if (runTowalk)
        {
            accel -= data.RunSpeed * Time.deltaTime;
            accel = Mathf.Clamp(accel, data.WalkSpeed, data.RunSpeed);
            if (accel == maxSpeed)
                runTowalk = false;
        }
        else
        {
            accel += maxSpeed * Time.deltaTime;
            accel = Mathf.Clamp(accel, 0, maxSpeed);
        }

        animVelocity = new Vector2(horizontalInput, verticalInput) * accel;

        if (rigid.velocity.y > 0)
            rigid.velocity = new Vector3(velocity.x, -9.86f, velocity.z);
        else
            rigid.velocity = new Vector3(velocity.x, rigid.velocity.y, velocity.z);
        anim.Move(animVelocity, reverseWalkSpeed);
    }

    public void RunAction(InputAction.CallbackContext context)
    {
        if (act == Act.idle || act == Act.run && !IsInteraction && !UnityEngine.Cursor.visible)
        {
            runTowalk = false;
            maxSpeed = data.RunSpeed;
            OnRunState();
        }
    }

    public void RunStop(InputAction.CallbackContext context)
    {
        if (act == Act.idle || act == Act.run)
        {
            runTowalk = true;
            maxSpeed = data.WalkSpeed;
            OnIdleState();
        }
    }

    public void MouseDelta(InputAction.CallbackContext context)
    {
        if (CheckMouseDelta())
        {
            Vector2 delta = context.ReadValue<Vector2>();

            if (delta.x > 0) angleY += sensivity * Time.deltaTime;
            else if (delta.x < 0) angleY -= sensivity * Time.deltaTime;

            transform.localEulerAngles = Vector3.up * angleY;
        }
    }

    bool CheckMouseDelta()
    {
        return (act != Act.evade && act != Act.attack && act != Act.die && !IsLockOn && !IsInteraction && (!GameMgr.Instance.isShop) && !UnityEngine.Cursor.visible);
    }

    public void EvadeAction(InputAction.CallbackContext context)
    {
        if (EvadeCheck())
        {
            if (IsGuard)
                guardToEvade = true;
            OnEvadeState();
        }
    }

    bool EvadeCheck()
    {
        if (SP >= 10f && dir != Vector3.zero && act != Act.evade && !runTowalk && act != Act.attack && act != Act.run && !IsInteraction && !UnityEngine.Cursor.visible)
            return true;
        else
            return false;
    }

    public void Evade()
    {
        SP -= 10f;
        InterfaceMgr.Instance.UpdateSP(SP);
        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(OnEvade());
    }

    IEnumerator OnEvade()
    {
        float t = 0f;
        float accel = 100f;
        Vector3 evadeStart = velocity;
        Vector3 evadeEnd = dir * data.EvadeSpeed;
        while (true)
        {
            velocity = Vector3.Lerp(evadeStart, evadeEnd, t);
            t += updateTime * accel;
            accel -= 30f;
            yield return new WaitForSeconds(updateTime);
        }
    }

    public void EvadeAnim()
    {
        anim.Evade(inputDir);
    }

    public void EvadeStop()
    {
        if (coroutine != null)
            StopCoroutine(coroutine);

        if (guardToEvade)
        {
            anim.GuardStop();
            guardToEvade = false;
        }
    }

    public void NormalAttackAction(InputAction.CallbackContext context)
    {
        if ((act == Act.idle || act == Act.attack) && !runTowalk  && !IsInteraction && (!GameMgr.Instance.isShop) && !UnityEngine.Cursor.visible)
        {
            OnAttackState();
        }
    }

    public void Attack()
    {
        anim.Attack();
    }

    public void GuardAction(InputAction.CallbackContext context)
    {
        if ((act == Act.idle || act == Act.guard) && !IsGuardBreak && SP > 0 && !UnityEngine.Cursor.visible)
        {
            if (IsGuard)
                OnIdleState();
            else
                OnGuardState();
        }
    }

    public void GuardOn()
    {
        IsGuard = true;
        maxSpeed = data.GuardSpeed;
        anim.Guard();
        InterfaceMgr.Instance.GuardGaugeOnOff(true);
    }

    public void GuardOff()
    {
        IsGuard = false;
        IsGuardSuccess = false;
        maxSpeed = data.WalkSpeed;
        if (!guardToEvade && !IsGuardBreak)
            anim.GuardStop();
    }

    public void Guard()
    {
        IsGuardSuccess = IsGuard;
        Stop();
        EffectMgr.Instance.PlayParticleSystem("GuardHit", guardPos.position, guardPos.eulerAngles);
        GameMgr.Instance.ShakeCamera(0.5f, 0.5f);
        guardPoint -= 10f;
        InterfaceMgr.Instance.GuardGaugeUpdate(guardPoint);
        if (guardPoint <= 0f)
        {
            SoundMgr.Instance.PlaySFXAudio("Guard_Break");
            OnGuardBreakState();
        }
        else
        {
            SoundMgr.Instance.PlaySFXAudio("Guard");
            anim.GuardSuccess();
        }
    }

    public void GuardBreak()
    {
        anim.GaurdBreak();
    }

    public void GuardBreakEnd()
    {
        anim.GuardStop();
    }

    void LevelUP()
    {
        Level++;
        currEXP -= needNextLevelEXP;
        UpdateLevelStatus();
        SetNextLevelEXP();
        InterfaceMgr.Instance.SetLevelUpUI(Level);
        InterfaceMgr.Instance.UpdateHP(HP);
        SoundMgr.Instance.PlaySFXAudio("LevelUP");
        levelUpEffect.Play();
    }

    void SetNextLevelEXP()
    {
        needNextLevelEXP = data.NextLevelEXP + data.NextLevelEXP * (Level - 1) * 0.5f;
    }

    private void OnTriggerEnter(Collider collision)
    {
        switch(collision.tag)
        {
            case "Monster":
                IFightable attackTarget = collision.GetComponentInParent<IFightable>();
                currEXP += attackTarget.OnDamage(data.Attack+levelStatus.attack + equipmentItemStatus.attack);
                SoundMgr.Instance.PlaySFXAudio("Attack_Sword_Hit");
                if (currEXP >= needNextLevelEXP)
                {
                    LevelUP();
                    InterfaceMgr.Instance.UpdatePlayerStatusInfo(levelStatus, equipmentItemStatus);
                    InterfaceMgr.Instance.LevelUP(HPMax);
                }
                InterfaceMgr.Instance.UpdatePlayerLevelInfo(Level, currEXP, needNextLevelEXP);
                break;
            //case "InteractionObject":
            //    IInteractionable interactionTarget = collision.GetComponent<IInteractionable>();
            //    interactionTarget.AddInteractionToList();
            //    break;
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        switch(collision.tag)
        {
            case "InteractionObject":
                {
                    IInteractionable interactionTarget = collision.GetComponent<IInteractionable>();
                    interactionTarget.AddInteractionToList();
                    interaction = true;
                }
                break;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        switch(collision.tag)
        {
            case "InteractionObject":
                {
                    interaction = false;
                    IInteractionable interactionTarget = collision.GetComponent<IInteractionable>();
                    interactionTarget.RemoveInteractionToList();
                }
                break;
        }
    }

    public void GetItems(List<DropItem> items)
    {
        for (int i = 0; i < items.Count; i++)
        {
            inventory.GetItem(items[i]);
            QuestMgr.Instance.UpdateItem(items[i]);
        }   
    }

    public void GetItem(DropItem item)
    { 
        inventory.GetItem(item);
        QuestMgr.Instance.UpdateItem(item);
    }

    public void PlusGold(int gold) { inventory.PlusGold(gold); }

    public bool MinusGold(int gold) { return inventory.MinusGold(gold); }

    public void DragItemInventoryToQuickSlot(DropItem item, int startIndex, int endIndex, int amount = 0)
    {
        DropItem temp = new DropItem();
        if (amount != 0)
        {
            temp = item;
            temp.amount -= amount;
            item.amount = amount;
        }
        else
        {
            temp = quickSlot.GetItem(endIndex);

            if (temp.amount != 0)
            {
                if (item.id == temp.id)
                {
                    CountableItemData cid = ItemMgr.Instance.GetItem(temp.id) as CountableItemData;
                    if (temp.amount == cid.MaxAmount)
                    {
                        DropItem temp2 = new DropItem();
                        temp2 = item;
                        item = temp;
                        temp = temp2;
                    }
                    else if (item.amount + temp.amount > cid.MaxAmount)
                    {
                        item.amount -= cid.MaxAmount - temp.amount;
                        temp.amount = cid.MaxAmount;
                    }
                    else
                    {
                        temp.amount += item.amount;
                        item.amount = 0;
                        item.id = -1;
                    }
                }
                else
                {
                    DropItem temp2 = new DropItem();
                    temp2 = item;
                    item = temp;
                    temp = temp2;
                }
            }
            else
            {
                temp = item;
                item.amount = 0;
                item.id = -1;
            }
        }

        if (item.id == -1)
            InterfaceMgr.Instance.ClearSlot(MakeSlot.SlotType.Inventory, startIndex);
        else
            inventory.DragItemQuickToInventory(item, startIndex);

        if (temp.id == -1)
            InterfaceMgr.Instance.ClearSlot(MakeSlot.SlotType.Quick, endIndex);
        else
            quickSlot.DragItemInventoryToQuick(temp, endIndex);

        //inventory.DragItemQuickToInventory(item, startIndex);
        //quickSlot.DragItemInventoryToQuick(temp, endIndex);
    }

    public void DragItemQuickToInventory(DropItem item, int endIndex)
    {
        inventory.DragItemQuickToInventory(item, endIndex);
    }

    public void QuickSlot1(InputAction.CallbackContext context)
    {
        quickSlot.Use(0);
    }

    public void QuickSlot2(InputAction.CallbackContext context)
    {
        quickSlot.Use(1);
    }

    public void QuickSlot3(InputAction.CallbackContext context)
    {
        quickSlot.Use(2);
    }

    public void QuickSlot4(InputAction.CallbackContext context)
    {
        quickSlot.Use(3);
    }

    public void QuickSlot5(InputAction.CallbackContext context)
    {
        quickSlot.Use(4);
    }

    public void QuickSlot6(InputAction.CallbackContext context)
    {
        quickSlot.Use(5);
    }

    public void QuickSlot7(InputAction.CallbackContext context)
    {
        quickSlot.Use(6);
    }

    public void QuickSlot8(InputAction.CallbackContext context)
    {
        quickSlot.Use(7);
    }

    public void QuickSkill1(InputAction.CallbackContext context)
    {
        if(act == Act.idle || IsGuard)
        {
            skillSlot.Use(0);
        }
    }

    public void QuickSkill2(InputAction.CallbackContext context)
    {
        if (act == Act.idle || IsGuard)
        {
            skillSlot.Use(1);
        }
    }

    public void QuickSkill3(InputAction.CallbackContext context)
    {
        if (act == Act.idle || IsGuard)
        {
            skillSlot.Use(2);
        }
    }

    public void OpenSkillUI(InputAction.CallbackContext context)
    {
        InterfaceMgr.Instance.SkillUI();
    }

    public void InteractionAction(InputAction.CallbackContext context)
    {
        if(interaction)
        {
            IsInteraction = false;
            Stop();
            anim.Stop();
            InterfaceMgr.Instance.SelectInteraction();
        }
    }

    public void OpenPlayerInfoAction(InputAction.CallbackContext context)
    {
        InterfaceMgr.Instance.PlayerInfoUI();
    }

    bool CheckGuardSuccess(Vector3 enemyPos)
    {
        Vector2 test = new Vector2(enemyPos.x, enemyPos.z) - new Vector2(transform.position.x, transform.position.z);
        float dot = Vector2.Dot(new Vector2(transform.forward.x, transform.forward.z), test);
        return (dot >= 4f);
    }

    public void LockOnAction(InputAction.CallbackContext context)
    {
        if (act == Act.idle || act == Act.guard  && !IsInteraction)
        {
            lockOnIndex = 0;
            if (!IsLockOn)
            {
                if (FindLockOnTargets())
                {
                    IsLockOn = true;
                    InterfaceMgr.Instance.LockOnImageOn();
                }
            }
            else
                LockOnEnd();
        }
    }

    bool CheckLockOn()
    {
        return (IsLockOn && (Vector3.Distance(this.transform.position, LockOnTarget.position) <= data.LockOnRange));
    }

    void LockOnEnd()
    {
        IsLockOn = false;
        lockOnTargets.Clear();
        LockOnTarget = null;
        lockOnIndex = 0;
        angleX = transform.localEulerAngles.x;
        angleY = transform.localEulerAngles.y;
        InterfaceMgr.Instance.LockOnImageOff();
    }

    public void LockOnMoveAction(InputAction.CallbackContext context)
    {
        if (IsLockOn)
        {
            float value = context.ReadValue<float>();
            switch (value)
            {
                case 1:
                    if (lockOnTargets.Count > lockOnIndex + 1)
                    {
                        Monster nextTarget = lockOnTargets[lockOnIndex + 1].GetComponent<Monster>();
                        if (!nextTarget.isDead)
                            lockOnIndex++;
                    }
                    break;
                case -1:
                    if (0 < lockOnIndex)
                    {
                        Monster prevTarget = lockOnTargets[lockOnIndex - 1].GetComponent<Monster>();
                        if (!prevTarget.isDead)
                            lockOnIndex--;
                    }
                    break;
            }

            LockOnTarget = lockOnTargets[lockOnIndex].transform;
        }
    }

    bool FindLockOnTargets()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, data.LockOnRange, data.TargetLayer);
        if (targets != null && targets.Length > 0)
        {
            for (int i = 0; i < targets.Length; i++)
            {
                Vector3 viewPos = Camera.main.WorldToViewportPoint(targets[i].transform.position);
                if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1)
                {
                    lockOnTargets.Add(targets[i]);
                }
            }

            if (lockOnTargets.Count == 0)
                return false;
            else
            {
                LockOnTarget = lockOnTargets[0].transform;
                return true;
            }
        }
        else
            return false;
    }

    public void OnDamage(float damage, Vector3 enemyPos)
    {
        if(act != Act.Execute)
        {
            if (IsGuard && CheckGuardSuccess(enemyPos))
                Guard();
            else
            {
                GameMgr.Instance.ShakeCamera(0.5f, 1f);
                SoundMgr.Instance.PlaySFXAudio("Player_Damaged");
                EffectMgr.Instance.PlayParticleSystem("Hit", transform.position+Vector3.up * 10);
                HP -= damage; //(levelStatus.defence + equipmentItemStatus.defence) damage;
                HP = Mathf.Clamp(HP, 0, HPMax);
                InterfaceMgr.Instance.UpdateHP(HP);
                if (isDead)
                    OnDieState();
                else
                     OnDamagedState();
            }
        }
    }

    public float OnDamage(float damage) { return 0; }

    public void DamagedAnim()
    {
        anim.Damaged();
    }

    public void Die()
    {
        anim.Die();
        CapsuleCollider collider = GetComponent<CapsuleCollider>();
        collider.enabled = false;
        rigid.useGravity = false;
    }

    public bool RestoreHealth(ItemMgr.PortionType type, float value)
    {
        switch(type)
        {
            case ItemMgr.PortionType.HP:
                if (HP >= data.HP)
                    return false;
                HP += value;
                HP = Mathf.Clamp(HP, 0, data.HP);
                InterfaceMgr.Instance.UpdateHP(HP);
                break;
            case ItemMgr.PortionType.MP:
                
                break;
            case ItemMgr.PortionType.SP:
                if (SP >= data.SP)
                    return false;
                SP += value;
                SP = Mathf.Clamp(SP, 0, data.SP);
                InterfaceMgr.Instance.UpdateSP(SP);
                break;
        }

        return true;
    }

    void UpdateLevelStatus()
    {
        HPMax = data.HP + (Level - 1) * data.PlusHP;
        HP = HPMax;
        levelStatus.attack = Level * data.PlusDamage;
        levelStatus.defence = Level * data.PlusDefence;
        levelStatus.recoveryMP = Level * data.PlusRecoveryMP;
        levelStatus.recoverySP = Level * data.PlusRecoverySP;
    }

    public int EquipmentExchange(ItemMgr.EquipType type)
    {
        if (equipItems[(int)type] == null)
            return -1;
        SoundMgr.Instance.PlaySFXAudio("Equip");
        int unequipItemId = equipItems[(int)type].ID;
        equipItems[(int)type] = null;
        InterfaceMgr.Instance.PlayerInfoClearSlot((int)type);
        return unequipItemId;
    }

    public void UnequipItem(int index)
    {
        SoundMgr.Instance.PlaySFXAudio("Equip");
        DropItem unequipItem = new DropItem();
        unequipItem.id = equipItems[index].ID;
        unequipItem.amount = 1;
        inventory.GetItem(unequipItem);
        equipItems[index] = null;
        UpdateEquipmentItemStatue();
        InterfaceMgr.Instance.UpdatePlayerStatusInfo(levelStatus, equipmentItemStatus);
    }

    public bool EquipItem(EquipableItemData itemData)
    {
        if (itemData == null)
            return false;

        SoundMgr.Instance.PlaySFXAudio("Equip");
        equipItems[(int)itemData.EquipType] = itemData;
        UpdateEquipmentItemStatue();
        DropItem temp = new DropItem();
        temp.id = itemData.ID;
        temp.amount = 1;
        InterfaceMgr.Instance.AddItemToPlayerInfoUI(temp, (int)itemData.EquipType);
        InterfaceMgr.Instance.UpdatePlayerStatusInfo(levelStatus, equipmentItemStatus);
        return true;
    }

    void UpdateEquipmentItemStatue()
    {
        ResetPlayerStatus();
        for (int i = (int)ItemMgr.EquipType.Armor; i <= (int)ItemMgr.EquipType.Shield; i++)
        {
            if (equipItems[i] == null)
                continue;

            equipmentItemStatus.attack += equipItems[i].Status.attack;
            equipmentItemStatus.defence += equipItems[i].Status.defence;
            equipmentItemStatus.recoveryMP += equipItems[i].Status.recoveryMP;
            equipmentItemStatus.recoverySP += equipItems[i].Status.recoverySP;
            equipmentItemStatus.guardGauge += equipItems[i].Status.guardGauge;
        }
    }

    void ResetPlayerStatus()
    {
        equipmentItemStatus.attack = 0f;
        equipmentItemStatus.defence = 0f;
        equipmentItemStatus.recoveryMP = 0f;
        equipmentItemStatus.recoverySP = 0f;
        equipmentItemStatus.guardGauge = 0f;
    }

    public void InventoryAction(InputAction.CallbackContext context)
    {
        InterfaceMgr.Instance.InventoryUI();
    }

    public void SelectInterAction(InputAction.CallbackContext context)
    {
        if(selectInteractionTimer <= 0f && !IsInteraction)
        {
            float value = context.ReadValue<float>();
            if (value > 0)
                InterfaceMgr.Instance.WheelUp();
            else
                InterfaceMgr.Instance.WheelDown();
        }
    }

    public void SkillAnim()
    {
        anim.Skill();
    }

    public void Execute()
    {
        Stop();
    }

    public void ExecuteEnd()
    {

    }

    public void ExecuteAnim()
    {
        anim.Execute();
    }

    public void Charge()
    {
        Stop();
        EffectMgr.Instance.PlayParticleSystem("Charge", weaponPos.position);
    }

    public void ChargeAnim()
    {
        anim.ChargeAttack();
    }

    public void ChargeAttack()
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(OnChargeAttack());
    }

    public void ChargeAttackEnd()
    {
        StopCoroutine(coroutine);
    }

    IEnumerator OnChargeAttack()
    {
        float t = 0f;
        float accel = 150f;
        Vector3 jumpStart = velocity;
        Vector3 jumpEnd = transform.forward * 150f;
        while(true)
        {
            velocity = Vector3.Lerp(jumpStart, jumpEnd, t);
            t += updateTime * accel;
            accel -= 50f;
            yield return new WaitForSeconds(updateTime);
        }
    }

    ParticleSystem a = null;

    public void Rush()
    {
        attackCollider.gameObject.SetActive(true);
        playerCollider.enabled = false;
        a = Instantiate(rushEffect);
        a.Play();
        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(OnRush());
    }

    public void RushEnd()
    {
        playerCollider.enabled = true;
        a.Stop();
        anim.RushEnd();
        StopCoroutine(coroutine);
        attackCollider.gameObject.SetActive(false);
    }

    IEnumerator OnRush()
    {
        velocity = transform.forward * 100f * Time.deltaTime;
        yield return new WaitForSeconds(20.5f);
        anim.RushEnd();
        Stop();
    }

    public void RushAnim()
    {
        anim.Rush();
    }

    public void MapAction(InputAction.CallbackContext context)
    {
        InterfaceMgr.Instance.MapUI(gameObject.transform);
    }

    public void ESCAction(InputAction.CallbackContext context)
    {
        InterfaceMgr.Instance.ESCUI();
    }

    public PlayerSaveData MakeSaveData()
    {
        PlayerSaveData data = new PlayerSaveData();

        data.Level = this.Level;
        data.MapID = MapMgr.Instance.GetMapData(SceneManager.GetActiveScene().name).MapID;
        data.HP = this.HP;
        data.MP = this.MP;
        data.SP = this.SP;
        data.EXP = this.currEXP;
        data.Gold = inventory.GetGold();
        data.PlayerPos = transform.position;
        data.PlayerAngle = transform.eulerAngles;
        data.SaveTime = DateTime.Now.ToString();
        data.PlayTime = playTime;

        int[] equipItems = new int[this.equipItems.Length];
        for(int i=0;i<equipItems.Length;i++)
        {
            if (this.equipItems[i] == null)
            {
                equipItems[i] = -1;
                continue;
            }
                
            equipItems[i] = this.equipItems[i].ID;
        }
        data.equipItems = equipItems;
        data.Items = inventory.GetInventoryAllItem();
        data.itemSlot = quickSlot.GetAllItem();
        data.SkillSlots = skillSlot.GetAllSkills();

        int[] skillLevels = new int[SkillMgr.Instance.SkillCount];
        for(int i=0;i<SkillMgr.Instance.SkillCount;i++)
            skillLevels[i] = SkillMgr.Instance.GetSkill(i).Level;

        data.SkillLevels = skillLevels;

        return data;
    }

    public Transform GetMainCameraPos() { return mainCameraPos; }
    public PlayerData GetPlayerData() { return data; }

    public void OnIdleState()
    {
        if (!fsm.ChangeState(stateData.IdleState))
        {
            ErrorMessage("Idle State가 Null 입니다.");
        }
    }

    public void OnRunState()
    {
        if (!fsm.ChangeState(stateData.RunState))
        {
            ErrorMessage("Run State가 Null 입니다.");
        }
    }

    public void OnEvadeState()
    {
        if (!fsm.ChangeState(stateData.EvadeState))
        {
            ErrorMessage("Evade State가 Null 입니다.");
        }
    }

    public void OnGuardState()
    {
        if (!fsm.ChangeState(stateData.GuardState))
        {
            ErrorMessage("Guard State가 Null 입니다.");
        }
    }

    public void OnAttackState()
    {
        if (!fsm.ChangeState(stateData.AttackState))
        {
            ErrorMessage("Attack State가 Null 입니다.");
        }
    }

    public void OnDamagedState()
    {
        if (!fsm.ChangeState(stateData.DamagedState))
        {
            ErrorMessage("Damaged State가 Null 입니다.");
        }
    }

    public void OnDieState()
    {
        if (!fsm.ChangeState(stateData.DieState))
        {
            ErrorMessage("Die State가 Null 입니다.");
        }
    }

    public void OnGuardBreakState()
    {
        IsGuardBreak = true;
        if (!fsm.ChangeState(stateData.GuardBreakState))
        {
            ErrorMessage("Guard Break State가 Null 입니다.");
        }
    }

    public void OnSkillState()
    {
        if(!fsm.ChangeState(stateData.SkillState))
        {
            ErrorMessage("Skill State가 Null 입니다.");
        }
    }

    public void OnExecuteState()
    {
        if(!fsm.ChangeState(stateData.ExecuteState))
        {
            ErrorMessage("Execute State가 Null 입니다.");
        }
    }

    public void OnChargeState()
    {
        if(!fsm.ChangeState(stateData.ChargeState))
        {
            ErrorMessage("Charge State가 Null 입니다.");
        }
    }

    public void OnChargeAttackState()
    {
        if(!fsm.ChangeState(stateData.ChargeAttackState))
        {
            ErrorMessage("ChargeAttack State가 Null 입니다.");
        }
    }

    public void OnRushState()
    {
        if(!fsm.ChangeState(stateData.RushState))
        {
            ErrorMessage("Rush State가 Null 입니다.");
        }
    }
}
