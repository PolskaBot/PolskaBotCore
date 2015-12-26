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
        public const ushort ID = 5565;

        public string var_2768 { get; private set; } //var_2768
        public string var_3110 { get; private set; } //var_3110
        public string var_2036 { get; private set; } //var_2036

        public string MessageType { get; private set; }
        //ttip_killscreen_basic_repair

        public Notify(EndianBinaryReader reader)
        {
            int length = reader.ReadInt32();
            if (length > 0)
            {
                for (int i = 0; i < length; i++)
                {
                    reader.ReadInt16();                                                      //16458  | class_907
                    reader.ReadInt32();
                    reader.ReadInt16();                                                      //24278  | class_356
                    reader.ReadInt16();
                    reader.ReadInt16();
                    reader.ReadInt16();                                                      //19662  | class_370
                    Class370 class370 = new Class370(reader);
                    MessageType = class370.MessageType;
                    reader.ReadInt16();                                                      //19662  | class_370
                    Class370 _class370 = new Class370(reader);
                    reader.ReadInt16();                                                      //19662  | class_370
                    Class370 __class370 = new Class370(reader);
                    int xddddd = reader.ReadInt16(); //6467                                                      //6467   | class_357
                    int xdddd = reader.ReadInt16(); //1
                    int xddd = reader.ReadInt32(); //0
                    int xdd = reader.ReadInt16(); //7603
                    int xd = reader.ReadInt16();                                                      //19662  | class_370
                    Class370 ___class370 = new Class370(reader);
                    reader.ReadBoolean();
                }
            }
            var_2768 = Encoding.UTF8.GetString(reader.ReadBytes(reader.ReadUInt16())); //var_2768
            reader.ReadInt16();
            reader.ReadInt16();
            var_3110 = Encoding.UTF8.GetString(reader.ReadBytes(reader.ReadUInt16())); //var_3110
            reader.ReadInt16();                                                              //12565  | class_524
            reader.ReadInt16();
            reader.ReadInt16();
            reader.ReadInt16();
            var_2036 = Encoding.UTF8.GetString(reader.ReadBytes(reader.ReadUInt16())); //var_2036
        }
    }
}
