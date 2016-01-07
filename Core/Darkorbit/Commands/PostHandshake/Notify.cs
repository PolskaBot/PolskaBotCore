using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiscUtil.IO;

namespace PolskaBot.Core.Darkorbit.Commands.PostHandshake
{
    class Notify : Command
    {
        public const ushort ID = 6339;

        public string var_2770 { get; private set; } //var_2770
        public string var_3114 { get; private set; } //var_3114
        public string var_2034 { get; private set; } //var_2034

        public string MessageType { get; private set; }
        //ttip_killscreen_basic_repair

        public Notify(EndianBinaryReader reader)
        {
            var_3114 = Encoding.UTF8.GetString(reader.ReadBytes(reader.ReadUInt16()));
            var_2034 = Encoding.UTF8.GetString(reader.ReadBytes(reader.ReadUInt16()));
            int length = reader.ReadInt32();
            if (length > 0)
            {
                for (int i = 0; i < length; i++)
                {
                    reader.ReadInt16();                                                      //class_907
                    reader.ReadInt16();
                    reader.ReadInt16();                                                      //class_370
                    Class370 var_1480 = new Class370(reader);
                    reader.ReadInt16();                                                      //class_356
                    reader.ReadInt16();
                    reader.ReadInt16();
                    reader.ReadInt16();
                    reader.ReadBoolean();
                    reader.ReadInt16();                                                      //class_357
                    reader.ReadInt32();
                    reader.ReadInt16();
                    reader.ReadInt16();                                                      //class_370
                    Class370 toolTipKey = new Class370(reader);
                    MessageType = toolTipKey.MessageType;
                    reader.ReadInt16();                                                      //class_370
                    Class370 name_29 = new Class370(reader);
                    reader.ReadInt32();
                    reader.ReadUInt16();
                    reader.ReadInt16();                                                      //class_370
                    Class370 var_2698 = new Class370(reader);
                }
            }
            reader.ReadInt16();
            var_2770 = Encoding.UTF8.GetString(reader.ReadBytes(reader.ReadUInt16()));
            reader.ReadInt16();                                                              //class_524
            reader.ReadInt16();
        }
    }
}
