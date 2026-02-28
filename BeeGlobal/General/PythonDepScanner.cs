using System;
using System.Diagnostics;
using System.IO;
using System.Text;

public static class PythonDepScanner
{
    public static void CheckToolFolder(string toolFolder, string pythonExe = "python")
    {
        toolFolder = Path.GetFullPath(toolFolder);
        if (!Directory.Exists(toolFolder))
            throw new DirectoryNotFoundException(toolFolder);

        // đảm bảo có _depcheck.py (nếu bạn đã tạo sẵn thì bỏ đoạn này cũng được)
        var checkerPath = Path.Combine(toolFolder, "_depcheck.py");
        if (!File.Exists(checkerPath))
            File.WriteAllText(checkerPath, DepCheckPyContent, new UTF8Encoding(false));

        // chạy python
        var psi = new ProcessStartInfo
        {
            FileName = pythonExe,
            Arguments = $"\"{checkerPath}\" \"{toolFolder}\"",
            WorkingDirectory = toolFolder,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true,
            StandardOutputEncoding = Encoding.UTF8,
            StandardErrorEncoding = Encoding.UTF8,
        };

        string stdout, stderr;
        using (var p = Process.Start(psi))
        {
            stdout = p.StandardOutput.ReadToEnd();
            stderr = p.StandardError.ReadToEnd();
            p.WaitForExit();
        }

        var logPath = Path.Combine(toolFolder, "PythonDependencyLog.txt");
        var sb = new StringBuilder();
        sb.AppendLine("========== PYTHON DEPENDENCY SCAN ==========");
        sb.AppendLine("Time: " + DateTime.Now);
        sb.AppendLine("ToolFolder: " + toolFolder);
        sb.AppendLine("PythonExe: " + pythonExe);
        sb.AppendLine();

        if (!string.IsNullOrWhiteSpace(stderr))
        {
            sb.AppendLine("----- STDERR -----");
            sb.AppendLine(stderr.Trim());
            sb.AppendLine();
        }

        sb.AppendLine("----- JSON RESULT (stdout) -----");
        sb.AppendLine(stdout?.Trim() ?? "");
        sb.AppendLine();

        File.WriteAllText(logPath, sb.ToString(), new UTF8Encoding(false));
    }

    // Python script ở trên, nhúng dạng string để auto tạo file _depcheck.py
    private static readonly string DepCheckPyContent =
@"import os, sys, ast, json, importlib.util, importlib

def is_local_module(tool_dir, modname):
    p1 = os.path.join(tool_dir, modname.replace('.', os.sep) + '.py')
    p2 = os.path.join(tool_dir, modname.replace('.', os.sep), '__init__.py')
    return os.path.isfile(p1) or os.path.isfile(p2)

def top_level(modname):
    return modname.split('.', 1)[0] if modname else modname

def collect_imports(py_path):
    with open(py_path, 'r', encoding='utf-8', errors='ignore') as f:
        src = f.read()
    tree = ast.parse(src, filename=py_path)

    mods = set()
    for node in ast.walk(tree):
        if isinstance(node, ast.Import):
            for a in node.names:
                if a.name:
                    mods.add(a.name)
        elif isinstance(node, ast.ImportFrom):
            if node.level and node.level > 0:
                continue
            if node.module:
                mods.add(node.module)
    return sorted(mods)

def try_import(mod, tool_dir):
    tl = top_level(mod)
    if is_local_module(tool_dir, tl):
        return True, None

    try:
        spec = importlib.util.find_spec(tl)
        if spec is None:
            return False, f'ModuleNotFound: {tl}'
        try:
            importlib.import_module(tl)
            return True, None
        except Exception as ex:
            return False, f'ImportError: {tl} -> {type(ex).__name__}: {ex}'
    except Exception as ex:
        return False, f'SpecError: {tl} -> {type(ex).__name__}: {ex}'

def main(tool_dir):
    tool_dir = os.path.abspath(tool_dir)
    sys.path.insert(0, tool_dir)

    items = []
    for root, _, files in os.walk(tool_dir):
        for fn in files:
            if fn.lower().endswith('.py') and fn != '_depcheck.py':
                items.append(os.path.join(root, fn))

    out = []
    for py in sorted(items):
        try:
            imports = collect_imports(py)
        except Exception as ex:
            out.append({'file': py, 'error': f'ParseError: {type(ex).__name__}: {ex}', 'missing': []})
            continue

        missing = []
        for m in imports:
            ok, err = try_import(m, tool_dir)
            if not ok:
                missing.append(err)

        out.append({'file': py, 'imports': imports, 'missing': missing})

    print(json.dumps(out, ensure_ascii=False))

if __name__ == '__main__':
    if len(sys.argv) < 2:
        print('[]')
    else:
        main(sys.argv[1])
";
}