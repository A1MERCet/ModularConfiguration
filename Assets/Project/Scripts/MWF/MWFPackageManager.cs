using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class MWFPackageManager: SingletonMono<MWFPackageManager>
{
    public LoggerProxy logger = new LoggerProxy("[MWF包管理器]");
    
    private List<MWFPackage> packages = new();
    public List<MWFPackage> Packages => packages;

    public MWFPackage LoadPackage(string path) {
        if (!Directory.Exists(path)) { logger.Error($"找不到路径 {path}"); return null; }
        
        logger.Info($"正在加载包: {Path.GetFileName(path)} {path}");
        
        MWFPackage package = new MWFPackage() {
            name = Path.GetFileName(path),
            path = path,
            assetsPath = Path.Combine(path, "assets", "modularwarfare"),
            glbPath = Path.Combine(path, "assets", "modularwarfare", "gltf"),
        };

        var pathGuns = Path.Combine(path, "guns");
        var pathGunRenders = Path.Combine(path, "guns", "render");

        if (Directory.Exists(pathGuns)) {
            int count = 0;
            foreach (var file in ListFiles(pathGuns))
            {
                var type = JsonUtility.FromJson<MWFTypeGun>(file.content);
                type.ParseJsonObject(JObject.Parse(file.content));
                type.path = file.path;
                package.Types.Add(type);
                count++;
            }
            logger.Info($"    加载TypeGun*{count}");
        }
        if (Directory.Exists(pathGunRenders)) {
            int count = 0;
            foreach (var file in ListFiles(pathGunRenders))
            {
                var render = JsonUtility.FromJson<MWFRenderGun>(file.content);
                render.ParseJsonObject(JObject.Parse(file.content));
                render.path = file.path;
                render.internalName = file.name.Replace(".render.json", "");
                package.Renders.Add(render);
                count++;
            }
            logger.Info($"    加载RenderGun*{count}");
        }

        packages.Add(package);
        return package;
    }

    public struct FileContent
    {
        public string name;
        public string path;
        public string content;
    }
    public List<FileContent> ListFiles(string path, string ext = "*.json", SearchOption searchOption = SearchOption.TopDirectoryOnly)
    {
        string[] files = Directory.GetFiles(path, ext, searchOption);
        List<FileContent> fileContents = new();
        
        foreach (string f in files)
            fileContents.Add(new FileContent() {
                    name = Path.GetFileName(f),
                    path = f,
                    content = File.ReadAllText(f)
                });
        return fileContents;
    }
}