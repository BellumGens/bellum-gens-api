using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BellumGens.Api.Core.Providers
{
    public interface IFileService
    {
        public string SaveImageFile(string blob, string name);
    }
}
