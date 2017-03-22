using System;
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

        private int[] ProcedureList_Latte = { 1, 2, 3, 4, 8, 10, 13, 14 };
        private int[] ProcedureList_Latte_HalfShuger = { 1, 2, 3, 4, 8, 11, 13, 14 };
        private int[] ProcedureList_Latte_NoShuger = { 1, 2, 3, 4, 9, 14 };
        private int[] ProcedureList_Black = { 1, 2, 3, 7, 9, 14 };
        private int[] ProcedureList_AmaricanBlack = { 1, 2, 3, 6, 9, 14 };
        private int[] ProcedureList_Mocha = { 1, 2, 3, 4, 8, 10, 12, 13, 14 };
        private int[] ProcedureList_Mocha_HalfShuger = { 1, 2, 3, 4, 8, 11, 12, 13, 14 };
        private int[] ProcedureList_Cappuccino = { 1, 2, 3, 5, 8, 10, 13, 14 };
        private int[] ProcedureList_Cappuccino_HalfShuger = { 1, 2, 3, 5, 8, 11, 13, 14 };
        private int[] ProcedureList_Cappuccino_NoShuger = { 1, 2, 3, 5, 9, 14 };

        private void ProcessInit()
        {
            //throw new NotImplementedException();
            //TaskList_Latte.Insert(0, 1);

        }
        public int[] GetProcedure(int processid)
        {
            switch (processid)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 6:
                    break;
                case 7:
                    break;
                case 8:
                    break;
                case 9:
                    break;
                case 10:
                    break;
            }

            return null;
        }

    }
}
