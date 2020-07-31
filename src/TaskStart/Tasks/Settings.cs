using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace TaskStart.Tasks
{
    public class Settings
    {
        private readonly string _settingsFileName = $"Settings{(Environment.Is64BitOperatingSystem ? 64 : 32)}.xml";

        private Settings()
        {

        }

        private static Settings _instance;
        public static Settings Instance => _instance ?? (_instance = new Settings());

        public IEnumerable<Task> GetTasks()
        {
            var result = new List<Task>();
            if (File.Exists(_settingsFileName))
            {
                try
                {
                    var xmlSer = new XmlSerializer(typeof(List<Task>));
                    using (var sr = new StreamReader(_settingsFileName))
                    {
                        var res = xmlSer.Deserialize(sr) as List<Task>;
                        result = res ?? new List<Task>();
                    }
                }
                catch (Exception)
                {
                    return result;
                }
            }
            return result;
        }

        public void SetTasks(IEnumerable<Task> tasks)
        {
            var taskList = tasks.ToList();
            using (var sw = new StreamWriter(_settingsFileName, false))
            {
                var xmlSer = new XmlSerializer(typeof(List<Task>));
                xmlSer.Serialize(sw, taskList);
            }
        }
    }
}
