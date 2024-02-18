using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Archer : Character
{
    [Header("Function")]
    [SerializeField] protected Bow bow;
    [SerializeField] protected ArcherAnimation anim;

    protected int useArrowIndex;

    protected Vector2 arrowDir;
    protected Vector2 start;
    protected Vector2 targetPos;
    protected float angle;

    protected enum Act { Idle, Move, Charge, Shot}
    protected Act act;

    public BowData bowData { get; set; }
    public ArrowData[] Arrows { get; set; }
    public int[] ArrowCount { get; set; }

    protected virtual void Awake()
    {
        rigid2d = GetComponent<Rigidbody2D>();
        if(rigid2d)
        {
            rigid2d.freezeRotation = true;
            rigid2d.gravityScale = 0;
        }
    }

    private void FixedUpdate()
    {
        ApplyVelocity();
    }

    protected void ApplyVelocity()
    {
        rigid2d.velocity = velocity;
    }

    public override void Initialize()
    {
        Stop();

        // 0 : 화살1, 1 : 화살2, 2 : 기본화살
        Arrows = new ArrowData[3];
        ArrowCount = new int[2];
        useArrowIndex = 2;

        for (int i = 0; i < Arrows.Length; i++)
            Arrows[i] = (ArrowData)Resources.Load("Arrow/Normal Arrow"); ;

        for (int i = 0; i < ArrowCount.Length; i++)
            ArrowCount[i] = 0;

        bowData = (BowData)Resources.Load("Bow/Normal Bow");

        bow.Initialize(bowData);
        anim.Initialize();

        act = Act.Idle;
    }

    protected void GetDirVector()
    {
        start = bow.ShotPos;
        arrowDir = (targetPos - start).normalized;
        angle = Vector2.Angle(Vector2.right, arrowDir);
    }

    public void Charge(int playType = 0)
    {
        if (Arrows[useArrowIndex] != Arrows[2])
        {
            ArrowCount[useArrowIndex]--;
            if(ArrowCount[useArrowIndex] <= 0)
            {
                Arrows[useArrowIndex] = Arrows[2];
                InterfaceMgr.Instance.UseNormalArrow();
            }
        }

        bow.Charge(Arrows[useArrowIndex], playType);
        attackTimer = bowData.TimeBetShot;

        InterfaceMgr.Instance.UpdateArrowInfo(useArrowIndex);
    }

    public void Shot()
    {
        bow.Shot(arrowDir, Arrows[useArrowIndex].Damage);
    }

    public void MoveAnim()
    {
        anim.Move();
    }

    public void StopAnim()
    {
        anim.Stop();
    }

    public void ChargeAnim()
    {
        anim.Charge();
    }

    public void ShotAnim()
    {
        anim.Shot();
    }

    public void MountBow(BowData bowData)
    {
        if (bowData)
        {
            ItemMgr.Instance.PlusItem(this.bowData.ID);
            this.bowData = bowData;
            bow.Initialize(this.bowData);
        }
    }

    public void MountArrow1(ArrowData arrowData, int count = 0)
    {
        if(arrowData)
        {
            ItemMgr.Instance.PlusItem(Arrows[0].ID, ArrowCount[0]);
            ArrowCount[0] = count;
            Arrows[0] = arrowData;
        }
    }

    public abstract void OnIdleState();
    public abstract void OnMoveState();
    public abstract void OnChargeState();
    public abstract void OnShotState();
}
