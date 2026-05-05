using BeeCore;
using BeeGlobal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;

namespace BeeCore.Persistence.Tests
{
    internal static class Program
    {
        private static readonly string[] FrozenPropetyToolKeys =
        {
            "_disposed",
            "Propety",
            "<Propety2>k__BackingField",
            "IndexCamera",
            "Name",
            "TypeTool",
            "IndexLogics",
            "UsedTool",
            "IsSendResult",
            "IndexImgRegis",
            "_Score",
            "_Percent",
            "Location",
            "CycleTime",
            "ScoreResult",
            "MinValue",
            "MaxValue",
            "StepValue",
            "_StatusTool",
            "Results"
        };

        private static int Main()
        {
            var tests = new Action[]
            {
                LoadProgram_ReturnsSameToolCount,
                RoundTrip_PreservesPropetyToolFrozenKeys,
                RoundTrip_PreservesScore
            };

            int failed = 0;
            foreach (Action test in tests)
            {
                try
                {
                    test();
                    Console.WriteLine("PASS " + test.Method.Name);
                }
                catch (Exception ex)
                {
                    failed++;
                    Console.Error.WriteLine("FAIL " + test.Method.Name + ": " + ex.Message);
                    Console.Error.WriteLine(ex);
                }
            }

            return failed == 0 ? 0 : 1;
        }

        private static void LoadProgram_ReturnsSameToolCount()
        {
            List<List<PropetyTool>> expected = CreateSyntheticProgram();
            List<List<PropetyTool>> actual = SaveAndLoad(expected);

            AssertEqual(expected.Count, actual.Count, "program count");
            AssertEqual(expected[0].Count, actual[0].Count, "tool count");
        }

        private static void RoundTrip_PreservesPropetyToolFrozenKeys()
        {
            PropetyTool original = CreateTool(new Circle(), TypeTool.Circle, "Circle", 0, 73.5f);
            PropetyTool roundTripped = SaveAndLoad(new List<List<PropetyTool>>
            {
                new List<PropetyTool> { original }
            })[0][0];

            var info = new SerializationInfo(typeof(PropetyTool), new FormatterConverter());
            roundTripped.GetObjectData(info, new StreamingContext(StreamingContextStates.All));

            HashSet<string> actualKeys = new HashSet<string>();
            foreach (SerializationEntry entry in info)
                actualKeys.Add(entry.Name);

            foreach (string frozenKey in FrozenPropetyToolKeys)
                AssertTrue(actualKeys.Contains(frozenKey), "missing frozen key: " + frozenKey);
        }

        private static void RoundTrip_PreservesScore()
        {
            const float score = 88.25f;
            PropetyTool original = CreateTool(new Width(), TypeTool.Width, "Width", 1, score);
            PropetyTool roundTripped = SaveAndLoad(new List<List<PropetyTool>>
            {
                new List<PropetyTool> { original }
            })[0][0];

            AssertEqual(score, roundTripped.Score, "score");
        }

        private static List<List<PropetyTool>> CreateSyntheticProgram()
        {
            return new List<List<PropetyTool>>
            {
                new List<PropetyTool>
                {
                    CreateTool(new Circle(), TypeTool.Circle, "Circle", 0, 81f),
                    CreateTool(new Width(), TypeTool.Width, "Width", 1, 82f),
                    CreateTool(new Patterns(), TypeTool.Pattern, "Pattern", 2, 83f),
                    CreateTool(new Yolo(), TypeTool.Learning, "Learning", 3, 84f)
                }
            };
        }

        private static PropetyTool CreateTool(dynamic payload, TypeTool typeTool, string name, int index, float score)
        {
            payload.Index = index;
            payload.IndexThread = 0;

            return new PropetyTool
            {
                Propety = payload,
                Propety2 = payload,
                TypeTool = typeTool,
                Name = name,
                Score = score,
                MinValue = 0,
                MaxValue = 100,
                StepValue = 1,
                Results = Results.None,
                StatusTool = StatusTool.WaitCheck,
                IndexLogics = new bool[6]
            };
        }

        private static List<List<PropetyTool>> SaveAndLoad(List<List<PropetyTool>> program)
        {
            string dir = Path.Combine(Path.GetTempPath(), "BeeCore.Persistence.Tests", Guid.NewGuid().ToString("N"));
            string path = Path.Combine(dir, "sample.prog");

            try
            {
                Access.SaveProg(path, program);
                return Access.LoadProg(path);
            }
            finally
            {
                try
                {
                    if (Directory.Exists(dir))
                        Directory.Delete(dir, true);
                }
                catch
                {
                }
            }
        }

        private static void AssertEqual<T>(T expected, T actual, string label)
        {
            if (!object.Equals(expected, actual))
                throw new InvalidOperationException(label + " expected " + expected + " but was " + actual);
        }

        private static void AssertTrue(bool condition, string message)
        {
            if (!condition)
                throw new InvalidOperationException(message);
        }
    }
}
