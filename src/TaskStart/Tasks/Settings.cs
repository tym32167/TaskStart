using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace TaskStart.Tasks
{
	public class Settings
	{
		private const string SettingsFileName = "Settings.xml";

		private Settings()
		{

		}

		private static Settings _instance;
		public static Settings Instance
		{
			get { return _instance ?? (_instance = new Settings()); }
		}

		public IEnumerable<Task> GetTasks()
		{
			var result = new List<Task>();
			if (File.Exists(SettingsFileName))
			{
				try
				{
					var xmlSer = new XmlSerializer(typeof(List<Task>));
					using (var sr = new StreamReader(SettingsFileName))
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
			using (var sw = new StreamWriter(SettingsFileName, false))
			{
				var xmlSer = new XmlSerializer(typeof(List<Task>));
				xmlSer.Serialize(sw, taskList);
			}
		}
	}
}
