using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Hosting
{
    public static class HostEnvironmentExtensions
    {
        public static bool ShouldRunAngular(this IHostEnvironment hostEnvironment)
        {
            if (hostEnvironment == null)
                throw new ArgumentNullException(nameof(hostEnvironment));

            return Environment.GetEnvironmentVariable("RUN_NG") == "true";
        }
    }
}
