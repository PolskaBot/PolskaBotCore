using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiscUtil.IO;

namespace PolskaBot.Core
{
    class RemoteClient : Client
    {
        public RemoteClient(API api) : base(api)
        {

        }

        public override void Parse(EndianBinaryReader reader)
        {

        }
    }
}
