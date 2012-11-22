using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Corbis.Job.Interface;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Xml.Linq;

namespace Corbis.Job.Core
{
    #region InProc Job Storage

    public class InProcJobStorage : IJobStorage 
    {
        protected Dictionary<string, DateTime> LastExecutions
        {
            get { return this.m_LastExecutions; }
        }
        private readonly Dictionary<string, DateTime> m_LastExecutions = new Dictionary<string, DateTime>();

        public virtual DateTime? GetLastExecutionTime(string jobName)
        {
            lock (this.LastExecutions)
            {
                return this.LastExecutions.ContainsKey(jobName) ? this.LastExecutions[jobName] : (DateTime?)null;
            }
        }

        public virtual void SetLastExecutionTime(string jobName, DateTime utc)
        {
            lock (this.LastExecutions)
            {
                this.LastExecutions[jobName] = utc;
            }
        }
    }

    #endregion InProc Job Storage

    #region Xml File Job Storage

    [Serializable, DataContract]
    public class JobData
    {
        /// <summary>
        /// 
        /// </summary>
        [XmlAttribute("name")]
        public string JobName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlAttribute("lastExecuted")]
        public DateTime LastExecuted { get; set; }

    }
    [Serializable, DataContract]
    public class JobStorageContent
    {
        public List<JobData> JobDataItems
        {
            get { return this.m_JobDataItems; }
        }
        private readonly List<JobData> m_JobDataItems = new List<JobData>();
    }
    public class XmlFileJobStorage : IJobStorage
    {
        /// <summary>
        /// Storage file path
        /// </summary>
        protected string FilePath { get; set; }

        /// <summary>
        /// Initializes 
        /// </summary>
        /// <param name="filepath"></param>
        public void Initialize(string filepath)
        {
            if (!Path.IsPathRooted(filepath))
                filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filepath.TrimStart(Path.DirectorySeparatorChar));

            this.FilePath = filepath;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jobName"></param>
        /// <returns></returns>
        public virtual DateTime? GetLastExecutionTime(string jobName)
        {
            lock (this)
            {
                if (!File.Exists(this.FilePath))
                    return (DateTime?)null;

                JobStorageContent storage = null;

                using (FileStream stream = new FileStream(this.FilePath, FileMode.Open, FileAccess.Read))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(JobStorageContent));
                    storage = serializer.Deserialize(stream) as JobStorageContent;
                }

                var data = storage.JobDataItems.Where(x => string.Equals(x.JobName, jobName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                return data == null ? (DateTime?)null : data.LastExecuted;
            }
        }


        public virtual void SetLastExecutionTime(string jobName, DateTime utc)
        {
            lock (this)
            {
                JobStorageContent storage = null;

                XmlSerializer serializer = new XmlSerializer(typeof(JobStorageContent));

                if (File.Exists(this.FilePath))
                {
                    using (FileStream stream = new FileStream(this.FilePath, FileMode.Open, FileAccess.ReadWrite))
                    {
                        storage = serializer.Deserialize(stream) as JobStorageContent;
                    }
                }
                else
                {
                    storage = new JobStorageContent();
                }

                var item = storage.JobDataItems.Where(x => string.Equals(x.JobName, jobName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

                if (item == null)
                {
                    item = new JobData() { JobName = jobName };
                    storage.JobDataItems.Add(item);
                }

                item.LastExecuted = utc;

                using (FileStream stream = new FileStream(this.FilePath, FileMode.Create, FileAccess.Write))
                {                    
                    serializer.Serialize(stream, storage);
                }
            }
        }
    }

    #endregion Xml File Job Storage

    #region DB Job Storage

    public class DbJobStorage : IJobStorage
    {
        public virtual DateTime? GetLastExecutionTime(string jobName)
        {
            throw new NotImplementedException();
        }

        public virtual void SetLastExecutionTime(string jobName, DateTime utc)
        {
            throw new NotImplementedException();
        }
    }

    #endregion DB Job Storage

    public class JobStorageFactory
    {
        public static IJobStorage CreateInstance(JobStorageElement configElem)
        {
            switch (configElem.Mode)
            {
                case JobStorageModes.InProc:
                    return new InProcJobStorage();
                case JobStorageModes.XmlFile:
                    {
                        var storage = new XmlFileJobStorage();
                        storage.Initialize(configElem.FilePath);
                        return storage;
                    }
                case JobStorageModes.Custom:
                    {
                        Type type = Type.GetType(configElem.Type);

                        if(type == null || !typeof(IJobStorage).IsAssignableFrom(type))
                            throw new TypeLoadException();

                        return Activator.CreateInstance(type) as IJobStorage;
                    }
                default:
                    throw new NotImplementedException();
            }

            throw new Exception();
        }
    }
}
