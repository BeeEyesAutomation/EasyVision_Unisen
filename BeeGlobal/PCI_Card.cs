using Newtonsoft.Json.Linq;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace BeeGlobal
{
  public  enum PCI_Write
    {
        LightON,
        LightOFF,
        OK,
        NG,OFF
    }
    public class PCI_Card
    {
        public  short m_dev;
        public  bool Connect()
        {
            
            m_dev = DASK.Register_Card(6, 0);
            if (m_dev < 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public  bool Write(PCI_Write PCI_Write)
        {
            uint value = 128u;
            switch (PCI_Write)
            {
                case PCI_Write.LightON:
                    Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.INFO, "Light", "ON"));

                    value = 128u;
                    break;
                case PCI_Write.LightOFF:
                    Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.INFO, "Light", "OFF"));

                    value = 0u;
                    break;
                case PCI_Write.OK:
                    Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.INFO, "Light", "OK"));

                    value = 0u;
                    break;
                case PCI_Write.NG:
                    value = 256u;  
                    break;
                case PCI_Write.OFF:
                    Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.INFO, "Light", "OFF 2"));

                    value = 0;
                    break;
                   
            }    
            short ret = DASK.DO_WritePort((ushort)m_dev, 0, (uint)value);
            if (ret < 0)
            {  
                     
                    Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR,"Write", "Fail"));
               
                return false;
            }
            return true;
        }
        public  int iSensorOn = 0;
        public  Task<bool> Read()
        {
            return Task.Run(() =>
            {
             
            short ret=0;
                uint value = 0;
                try
                {
                    ret = DASK.DI_ReadPort((ushort)m_dev, 0, out value);
                }
                catch(Exception ex)
                {
                     Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR,"PCI", ex.Message));
                }
               
                if (ret < 0)
                {
                    return  false;
                }
                if (value == 1)
                {
                    iSensorOn = 1;
                return false;
                }
                if (iSensorOn == 1 && value == 0)
                {

                    iSensorOn = 0;
                    return true;
                }

            return false;

            });
        }
        private  Task _loopTask;
        private  CancellationTokenSource _loopCts;
        public  event Action<bool> OnBitsRead;
        public  bool StartReadLoop(int cycleMs = 1)
        {
            StopReadLoop();
            _loopCts = new CancellationTokenSource();
            var token = _loopCts.Token;

            _loopTask = Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                 try
                    {
                        //if (Global.IsAllowReadPLC)
                        {

                            
                            //val = true;
                        //  val= false;


                      bool  val = await Read();
                        //if(Global.ParaCommon==null)
                        //    Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "ParaCommon","null"));

                        //if (Global.Comunication.Protocol == null)
                        //    Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "Protocol", "null"));

                        //if (Global.IsRun && Global.Config.IsExternal)
                        //    if (Global.Comunication.Protocol. AddressInput[(int)I_O_Input.Trigger] != -1&&val==true)
                        //    {
                        //        int ix = Global.Comunication.Protocol.ParaBits.FindIndex(a => a.I_O_Input == I_O_Input.Trigger && a.TypeIO == TypeIO.Input);
                        //        if (ix >= 0)
                        //        {

                        //            Global.Comunication.Protocol.ParaBits[ix].Value = Convert.ToInt32(Convert.ToInt32(val));

                        //            Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.TRACE, "IO", " Trigger 1..."));
                        //            Global.TriggerInternal = false;
                        //            Global.IsAllowReadPLC = false;


                        //            switch (Global.TriggerNum)
                        //            {
                        //                case TriggerNum.Trigger0:
                        //                    Global.TriggerNum = TriggerNum.Trigger1;
                        //                    break;
                        //                case TriggerNum.Trigger1:
                        //                    Global.TriggerNum = TriggerNum.Trigger2;
                        //                    break;
                        //                case TriggerNum.Trigger2:
                        //                    Global.TriggerNum = TriggerNum.Trigger3;
                        //                    break;
                        //                case TriggerNum.Trigger3:
                        //                    Global.TriggerNum = TriggerNum.Trigger4;
                        //                    break;

                        //            }

                        //            Global.StatusProcessing = StatusProcessing.Trigger;
                        //            Global.Comunication.Protocol.IO_Processing = IO_Processing.Trigger;
                        //        }
                        //    }
                        var handler = OnBitsRead;
                            if (handler != null)
                                handler(val);
                          //  Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.INFO, "Read", val+""));

                        }

                    }
                    catch (Exception ex)
                    {

                        Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "PCI0", ex.Message));
                    }

                    await TimingUtils.DelayAccurateAsync(cycleMs < 50 ? 50 : cycleMs);
                }
            }, token);
            return true;
        }

        public  void StopReadLoop()
        {
            if (_loopCts == null) return;
            try { _loopCts.Cancel(); _loopTask?.Wait(800); } catch { }
            _loopTask = null;
            _loopCts = null;
        }
    }
}
