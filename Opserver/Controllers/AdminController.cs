using System.Web.Mvc;
using StackExchange.Exceptional;
using StackExchange.Opserver.Helpers;
using StackExchange.Opserver.Models;
using System.Threading.Tasks;
using StackExchange.Opserver.Data.SQL;

namespace StackExchange.Opserver.Controllers
{
    [OnlyAllow(Roles.GlobalAdmin)]
    public class AdminController : StatusController
    {
        [Route("admin/security/purge-cache")]
        public ActionResult Dashboard()
        {
            Current.Security.PurgeCache();
            return TextPlain("Cache Purged");
        }

        /// <summary>
        /// Access our error log.
        /// </summary>
        [Route("admin/errors/{resource?}/{subResource?}"), AlsoAllow(Roles.LocalRequest)]
        public Task InvokeErrorHandler() => ExceptionalModule.HandleRequestAsync(System.Web.HttpContext.Current);

        /// <summary>
        /// Kill active process id.
        /// </summary>
        /// <param name="node">which server</param>
        /// <param name="spid">which spid</param>
        /// <returns></returns>
        [Route("sql/kill-spid"), HttpPost, OnlyAllow(Roles.SQLAdmin)]
        public async Task<ActionResult> KillSpid(string node, int spid)
        {
            var instance = SQLInstance.Get(node);
            return Json(await instance.KillSpid(spid));
        }
    }
}
