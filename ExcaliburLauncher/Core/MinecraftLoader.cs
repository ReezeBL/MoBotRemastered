using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace ExcaliburLauncher.Core
{
    internal static class MinecraftLoader
    {
        public static void StartJavaProcess(string javaPath, ExcaliburAuth.AuthData authData,
            ExcaliburAuth.ServerConfig serverConfig)
        {
            var process = new Process();
            var startInfo = new ProcessStartInfo
            {
                FileName = Path.Combine(javaPath, "java.exe"),
                WorkingDirectory = serverConfig.Directory,
                Arguments = GetArguments(serverConfig, authData),
                CreateNoWindow = true,
                //RedirectStandardOutput = true,
                //UseShellExecute = false,
            };

            process.StartInfo = startInfo;
            //process.OutputDataReceived += (sender, args) => Console.WriteLine("received output: {0}", args.Data);
            process.Start();
            //process.BeginOutputReadLine();
        }

        private static string GetArguments(ExcaliburAuth.ServerConfig serverConfig, ExcaliburAuth.AuthData authData)
        {
            var folder = serverConfig.Directory;

            var javaLibPath = "-Djava.library.path=\"" + folder + "/bin/natives\"";
            var extraArguments = serverConfig.ExtraArguments.Replace("@RAM@", "2048M");
            var unknownShit = "-XX:+UseConcMarkSweepGC -XX:+CMSIncrementalMode -XX:-UseAdaptiveSizePolicy";
            var arguments = serverConfig.Arguments.Replace("@SESSION@", authData.Session).Replace("@USER@", authData.Username);
            var version = serverConfig.Version;
            var mainClass = serverConfig.MainClass;
            var classPath = new StringBuilder();
            Array.ForEach(serverConfig.ClassPath.ToArray(), e =>
                classPath.Append($"{folder}/{e};"));
            //C:\Users\Шилкин\AppData\Roaming\exjava\jvm\bin\java.exe -Xmx1024m -Dfml.ignoreInvalidMinecraftCertificates=true -Dfml.ignorePatchDiscrepancies=true -Djava.library.path=C:\Users\Шилкин\AppData\Roaming\.exclient\hitech\bin\natives -cp C:\Users\Шилкин\AppData\Roaming\.exclient\hitech\bin\exblforforge.jar;C:\Users\Шилкин\AppData\Roaming\.exclient\hitech\bin\exblforge.jar;C:\Users\Шилкин\AppData\Roaming\.exclient\hitech\bin\exauth.jar;C:\Users\Шилкин\AppData\Roaming\.exclient\hitech\bin\liteloader.jar;C:\Users\Шилкин\AppData\Roaming\.exclient\hitech\bin\excomplet.jar;C:\Users\Шилкин\AppData\Roaming\.exclient\hitech\bin\hitech.jar net.minecraft.launchwrapper.Launch --accessToken 3f564062e975286ae1edca1609b77a18 --username Siamant --session 3f564062e975286ae1edca1609b77a18 --tweakClass cpw.mods.fml.common.launcher.FMLTweaker --tweakClass com.mumfrey.liteloader.launch.LiteLoaderTweaker --gameDir C:\Users\Шилкин\AppData\Roaming\.exclient\hitech --version 1.7.10 --assetsDir C:\Users\Шилкин\AppData\Roaming\.exclient\hitech\assets --uuid 780e469a6f64172c834a8d2de068918c --userProperties {} --assetIndex 1.7.10
            string javaParams = $"{extraArguments} {unknownShit} {javaLibPath} -cp \"{classPath}\" {mainClass} {arguments} --version {version} --gameDir {folder} --assetsDir {folder}/assets --uuid {authData.Uuid} --userProperties {{}} --assetIndex {version}";
            return javaParams;
        }
    }
}