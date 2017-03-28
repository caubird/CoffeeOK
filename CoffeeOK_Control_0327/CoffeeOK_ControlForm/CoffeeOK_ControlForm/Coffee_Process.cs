﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoffeeOK_ControlForm
{
    class Coffee_Process
    {

        //工序列表
        //####################################################################################
        //工步1：取咖啡杯
        //工步2：取杯器动作，落杯
        //工步3：放置咖啡杯到咖啡机。
        //工步4：点击拿铁。
        //工步5：点击卡普奇诺。
        //工步6：点击美式咖啡。
        //工步7：点击意式浓缩。
        //工步8：从咖啡机取做好的咖啡杯到放调料位置。
        //工步9：从咖啡机取做好的咖啡杯到出货窗口位置。
        //工步10：加糖
        //工步11：加半糖
        //工步12：加巧克力酱
        //工步13：将咖啡杯从调料窗口移至出货窗口位置。
        //工步14：出货窗口转动出货
        //####################################################################################

        private int[] Process_Latte = { 0, 1, 2, 3, 7, 9, 12, 13,};
        private int[] Process_Latte_HalfSugar = { 0, 1, 2, 3, 7, 10, 12, 13,};
        private int[] Process_Latte_NoSugar = { 0, 1, 2, 3, 8, 13,};
        private int[] Process_Black = { 0, 1, 2, 6, 8, 13,};
        private int[] Process_AmaricanBlack = { 0, 1, 2, 5, 8, 13,};
        private int[] Process_Mocha = { 0, 1, 2, 3, 7, 9, 11, 12, 13,};
        private int[] Process_Mocha_HalfSugar = { 0, 1, 2, 3, 7, 10, 11, 12, 13,};
        private int[] Process_Cappuccino = { 0, 1, 2, 4, 7, 9, 12, 13,};
        private int[] Process_Cappuccino_HalfSugar = { 0, 1, 2, 4, 7, 10, 12, 13,};
        private int[] Process_Cappuccino_NoSugar = { 0, 1, 2, 4, 8, 13,};
        private int[] ThoughCoffee = { 14};

        public Coffee_Procedure[] ProcedureTable = new Coffee_Procedure[15];

        public string[] ProcedureNameList = {
            "PickACup",
            "DropACup",
            "PutCupToCoffeeMachine",
            "PressLatte",
            "PressCappuccino",
            "PressAmaricanBlack",
            "PressBlack",
            "PutCupToAddSugarPossition",
            "PutCupToPickUpPossition",
            "AddSugar",
            "AddHalfSugar",
            "Addchocolate",
            "PutCoffeeWithSugarToPickUpPossiton",
            "PushOutCoffee",
            "DiscardCoffee"
        };

        public Coffee_Process()
        {
            ProcessInit();
        }

        public void ProcessInit()
        {
            ProcedureTable[0] = new Coffee_Procedure("PickACup", 100, 1);                   //机器人伸手取杯，等待100ms
            ProcedureTable[1] = new Coffee_Procedure("DropACup", 2000, 1);                  //等待2s等待落杯
            ProcedureTable[2] = new Coffee_Procedure("PutCupToCoffeeMachine", 500, 1);      //将杯子放到咖啡机上
            ProcedureTable[3] = new Coffee_Procedure("PressLatte", 60000, 1);               //等待1分钟等待做好咖啡
            ProcedureTable[4] = new Coffee_Procedure("PressCappuccino", 90000, 1);          //等待1分钟等待做好咖啡
            ProcedureTable[5] = new Coffee_Procedure("PressAmaricanBlack", 65000, 1);       //等待1分钟等待做好咖啡
            ProcedureTable[6] = new Coffee_Procedure("PressBlack", 90000, 1);               //等待1分钟等待做好咖啡
            ProcedureTable[7] = new Coffee_Procedure("PutCupToAddSugarPossition", 500, 1);  //将咖啡杯放到加糖位置
            ProcedureTable[8] = new Coffee_Procedure("PutCupToPickUpPossition", 500, 1);    //将咖啡杯从咖啡机放到取杯位置
            ProcedureTable[9] = new Coffee_Procedure("AddSugar", 15000, 1);                 //加糖，等待15s加糖完毕
            ProcedureTable[10] = new Coffee_Procedure("AddHalfSugar", 13000, 1);            //加半糖，等待14s加半糖完毕
            ProcedureTable[11] = new Coffee_Procedure("Addchocolate", 10000, 1);            //等待15s加入巧克力
            ProcedureTable[12] = new Coffee_Procedure("PutCoffeeWithSugarToPickUpPossiton", 1000, 1);//将杯子放到取杯位置
            ProcedureTable[13] = new Coffee_Procedure("PushOutCoffee", 1000, 1);            //将杯子出杯给下单客户
            ProcedureTable[14] = new Coffee_Procedure("DiscardCoffee", 500, 1);                //把做好的咖啡丢掉
        }

        public int[] GetProcedure(int processid)
        {
            switch (processid)
            {
                case 5://拿铁加糖
                    return Process_Latte;
                    break;
                case 8://拿铁半塘
                    return Process_Latte_HalfSugar;
                    break;
                case 15://拿铁无糖
                    return Process_Latte_NoSugar;
                    break;
                case 10://浓缩 无糖
                    return Process_Black;
                    break;
                case 14://美式 无糖
                    return Process_AmaricanBlack;
                    break;
                case 6://普通摩卡，加糖
                    return Process_Mocha;
                    break;
                case 12://摩卡，半糖
                    return Process_Mocha_HalfSugar;
                    break;
                case 18://卡布奇诺
                    return Process_Cappuccino;
                    break;
                case 16://卡布奇诺半糖
                    return Process_Cappuccino_HalfSugar;
                    break;
                case 17://卡布奇诺 无糖
                    return Process_Cappuccino_NoSugar;
                    break;
                default:
                    //出错，记录日志
                    break;
            }

            return null;
        }

    }

    class Coffee_Procedure
    {
        public String ProcedureName = "";
        public int WaitTimeAfterWork = 0;
        public int ProcedureNumber = 0;
        public Coffee_Procedure(string name, int waittime, int number)
        {
            ProcedureName = name;
            WaitTimeAfterWork = waittime;
            ProcedureNumber = number;
        }
    }
}
