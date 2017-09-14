﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AssemblyCSharp;
using UnityEngine.UI;
using LitJson;

public class CreateXiazuiScript : MonoBehaviour {

    public GameObject xiazuiSelectList;
    public List<Toggle> xiazuiList; // 下嘴组合的列表  0.不下嘴   1. 147     2. 258    3. 369
    public List<Toggle> beishu;  // 1.  5倍   2.   10倍    3. 20倍

    private XiazuiVO xiazuiVO; //创建下嘴信息
	
	void Start () {
        addListener();
    }

    public void createXiazui()
    {
        int XZList = 0;  // 下嘴的组合  147  258  369
        int XZbeishu = 0;  // 下嘴的倍数  5  10   20
        for(int i=0; i<xiazuiList.Count;i++)
        {
            Toggle item = xiazuiList[i];
            if(item.isOn)
            {
                if(i == 0 ){
                    XZList = 1;   // 表示 147 组合
                }
                if(i == 1) {
                    XZList = 2;   // 表示 258 组合
                }
                if(i ==2){
                    XZList = 3;    // 表示 369组合
                }
                break;
            }
        }

        for(int i=0;i<beishu.Count;i++)
        {
            Toggle bs = beishu[1];
            if(bs.isOn)
            {
                if(i== 0){
                    XZbeishu = 5;  // 5倍
                }
                if (i == 1){
                    XZbeishu = 10;   // 10倍
                }
                if (i == 2) {
                    XZbeishu = 20;    // 20倍
                }
            }
        }

        ReadyVO readyVO = new ReadyVO();
        readyVO.phase = 1;
        xiazuiVO = new XiazuiVO();
        xiazuiVO.xiazuiList = XZList;
        xiazuiVO.xiazuiMultiple = XZbeishu;
        string sendmsg = JsonMapper.ToJson(xiazuiVO);
        //if(GlobalDataScript.xiazui == true){
            //xiazuiSelect.SetActive(true);
            CustomSocket.getInstance().sendMsg(new GameReadyRequest(readyVO));
            CustomSocket.getInstance().sendMsg(new XiazuiRequest(xiazuiVO));
        //}
        //else{
        //    TipsManagerScript.getInstance().setTips("没选择下嘴，没有额外收益可能");
        //}
    }

    public void onXiazuiCallback(ClientResponse Xiazuirespone)
    {
        MyDebug.Log("   wxd>>>  " + Xiazuirespone.message);
        if (Xiazuirespone.status == 1){
            print(xiazuiVO);
            GlobalDataScript.xiazuiVo = xiazuiVO;

            GlobalDataScript.loginResponseData.main = true;
            GlobalDataScript.loginResponseData.isOnLine = true;
            GlobalDataScript.reEnterRoomData = null;

            GlobalDataScript.gamePlayPanel = PrefabManage.loadPerfab("Prefab/xiazui_btnList");
        }
        else {
            TipsManagerScript.getInstance().setTips(Xiazuirespone.message);
        }
    }

    public void onStartXiazuiCallback(ClientResponse StartXiazuiRespone)
    {
        print("   wxd>>>   onStartXiazuiCallback");
    }

    public void gameReadyNotice(ClientResponse response) //todu 时机不对
    {
    }



    public void addListener()
    {
        SocketEventHandle.getInstance().gameReadyNotice += gameReadyNotice; //todu 时机不对
        SocketEventHandle.getInstance().XiazuiCallBack += onXiazuiCallback;
        SocketEventHandle.getInstance().StartXiazuiCallBack += onStartXiazuiCallback; //todu 时机不对

    }

    private void removeListener() // todo 需要调用
    {
        SocketEventHandle.getInstance().gameReadyNotice -= gameReadyNotice; //todu 时机不对
        SocketEventHandle.getInstance().XiazuiCallBack -= onXiazuiCallback;
        SocketEventHandle.getInstance().StartXiazuiCallBack += onStartXiazuiCallback; //todu 时机不对
    }
}