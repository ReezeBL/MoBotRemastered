using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Input;
using ExcaliburLauncher.Core;
using ExcaliburLauncher.GUI.Viewers;

namespace ExcaliburLauncher.GUI.Commands
{
    internal class ConnectCommand : ICommand
    {
        private readonly MainWindowView view;
        public ConnectCommand(MainWindowView view)
        {
            this.view = view;
        }

        public bool CanExecute(object parameter)
        {
            //return view.SelectedConfig != null && !string.IsNullOrEmpty(view.Username) && !string.IsNullOrEmpty(view.Password);
            return true;
        }

        public async void Execute(object parameter)
        {
            //var authParams = await ExcaliburAuth.GetAuthSession(view.Username, view.Password);
            //Load(view.SelectedConfig, authParams);

            var process = new Process();
            var startInfo = new ProcessStartInfo
            {
                FileName = Path.Combine(view.JavaPath, "java.exe"),
                //WorkingDirectory = view.WorkingDirectory,
                //Arguments = GetArguments(view.SelectedConfig, authParams),
                //CreateNoWindow = true,
                //RedirectStandardOutput = true,
                //UseShellExecute = false,
            };

            process.StartInfo = startInfo;
            //process.OutputDataReceived += (sender, args) => Console.WriteLine("received output: {0}", args.Data);
            process.Start();
            //process.BeginOutputReadLine();
        }

        private string GetArguments(ExcaliburAuth.ServerConfig serverConfig, ExcaliburAuth.AuthData authData)
        {
            var folder = serverConfig.Directory;

            string javaLibPath = "-Djava.library.path=\"" + folder + "/bin/natives\"";
            string extraArguments = serverConfig.ExtraArguments.Replace("@RAM@", "2048M");
            string unknownShit = "-XX:+UseConcMarkSweepGC -XX:+CMSIncrementalMode -XX:-UseAdaptiveSizePolicy";
            string arguments = serverConfig.Arguments.Replace("@SESSION@", authData.Session).Replace("@USER@", authData.Username);
            string version = serverConfig.Version;
            string mainClass = serverConfig.MainClass;
            StringBuilder classPath = new StringBuilder();
            Array.ForEach(serverConfig.ClassPath.ToArray(), e =>
                classPath.Append($"{folder}/{e};"));
            //C:\Users\Шилкин\AppData\Roaming\exjava\jvm\bin\java.exe -Xmx1024m -Dfml.ignoreInvalidMinecraftCertificates=true -Dfml.ignorePatchDiscrepancies=true -Djava.library.path=C:\Users\Шилкин\AppData\Roaming\.exclient\hitech\bin\natives -cp C:\Users\Шилкин\AppData\Roaming\.exclient\hitech\bin\exblforforge.jar;C:\Users\Шилкин\AppData\Roaming\.exclient\hitech\bin\exblforge.jar;C:\Users\Шилкин\AppData\Roaming\.exclient\hitech\bin\exauth.jar;C:\Users\Шилкин\AppData\Roaming\.exclient\hitech\bin\liteloader.jar;C:\Users\Шилкин\AppData\Roaming\.exclient\hitech\bin\excomplet.jar;C:\Users\Шилкин\AppData\Roaming\.exclient\hitech\bin\hitech.jar net.minecraft.launchwrapper.Launch --accessToken 3f564062e975286ae1edca1609b77a18 --username Siamant --session 3f564062e975286ae1edca1609b77a18 --tweakClass cpw.mods.fml.common.launcher.FMLTweaker --tweakClass com.mumfrey.liteloader.launch.LiteLoaderTweaker --gameDir C:\Users\Шилкин\AppData\Roaming\.exclient\hitech --version 1.7.10 --assetsDir C:\Users\Шилкин\AppData\Roaming\.exclient\hitech\assets --uuid 780e469a6f64172c834a8d2de068918c --userProperties {} --assetIndex 1.7.10
            string javaParams = $"{extraArguments} {unknownShit} {javaLibPath} -cp \"{classPath}\" {mainClass} {arguments} --version {version} --gameDir {folder} --assetsDir {folder}/assets --uuid {authData.Uuid} --userProperties {{}} --assetIndex {version}";
            return javaParams;
        }

        public void Load(ExcaliburAuth.ServerConfig serverConfig, ExcaliburAuth.AuthData authData)
        {
            var folder = serverConfig.Directory;

            string javaLibPath = "-Djava.library.path=\"" + folder + "/bin/natives\"";
            string extraArguments = serverConfig.ExtraArguments.Replace("@RAM@", "2048M");
            string unknownShit = "-XX:+UseConcMarkSweepGC -XX:+CMSIncrementalMode -XX:-UseAdaptiveSizePolicy";
            string arguments = serverConfig.Arguments.Replace("@SESSION@", authData.Session).Replace("@USER@", authData.Username);
            string version = serverConfig.Version;
            string mainClass = serverConfig.MainClass;
            StringBuilder classPath = new StringBuilder();
            Array.ForEach(serverConfig.ClassPath.ToArray(), e =>
                classPath.Append($"{folder}/{e};"));
            //C:\Users\Шилкин\AppData\Roaming\exjava\jvm\bin\java.exe -Xmx1024m -Dfml.ignoreInvalidMinecraftCertificates=true -Dfml.ignorePatchDiscrepancies=true -Djava.library.path=C:\Users\Шилкин\AppData\Roaming\.exclient\hitech\bin\natives -cp C:\Users\Шилкин\AppData\Roaming\.exclient\hitech\bin\exblforforge.jar;C:\Users\Шилкин\AppData\Roaming\.exclient\hitech\bin\exblforge.jar;C:\Users\Шилкин\AppData\Roaming\.exclient\hitech\bin\exauth.jar;C:\Users\Шилкин\AppData\Roaming\.exclient\hitech\bin\liteloader.jar;C:\Users\Шилкин\AppData\Roaming\.exclient\hitech\bin\excomplet.jar;C:\Users\Шилкин\AppData\Roaming\.exclient\hitech\bin\hitech.jar net.minecraft.launchwrapper.Launch --accessToken 3f564062e975286ae1edca1609b77a18 --username Siamant --session 3f564062e975286ae1edca1609b77a18 --tweakClass cpw.mods.fml.common.launcher.FMLTweaker --tweakClass com.mumfrey.liteloader.launch.LiteLoaderTweaker --gameDir C:\Users\Шилкин\AppData\Roaming\.exclient\hitech --version 1.7.10 --assetsDir C:\Users\Шилкин\AppData\Roaming\.exclient\hitech\assets --uuid 780e469a6f64172c834a8d2de068918c --userProperties {} --assetIndex 1.7.10
            string javaParams = $"%appdata%\\exjava\\jvm\\bin\\java.exe {extraArguments} {unknownShit} {javaLibPath} -cp \"{classPath}\" {mainClass} {arguments} --version {version} --gameDir {folder} --assetsDir {folder}/assets --uuid {authData.Uuid} --userProperties {{}} --assetIndex {version}";

            StringBuilder toFile = new StringBuilder();
            toFile.AppendLine($"cd {folder}");
            toFile.AppendLine(javaParams);
            toFile.AppendLine("pause");

            File.WriteAllText("start.bat", toFile.ToString());
            Process.Start("start.bat");
        }

        public event EventHandler CanExecuteChanged;

        public virtual void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
