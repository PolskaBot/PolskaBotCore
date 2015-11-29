using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiscUtil.IO;
using MiscUtil.Conversion;
using PolskaBot.Core.Darkorbit.Commands;

namespace PolskaBot.Core
{
    class VanillaClient : Client
    {
        public VanillaClient(MergedClient mergedClient) : base(mergedClient)
        {

        }

        protected override void Parse(EndianBinaryReader reader)
        {
            short length = reader.ReadInt16();
            short id = reader.ReadInt16();

            mergedClient.fadeClient.Send(new ClientDecodeHeader(length, id));
            EndianBinaryReader fadeReader = new EndianBinaryReader(EndianBitConverter.Big, mergedClient.fadeClient.stream);
            short fadeLength = fadeReader.ReadInt16();
            short fadeID = fadeReader.ReadInt16();

            Console.WriteLine("Received packet with ID {0} and length {1}", fadeID, fadeLength);

            switch(id)
            {
                case ServerVersionCheck.ID:
                    ServerVersionCheck serverVersionCheck = new ServerVersionCheck(reader);

                    if(serverVersionCheck.compatible)
                    {
                        Console.WriteLine("Client is compatible");
                        Send(new ClientRequestCode());
                    } else
                    {
                        Console.WriteLine("Client is not compatible");
                        thread.Abort();
                    }
                    break;
                default:
                    reader.ReadBytes(length - 2);
                    break;
            }
        }
    }
}
