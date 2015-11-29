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
            //short length = reader.ReadInt16();
            //short id = reader.ReadInt16();

            //switch(id)
            //{
            //    case 100:
            //        Console.WriteLine("Received packet to forward");
            //        mergedClient.vanillaClient.Send(reader.ReadBytes(length - 2));
            //        break;
            //    default:
            //        Console.WriteLine("Received not known packet. Fast forwarding.");
            //        reader.ReadBytes(length - 2);
            //        break;
            //}
        }
    }
}
