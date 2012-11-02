using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using Corbis.CMS.Web.Code;
using Corbis.Common;
using Corbis.Logging;
using Corbis.Logging.Interface;

namespace Corbis.CMS.Web.GarbageCollector
{
    /// <summary>
    /// Class to find and remove unnecesary files in the system.
    /// </summary>
    public static class CorbisGarbageCollector
    {
        #region Constructor

        #endregion

        #region Properties

        /// <summary>
        /// System logger
        /// </summary>
        private static ILogManager Logger
        {
            get { return LogManagerProvider.Instance; }
        }

        private static string ConnectionString
        {
            get
            {
                return @"Data Source=.\SQLEXPRESS;AttachDbFilename=" + Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/App_Data"), "Corbis_Garbage.mdf") + ";Integrated Security=True;User Instance=True";
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Checks if need to clear.
        /// </summary>
        /// <returns></returns>
        private static bool Check()
        {
            var result = false;
            try
            {
                using (var connection = SqlHelper.GetConnection(ConnectionString))
                {
                    using (var command = connection.GetCommand("SELECT TOP 1 ID, LastTimeCollected FROM Tasks  ORDER BY LastTimeCollected", CommandType.Text))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var lastTimeCollected = (DateTime)reader["LastTimeCollected"];
                                if ((DateTime.Now - lastTimeCollected).Days >= 1)
                                {
                                    result = true;
                                }
                            }
                            reader.Close();

                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Logger.WriteError(e);
            }
            return result;
        }

        /// <summary>
        /// Adds new Record when Collected Finished.
        /// </summary>
        private static void ApplyResults()
        {
            try
            {
                using (var connection = SqlHelper.GetConnection(ConnectionString))
                {
                    using (var command = connection.GetCommand("INSERT INTO dbo.Tasks (LastTimeCollected) VALUES (@LastTimeCollected)", CommandType.Text))
                    {
                        command.AddParameter("@LastTimeCollected", DateTime.Now, SqlDbType.DateTime);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException e)
            {
                Logger.WriteError(e);
            }
        }




        /// <summary>
        /// Removes all not actual Templates.
        /// </summary>
        private static void CollectTemplates()
        {
            try
            {
                var root = new DirectoryInfo(GalleryRuntime.TemplateDirectory);
                var actualTemplates = GalleryRuntime.GetTemplates().Select(_ => _.ID.ToString()).ToList();
                var dirictories = root.GetDirectories().Select(_ => _.Name).ToList();
                var dirictoriesToRemove = dirictories.Except(actualTemplates).ToList();
                var itemToRemove = root.GetDirectories().Where(_ => dirictoriesToRemove.Contains(_.Name)).ToList();
                foreach (var directoryInfo in itemToRemove)
                {
                    directoryInfo.Remove();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
                throw;
            }

        }

        /// <summary>
        /// Removes all not actual Galleries.
        /// </summary>
        private static void CollectGalleries()
        {
            try
            {
                var root = new DirectoryInfo(GalleryRuntime.GalleryDirectory);
                var actualGalleries = GalleryRuntime.GetGalleries().Select(_ => _.ID.ToString()).ToList();
                var dirictories = root.GetDirectories().Select(_ => _.Name).ToList();
                var dirictoriesToRemove = dirictories.Except(actualGalleries).ToList();
                var itemToRemove = root.GetDirectories().Where(_ => dirictoriesToRemove.Contains(_.Name)).ToList();
                foreach (var directoryInfo in itemToRemove)
                {
                    directoryInfo.Remove();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
                throw;
            }
        }

        /// <summary>
        /// Start to collect garbage.
        /// </summary>
        public static void Collect()
        {
            if (!Check()) return;
            CollectTemplates();
            CollectGalleries();
            ApplyResults();
        }

        #endregion
    }
}