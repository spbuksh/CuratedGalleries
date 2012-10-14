using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;

namespace Corbis.Presentation.Test
{
    [TestClass]
    public class GenericCMSWebTests
    {
        /// <summary>
        /// Checks all controllers of the Corbis.Web.Public site. All controllers must be inherited from
        /// Corbis.Public.Web.Code.PublicControllerBase base type to get logging and other some generic base functionality
        /// </summary>
        [TestMethod]
        public void ControllerInheritance()
        {
            //add exceptions from inheritance rule (for example api controller or ...) if it necessary
            List<Type> excludes = new List<Type>();

            var typeBase = typeof(Corbis.CMS.Web.Controllers.CMSControllerBase);

            StringBuilder errorBuilder = new StringBuilder();

            foreach (var item in typeBase.Assembly.GetTypes().Where(x => typeof(IController).IsAssignableFrom(x) && !excludes.Contains(x)))
            {
                if (!typeBase.IsAssignableFrom(item))
                    errorBuilder.AppendLine(string.Format("Type '{0}' must be inherited from '{1}'", item.AssemblyQualifiedName, typeBase.AssemblyQualifiedName));
            }

            if (errorBuilder.Length != 0)
                Assert.Fail(errorBuilder.ToString());
        }
    }
}
