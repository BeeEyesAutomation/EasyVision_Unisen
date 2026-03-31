#include "YoloDebugLog.h"
#include <cstdarg>
#include <cstdio>
#include <sstream>
#include <iomanip>

namespace BeeLog
{
    static std::mutex g_mtx;
    static bool g_enabled = true;
    static std::string g_file = "yolo_debug.log";
    static thread_local std::vector<std::string> g_scope;

    static const char* LvStr(Level lv)
    {
        switch (lv)
        {
        case Level::Info: return "INF";
        case Level::Warn: return "WRN";
        case Level::Error:return "ERR";
        default: return "UNK";
        }
    }

    std::string NowString()
    {
        using namespace std::chrono;
        auto tp = system_clock::now();
        auto tt = system_clock::to_time_t(tp);
        auto ms = duration_cast<milliseconds>(tp.time_since_epoch()).count() % 1000;

        std::tm tmv{};
#if defined(_WIN32)
        localtime_s(&tmv, &tt);
#else
        localtime_r(&tt, &tmv);
#endif
        std::ostringstream oss;
        oss << std::put_time(&tmv, "%Y-%m-%d %H:%M:%S")
            << "." << std::setw(3) << std::setfill('0') << ms;
        return oss.str();
    }

    void SetEnabled(bool on) { g_enabled = on; }
    bool IsEnabled() { return g_enabled; }

    void SetFile(const std::string& filePath)
    {
        std::lock_guard<std::mutex> lk(g_mtx);
        g_file = filePath;
    }

    static void AppendLine(Level lv, const std::string& line)
    {
        if (!g_enabled) return;

        std::lock_guard<std::mutex> lk(g_mtx);
        std::ofstream ofs(g_file, std::ios::app);
        if (!ofs.is_open()) return;

        ofs << NowString() << " [" << LvStr(lv) << "] ";
        // scope prefix
        for (size_t i = 0; i < g_scope.size(); i++)
        {
            ofs << g_scope[i];
            if (i + 1 < g_scope.size()) ofs << ">";
        }
        if (!g_scope.empty()) ofs << " ";
        ofs << line << "\n";
    }

    void Write(Level lv, const std::string& msg)
    {
        AppendLine(lv, msg);
    }

    void Writef(Level lv, const char* fmt, ...)
    {
        char buf[2048];
        va_list ap;
        va_start(ap, fmt);
#if defined(_WIN32)
        vsnprintf_s(buf, sizeof(buf), _TRUNCATE, fmt, ap);
#else
        vsnprintf(buf, sizeof(buf), fmt, ap);
#endif
        va_end(ap);
        AppendLine(lv, buf);
    }

    void DumpFloats(const char* tag, const float* p, int count)
    {
        if (!g_enabled || !p || count <= 0) return;
        std::ostringstream oss;
        oss << tag << " [";
        int n = count;
        if (n > 32) n = 32; // limit to keep log small
        for (int i = 0; i < n; i++)
        {
            oss << p[i];
            if (i + 1 < n) oss << ", ";
        }
        if (count > n) oss << ", ...";
        oss << "]";
        AppendLine(Level::Info, oss.str());
    }

    Timer::Timer(const char* n) : name(n), t0(std::chrono::high_resolution_clock::now())
    {
        if (g_enabled) g_scope.push_back(n ? n : "scope");
    }

    Timer::~Timer()
    {
        auto t1 = std::chrono::high_resolution_clock::now();
        double ms = std::chrono::duration<double, std::milli>(t1 - t0).count();
        if (g_enabled)
        {
            std::string n = g_scope.empty() ? (name ? name : "scope") : g_scope.back();
            AppendLine(Level::Info, n + " = " + std::to_string(ms) + " ms");
            if (!g_scope.empty()) g_scope.pop_back();
        }
    }
}
