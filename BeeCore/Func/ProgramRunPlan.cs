using System.Collections.Generic;
using BeeGlobal;

namespace BeeCore
{
    public sealed class ProgramRunUnit
    {
        public int ProgramIndex;   // index trong PropetyTools
        public int CameraIndex;    // slot trong listCamera
    }

    public static class ProgramRunPlanner
    {
        public static List<ProgramRunUnit> Build(TriggerNum trig)
        {
            var list = new List<ProgramRunUnit>();
            var cfg = Global.Config;
            if (cfg == null)
            {
                list.Add(new ProgramRunUnit { ProgramIndex = 0, CameraIndex = 0 });
                return list;
            }

            int numProg = Clamp(cfg.NumProgram, 1, 4);

            switch (cfg.ProcessingMode)
            {
                case ProcessingMode.SingleProgramSingleCamera:
                    list.Add(new ProgramRunUnit { ProgramIndex = 0, CameraIndex = 0 });
                    break;

                case ProcessingMode.MultiProgramSingleCamera:
                    for (int p = 0; p < numProg; p++)
                        list.Add(new ProgramRunUnit { ProgramIndex = p, CameraIndex = 0 });
                    break;

                case ProcessingMode.MultiProgramMultiCamera:
                    for (int p = 0; p < numProg; p++)
                    {
                        int cam = (cfg.ProgramCameraMap != null && p < cfg.ProgramCameraMap.Length)
                                  ? cfg.ProgramCameraMap[p] : p;
                        list.Add(new ProgramRunUnit { ProgramIndex = p, CameraIndex = cam });
                    }
                    break;

                default:
                    list.Add(new ProgramRunUnit { ProgramIndex = 0, CameraIndex = 0 });
                    break;
            }
            return list;
        }

        // Build kế hoạch theo 1 trigger PLC duy nhất (giữ tương thích PLC-driven).
        public static ProgramRunUnit BuildSingle(TriggerNum trig)
        {
            var cfg = Global.Config;
            int prog = 0;
            switch (trig)
            {
                case TriggerNum.Trigger2: prog = 1; break;
                case TriggerNum.Trigger3: prog = 2; break;
                case TriggerNum.Trigger4: prog = 3; break;
            }
            int cam = 0;
            if (cfg != null && cfg.ProcessingMode == ProcessingMode.MultiProgramMultiCamera
                && cfg.ProgramCameraMap != null && prog < cfg.ProgramCameraMap.Length)
                cam = cfg.ProgramCameraMap[prog];
            return new ProgramRunUnit { ProgramIndex = prog, CameraIndex = cam };
        }

        private static int Clamp(int v, int lo, int hi) => v < lo ? lo : (v > hi ? hi : v);
    }
}
