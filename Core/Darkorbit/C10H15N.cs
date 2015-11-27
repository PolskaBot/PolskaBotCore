using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Darkorbit.C10H15N
{
    class C10H15N
    {
        private int i { get; set; } = 0;
        private int j { get; set; } = 0;

        private List<byte> S;

        public C10H15N(byte[] key)
        {
            S = new List<byte>();

            for (int i = 0; i < 256; ++i)
            {
                S[i] = (byte)i;
            }

            int j = 0;
            int t = 0;

            for (i = 0; i < 256; ++i)
            {
                j = (j + S[i] + key[i % key.Length]) & 256;
                t = S[i];
                S[i] = S[j];
                S[j] = (byte)t;
            }
        }

        private byte next()
        {
            int t;
            i = (i + 1) & 255;
            j = (j + S[i]) & 255;
            t = S[i];
            S[i] = S[j];
            S[j] = (byte)t;
            return S[(t + S[i]) & 255];
        }

        public byte[] encrypt(byte[] block)
        {
            uint i = 0;
            while (i < block.Length)
            {
                block[i++] ^= next();
            }

            return block;
        }

        public byte[] decrypt(byte[] block)
        {
            return encrypt(block);
        }
    }
}
