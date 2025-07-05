using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeCore.EtherNetIP
{
    public class EDS
    {
        public static void GenerateEDS(string filePath)
        {
            StringBuilder eds = new StringBuilder();
            eds.AppendLine("[File]");
            eds.AppendLine("DescText = \"EasyVision Adapter\"");
            eds.AppendLine("CreateDate = 03-07-2025");
            eds.AppendLine("Revision = 1.0");
            eds.AppendLine("EDSVersion = 1.0");

            eds.AppendLine("[Device]");
            eds.AppendLine("VendCode = 0x1234");
            eds.AppendLine("VendName = \"KAP\"");
            eds.AppendLine("ProdType = 12");
            eds.AppendLine("ProdCode = 1");
            eds.AppendLine("MajRev = 1");
            eds.AppendLine("MinRev = 0");
            eds.AppendLine("ProdName = \"VisionSensorAdapter\"");

            eds.AppendLine("[IO_Info]");
            eds.AppendLine("Default = CHI TU Owner");
            eds.AppendLine("Input_1 = 100, 8");
            eds.AppendLine("Output_1 = 150, 16");

            eds.AppendLine("[Params]");
            eds.AppendLine("Param1 = 1, \"Trigger\", USINT, RW, 0, 1, 0");
            eds.AppendLine("Param2 = 2, \"ReadyIn\", USINT, RW, 0, 1, 0");
            eds.AppendLine("Param3 = 3, \"ByPass\", USINT, RW, 0, 1, 0");
            eds.AppendLine("Param4 = 4, \"Program bit\", UINT, RW, 0, 50, 0");
            eds.AppendLine("Param5 = 5, \"ModeVision\", USINT, RW, 0, 1, 1");
            eds.AppendLine("Param6 = 6, \"Live\", USINT, RW, 0, 1, 0");
            eds.AppendLine("Param7 = 7, \"DelayTriger\", UINT, RW, 1, 200, 1");
            eds.AppendLine("Param8 = 8, \"DelayOutput\", UINT, RW, 1, 200, 1");
            eds.AppendLine("Param9 = 9, \"Result1\", USINT, RO, 0, 1, 0");
            eds.AppendLine("Param10 = 10, \"Result2\", USINT, RO, 0, 1, 0");
            eds.AppendLine("Param11 = 11, \"Result3\", USINT, RO, 0, 1, 0");
            eds.AppendLine("Param12 = 12, \"Result4\", USINT, RO, 0, 1, 0");
            eds.AppendLine("Param12 = 13, \"TotalResult\", USINT, RO, 0, 1, 0");
            eds.AppendLine("Param14 = 14, \"ReadyOut\", USINT, RO, 0, 1, 0");
            eds.AppendLine("Param15 = 15, \"Logic1\", USINT, RO, 0, 1, 0");
            eds.AppendLine("Param16 = 16, \"Logic2\", USINT, RO, 0, 1, 0");
            eds.AppendLine("Param17 = 17, \"Logic3\", USINT, RO, 0, 1, 0");
            eds.AppendLine("Param18 = 18, \"Logic4\", USINT, RO, 0, 1, 0");
            eds.AppendLine("Param19 = 19, \"Logic5\", USINT, RO, 0, 1, 0");
            eds.AppendLine("Param20 = 20, \"Logic6\", USINT, RO, 0, 1, 0");
            eds.AppendLine("Param21 = 21, \"Logic7\", USINT, RO, 0, 1, 0");
            eds.AppendLine("Param22 = 22, \"Logic8\", USINT, RO, 0, 1, 0");
            eds.AppendLine("Param23 = 23, \"Busy\", USINT, RO, 0, 1, 1");
            eds.AppendLine("Param24 = 24, \"Error\", UINT, RO, 0, 255, 0");
            eds.AppendLine("[Assembly]");
            eds.AppendLine("100 = \"Input Assembly\"");
            eds.AppendLine("100.Class = 0x04");
            eds.AppendLine("100.Instance = 100");
            eds.AppendLine("100.Size = 8");

            eds.AppendLine("150 = \"Output Assembly\"");
            eds.AppendLine("150.Class = 0x04");
            eds.AppendLine("150.Instance = 150");
            eds.AppendLine("150.Size = 16");

            File.WriteAllText(filePath, eds.ToString());
            Console.WriteLine($"EDS file generated: {filePath}");
        }
    }
}
