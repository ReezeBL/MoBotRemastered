using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using AForge.Math;
using Newtonsoft.Json;
using NLog;

namespace MoBot.Core.GameData.Entities
{
    public class Entity : INotifyPropertyChanged
    {
        protected static Dictionary<int, string> EntityNames = new Dictionary<int, string>();
        protected static Logger Logger = LogManager.GetCurrentClassLogger();

        public readonly int Id;
        private Vector3 position;

        public Entity(int id)
        {
            Id = id;
        }

        public float X => position.X;
        public float Y => position.Y;
        public float Z => position.Z;


        public Vector3 Position
        {
            get => position;
            protected set
            {
                if (position == value)
                    return;
                position = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public static void LoadEntities()
        {
            try
            {
                var jsonFile = File.ReadAllText(Settings.EntitiesPath);
                dynamic entities = JsonConvert.DeserializeObject(jsonFile);
                foreach (var entityInfo in entities)
                    EntityNames.Add((int) entityInfo.id, (string) entityInfo.name);
            }
            catch (FileNotFoundException exception)
            {
                Logger.Warn($"Cant find {exception.FileName} file!");
            }
        }

        public void SetPosition(double x, double y, double z)
        {
            Position = new Vector3((float) x, (float) y, (float) z);
        }

        public void Move(double dx, double dy, double dz)
        {
            Position += new Vector3((float) dx, (float) dy, (float) dz);
        }

        public void SetPosition(Vector3 newPos)
        {
            Position = newPos;
        }

        public void Move(Vector3 dir)
        {
            Position += dir;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}