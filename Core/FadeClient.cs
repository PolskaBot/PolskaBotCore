using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiscUtil.IO;
using PolskaBot.Core.Darkorbit.Commands;

namespace PolskaBot.Core
{
    class FadeClient : Client
    {
        public FadeClient(MergedClient mergedClient) : base(mergedClient)
        {

        }

        protected override void Parse(EndianBinaryReader reader)
        {

        }
    }
}
