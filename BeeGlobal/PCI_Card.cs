using Newtonsoft.Json.Linq;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
        public static short m_dev;
        public static bool Connect()
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
        public static bool Write(PCI_Write PCI_Write)
        {
            uint value = 128u;
            switch (PCI_Write)
            {
                case PCI_Write.LightON:
                    value = 128u;
                    break;
                case PCI_Write.LightOFF:
                    value = 0u;
                    break;
                case PCI_Write.OK:
                    value = 0u;
                    break;
                case PCI_Write.NG:
                    value = 256u;  
                    break;
                case PCI_Write.OFF:
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
        public static int iSensorOn = 0;
        public static Task<bool> Read()
        {
            return Task.Run(() =>
            {
             
            short ret;
                ret = DASK.DI_ReadPort((ushort)m_dev, 0, out var int_value);
                if (ret < 0)
                {
                    return  false;
                }
                if (int_value == 1)
                {
                    iSensorOn = 1;
                return false;
                }
                if (iSensorOn == 1 && int_value == 0)
                {

                    iSensorOn = 0;
                    return true;
                }

            return false;

            });
        }
        private static Task _loopTask;
        private static CancellationTokenSource _loopCts;
        public static event Action<bool> OnBitsRead;
        public static bool StartReadLoop(int cycleMs = 500)
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
                        if (Global.IsAllowReadPLC)
                        {
                          bool val = await Read();
                            
                            var handler = OnBitsRead;
                            if (handler != null) handler(val);
                        }

                    }
                    catch (Exception ex)
                    {

                        Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "ReadPLC", "Fail Read"));
                    }

                    await TimingUtils.DelayAccurateAsync(cycleMs < 50 ? 50 : cycleMs);
                }
            }, token);
            return true;
        }

        public static void StopReadLoop()
        {
            if (_loopCts == null) return;
            try { _loopCts.Cancel(); _loopTask?.Wait(800); } catch { }
            _loopTask = null;
            _loopCts = null;
        }
    }
}
