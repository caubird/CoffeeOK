using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoffeeOK_ControlForm
{
    class VirtualRobot
    {
        System.Windows.Forms.Timer Maintimer = new System.Windows.Forms.Timer();
        public VirtualRobot()
        {
            Maintimer.Tick += Maintimer_Tick;
            Maintimer.Interval = 100;//100ms
        }

        private void Maintimer_Tick(object sender, EventArgs e)
        {
            //throw new NotImplementedException();

        }
    }
}
