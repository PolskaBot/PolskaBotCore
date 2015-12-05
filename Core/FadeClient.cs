using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiscUtil.IO;
using PolskaBot.Core.Darkorbit.Commands;

namespace PolskaBot.Core
{
    public class FadeClient : Client
    {
        public FadeClient(MergedClient mergedClient) : base(mergedClient)
        {

        }

        public override void Parse(EndianBinaryReader reader)
        {

        }
    }
}
