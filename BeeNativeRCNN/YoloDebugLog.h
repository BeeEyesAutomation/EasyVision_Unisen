#pragma once
#include <string>
#include <vector>
#include <mutex>
#include <fstream>
#include <chrono>

namespace BeeLog
{
    enum class Level { Info, Warn, Error };

    struct Timer
    {
        const char* name;
        std::chrono::high_resolution_clock::time_point t0;
        Timer(const char* n);
        ~Timer();
    };

    // Enable/disable runtime
    void SetEnabled(bool on);
    bool IsEnabled();

    // Set log file path (UTF-8)
    void SetFile(const std::string& filePath);

    // Basic logging
    void Write(Level lv, const std::string& msg);
    void Writef(Level lv, const char* fmt, ...);

    // Helper: dump first N floats for quick sanity
    void DumpFloats(const char* tag, const float* p, int count);

    // Helper: dump tensor info (shape + elem type) - you can call with ov::Tensor outside by formatting string
    std::string NowString();
}
