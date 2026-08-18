using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
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
            skinPath = Path.Combine(path, "assets", "modularwarfare", "skins"),
            iconPath = Path.Combine(path, "assets", "modularwarfare", "textures", "items"),
            assetsPath = Path.Combine(path, "assets", "modularwarfare"),
            glbPath = Path.Combine(path, "assets", "modularwarfare", "gltf"),
            objPath = Path.Combine(path, "assets", "modularwarfare", "obj"),
        };

        var pathTextures = Path.Combine(path, "textures");
        
        var pathArmors = Path.Combine(path, "armor");
        var pathArmorRenders = Path.Combine(path, "armor", "render");
        
        var pathGuns = Path.Combine(path, "guns");
        var pathGunRenders = Path.Combine(path, "guns", "render");

        var pathAtts = Path.Combine(path, "attachments");
        var pathAttRenders = Path.Combine(path, "attachments", "render");
        
        if (Directory.Exists(pathGuns)) {
            int count = 0;
            foreach (var file in ListFiles(pathGuns))
            {
                var type = JsonConvert.DeserializeObject<MWFTypeGun>(file.content);
                type.package = package;
                type.ParseJsonObject(JObject.Parse(file.content));
                type.path = file.path;
                package.AddConfig(type);
                count++;
            }
            logger.Info($"    加载TypeGun*{count}");
        }
        if (Directory.Exists(pathGunRenders)) {
            int count = 0;
            foreach (var file in ListFiles(pathGunRenders))
            {
                var render = JsonConvert.DeserializeObject<MWFRenderGun>(file.content);
                render.package = package;
                render.ParseJsonObject(JObject.Parse(file.content));
                render.path = file.path;
                render.InternalName = file.name.Replace(".render.json", "");
                var type = package.GetConfig<MWFTypeGun>(render.InternalName);
                if (type != null) {
                    type.configRender = render;
                    render.configType = type;
                }else { logger.Warn($"没有找到renderID为 {render.InternalName} 的Type 是否有同ID其他类型的配置文件: {package.GetConfig(render.InternalName) == null}"); }
                count++;
            }
            logger.Info($"    加载RenderGun*{count}");
        }
        
        if (Directory.Exists(pathAtts)) {
            int count = 0;
            foreach (var file in ListFiles(pathAtts))
            {
                var type = JsonConvert.DeserializeObject<MWFTypeAtt>(file.content);
                type.package = package;
                type.ParseJsonObject(JObject.Parse(file.content));
                type.path = file.path;
                package.AddConfig(type);
                count++;
            }
            logger.Info($"    加载TypeAtt*{count}");
        }
        if (Directory.Exists(pathAttRenders)) {
            int count = 0;
            foreach (var file in ListFiles(pathAttRenders))
            {
                var render = JsonConvert.DeserializeObject<MWFRenderAtt>(file.content);
                render.package = package;
                render.ParseJsonObject(JObject.Parse(file.content));
                render.path = file.path;
                render.InternalName = file.name.Replace(".render.json", "");
                var type = package.GetConfig<MWFTypeAtt>(render.InternalName);
                if (type != null) {
                    type.configRender = render;
                    render.configType = type;
                }else { logger.Warn($"没有找到renderID为 {render.InternalName} 的Type 是否有同ID其他类型的配置文件: {package.GetConfig(render.InternalName) == null}"); }

                count++;
            }
            logger.Info($"    加载RenderAtt*{count}");
        }
        
        if (Directory.Exists(pathArmors)) {
            int count = 0;
            foreach (var file in ListFiles(pathArmors))
            {
                var type = JsonConvert.DeserializeObject<MWFTypeArmor>(file.content);
                type.package = package;
                type.ParseJsonObject(JObject.Parse(file.content));
                type.path = file.path;
                package.AddConfig(type);
                count++;
            }
            logger.Info($"    加载TypeArmor*{count}");
        }
        if (Directory.Exists(pathArmorRenders)) {
            int count = 0;
            foreach (var file in ListFiles(pathArmorRenders))
            {
                var render = JsonConvert.DeserializeObject<MWFRenderArmor>(file.content);
                render.package = package;
                render.ParseJsonObject(JObject.Parse(file.content));
                render.path = file.path;
                render.InternalName = file.name.Replace(".render.json", "");
                var type = package.GetConfig<MWFTypeArmor>(render.InternalName);
                if (type != null) {
                    type.configRender = render;
                    render.configType = type;
                }else { logger.Warn($"没有找到renderID为 {render.InternalName} 的Type 是否有同ID其他类型的配置文件: {package.GetConfig(render.InternalName) == null}"); }

                count++;
            }
            logger.Info($"    加载RenderArmor*{count}");
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