//using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardedButton : MonoBehaviour
{
    public GameObject content;
    public GameObject adAvailableTextHolder;
    public TimerText timerText;

    private const string ACTION_NAME = "rewarded_video";
    private bool isEventAttached;


    public void OnClick()
    {
        //AdmobController.instance.ShowRewardBasedVideo();
       
        AdmobController.instance.ShowRewardVideo(()=> {

            var dialog = (RewardedVideoDialog)DialogController.instance.GetDialog(DialogType.RewardedVideo);
            dialog.SetAmount(ConfigController.Config.rewardedVideoAmount);
            DialogController.instance.ShowDialog(dialog);
        });
        Sound.instance.PlayButton();
        DialogController.instance.CloseCurrentDialog();
    }

    private void CompleteMethod(bool completed, string advertiser)
    {
        Debug.Log("Closed rewarded from: " + advertiser + " -> Completed " + completed);
        if (completed == true)
        {
            var dialog = (RewardedVideoDialog)DialogController.instance.GetDialog(DialogType.RewardedVideo);
            dialog.SetAmount(ConfigController.Config.rewardedVideoAmount);
            DialogController.instance.ShowDialog(dialog);
        }
        else
        {
            Debug.Log("No Reward");
        }
    }

    public void OnClickWinDialog()
    {
       // AdmobController.instance.ShowRewardBasedVideo();
        Sound.instance.PlayButton();
    }

    private void ShowTimerText(int time)
    {
        if (adAvailableTextHolder != null)
        {
            adAvailableTextHolder.SetActive(true);
            timerText.SetTime(time);
            timerText.Run();
        }
    }

   

    private void OnApplicationPause(bool pause)
    {
        if (!pause)
        {
            if (adAvailableTextHolder.activeSelf)
            {
                int remainTime = (int)(ConfigController.Config.rewardedVideoPeriod - CUtils.GetActionDeltaTime(ACTION_NAME));
                ShowTimerText(remainTime);
            }
        }
    }
}
