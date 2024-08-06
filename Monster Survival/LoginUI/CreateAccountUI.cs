using Firebase.Auth;
using Firebase;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using TMPro;
using UnityEngine;

public class CreateAccountUI : MonoBehaviour
{
    [SerializeField] TMP_InputField player_email;
    [SerializeField] TMP_InputField auth_code;
    [SerializeField] TMP_InputField nickname;
    [SerializeField] TMP_InputField new_password;
    [SerializeField] TMP_InputField check_password;
    [SerializeField] TextMeshProUGUI auth_code_text;
    [SerializeField] GameObject email_fail;
    [SerializeField] GameObject auth_fail;
    [SerializeField] GameObject auth_success;
    [SerializeField] GameObject pwd_fail;
    [SerializeField] GameObject pwd_success;
    [SerializeField] GameObject pop_up;

    protected const string system_mail_id = "wldalsdl0408@naver.com";
    protected const string System_mail_pwd = "rudalsdl0408";

    Coroutine coroutine = null;
    int randomNum;
    int auth_code_timer;
    bool auth;

    private void FixedUpdate()
    {
        if (auth && check_password.text.Length >= 1)
            CheckPassword();
    }

    private void OnEnable()
    {
        auth_code_text.text = "인증 코드 받기";
        auth_code.text = string.Empty;
        new_password.text = string.Empty;
        check_password.text = string.Empty;
        player_email.text = string.Empty;
        Init();
    }

    protected virtual void Init()
    {
        auth = false;
        auth_fail.SetActive(false);
        auth_success.SetActive(false);
        pwd_fail.SetActive(false);
        pwd_success.SetActive(false);
        new_password.readOnly = true;
        check_password.readOnly = true;
        pop_up.SetActive(false);
        email_fail.SetActive(false);
    }

    void SetAuthCodeCount()
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
        auth_code_timer = 90;
        coroutine = StartCoroutine(CountAuthCodeTime());
    }

    IEnumerator CountAuthCodeTime()
    {
        while (true)
        {
            if (auth_code_timer >= 0)
            {
                int min = Mathf.FloorToInt(auth_code_timer / 60);
                int sec = Mathf.FloorToInt(auth_code_timer % 60);
                auth_code_text.text = string.Format("{0:D1}:{1:D2}", min, sec);
            }
            else
            {
                auth_code_text.text = "재전송";
                randomNum = -100;
                break;
            }
            yield return new WaitForSecondsRealtime(1f);
            auth_code_timer--;
        }
    }

    public virtual void OnClickSendCode()
    {
        email_fail.SetActive(false);
        randomNum = UnityEngine.Random.Range(100000, 999999);
        Init();

        MailMessage mail = new MailMessage();

        mail.To.Add(player_email.text);
        mail.From = new MailAddress(system_mail_id);
        mail.Subject = "회원가입 본인인증 코드";
        mail.Body = randomNum.ToString();
        mail.IsBodyHtml = true;
        mail.Priority = MailPriority.High;
        mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
        mail.SubjectEncoding = Encoding.UTF8;
        mail.BodyEncoding = Encoding.UTF8;

        SmtpClient smtp = new SmtpClient("smtp.naver.com", 587);
        smtp.EnableSsl = true;
        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
        smtp.Credentials = new System.Net.NetworkCredential(system_mail_id, System_mail_pwd);

        try
        {
            smtp.Send(mail);
            smtp.Dispose();
            Debug.Log("이메일 전송 완료");
            SetAuthCodeCount();

        }
        catch (Exception ex)
        {
            FirebaseException firebase_ex = ex.GetBaseException() as FirebaseException;
            AuthError error_code = (AuthError)firebase_ex.ErrorCode;
        }
    }

    public void CheckAuthCode()
    {
        if (randomNum.ToString().Equals(auth_code.text))
        {
            auth = true;
            auth_success.SetActive(true);
            auth_fail.SetActive(false);

            if (coroutine != null)
                StopCoroutine(coroutine);
            auth_code_text.text = "인증 코드 받기";
            new_password.readOnly = false;
            check_password.readOnly = false;
        }
        else
        {
            auth = false;
            auth_success.SetActive(false);
            auth_fail.SetActive(true);
            new_password.readOnly = true;
            check_password.readOnly = true;
        }
    }

    void CheckPassword()
    {
        if (new_password.text.Equals(check_password.text))
        {
            pwd_success.SetActive(true);
            pwd_fail.SetActive(false);
        }
        else
        {
            pwd_success.SetActive(false);
            pwd_fail.SetActive(true);
        }
    }

    public void OnClickCreateNewAccount()
    {
        if (pwd_success.activeSelf)
        {
            FirebaseManager.Instance.Create(player_email.text, new_password.text, nickname.text);
            pop_up.SetActive(true);
        }
    }
}
