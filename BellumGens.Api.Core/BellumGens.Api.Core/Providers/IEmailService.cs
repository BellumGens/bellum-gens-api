using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BellumGens.Api.Core.Providers
{
    public interface IEmailService
    {
        public Task SendNotificationEmail(string destination, string subject, string body, string bcc);
    }
}
