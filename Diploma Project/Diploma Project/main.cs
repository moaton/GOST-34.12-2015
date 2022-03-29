using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma_Project
{
    class main
    {
        public string keyByte = "";
        public string keyText = "";
        public int numbersOfBlock = 0;
        public string plainTextLengthened = "";


        byte[][] iterC = new byte[32][];    // итерациялық константалар массивтері
        byte[][] iterK = new byte[10][];    // раундық кілттер массивтері
        const int blockSize = 16;           // блок ұзындығы

        static byte[] Pi = {
            0xFC, 0xEE, 0xDD, 0x11, 0xCF, 0x6E, 0x31, 0x16, 0xFB, 0xC4, 0xFA, 0xDA, 0x23, 0xC5, 0x04, 0x4D,
            0xE9, 0x77, 0xF0, 0xDB, 0x93, 0x2E, 0x99, 0xBA, 0x17, 0x36, 0xF1, 0xBB, 0x14, 0xCD, 0x5F, 0xC1,
            0xF9, 0x18, 0x65, 0x5A, 0xE2, 0x5C, 0xEF, 0x21, 0x81, 0x1C, 0x3C, 0x42, 0x8B, 0x01, 0x8E, 0x4F,
            0x05, 0x84, 0x02, 0xAE, 0xE3, 0x6A, 0x8F, 0xA0, 0x06, 0x0B, 0xED, 0x98, 0x7F, 0xD4, 0xD3, 0x1F,
            0xEB, 0x34, 0x2C, 0x51, 0xEA, 0xC8, 0x48, 0xAB, 0xF2, 0x2A, 0x68, 0xA2, 0xFD, 0x3A, 0xCE, 0xCC,
            0xB5, 0x70, 0x0E, 0x56, 0x08, 0x0C, 0x76, 0x12, 0xBF, 0x72, 0x13, 0x47, 0x9C, 0xB7, 0x5D, 0x87,
            0x15, 0xA1, 0x96, 0x29, 0x10, 0x7B, 0x9A, 0xC7, 0xF3, 0x91, 0x78, 0x6F, 0x9D, 0x9E, 0xB2, 0xB1,
            0x32, 0x75, 0x19, 0x3D, 0xFF, 0x35, 0x8A, 0x7E, 0x6D, 0x54, 0xC6, 0x80, 0xC3, 0xBD, 0x0D, 0x57,
            0xDF, 0xF5, 0x24, 0xA9, 0x3E, 0xA8, 0x43, 0xC9, 0xD7, 0x79, 0xD6, 0xF6, 0x7C, 0x22, 0xB9, 0x03,
            0xE0, 0x0F, 0xEC, 0xDE, 0x7A, 0x94, 0xB0, 0xBC, 0xDC, 0xE8, 0x28, 0x50, 0x4E, 0x33, 0x0A, 0x4A,
            0xA7, 0x97, 0x60, 0x73, 0x1E, 0x00, 0x62, 0x44, 0x1A, 0xB8, 0x38, 0x82, 0x64, 0x9F, 0x26, 0x41,
            0xAD, 0x45, 0x46, 0x92, 0x27, 0x5E, 0x55, 0x2F, 0x8C, 0xA3, 0xA5, 0x7D, 0x69, 0xD5, 0x95, 0x3B,
            0x07, 0x58, 0xB3, 0x40, 0x86, 0xAC, 0x1D, 0xF7, 0x30, 0x37, 0x6B, 0xE4, 0x88, 0xD9, 0xE7, 0x89,
            0xE1, 0x1B, 0x83, 0x49, 0x4C, 0x3F, 0xF8, 0xFE, 0x8D, 0x53, 0xAA, 0x90, 0xCA, 0xD8, 0x85, 0x61,
            0x20, 0x71, 0x67, 0xA4, 0x2D, 0x2B, 0x09, 0x5B, 0xCB, 0x9B, 0x25, 0xD0, 0xBE, 0xE5, 0x6C, 0x52,
            0x59, 0xA6, 0x74, 0xD2, 0xE6, 0xF4, 0xB4, 0xC0, 0xD1, 0x66, 0xAF, 0xC2, 0x39, 0x4B, 0x63, 0xB6
        };
        static byte[] Pi_Reverse = new byte[256]
        {
            0xA5, 0x2D, 0x32, 0x8F, 0x0E, 0x30, 0x38, 0xC0, 0x54, 0xE6, 0x9E, 0x39, 0x55, 0x7E, 0x52, 0x91,
            0x64, 0x03, 0x57, 0x5A, 0x1C, 0x60, 0x07, 0x18, 0x21, 0x72, 0xA8, 0xD1, 0x29, 0xC6, 0xA4, 0x3F,
            0xE0, 0x27, 0x8D, 0x0C, 0x82, 0xEA, 0xAE, 0xB4, 0x9A, 0x63, 0x49, 0xE5, 0x42, 0xE4, 0x15, 0xB7,
            0xC8, 0x06, 0x70, 0x9D, 0x41, 0x75, 0x19, 0xC9, 0xAA, 0xFC, 0x4D, 0xBF, 0x2A, 0x73, 0x84, 0xD5,
            0xC3, 0xAF, 0x2B, 0x86, 0xA7, 0xB1, 0xB2, 0x5B, 0x46, 0xD3, 0x9F, 0xFD, 0xD4, 0x0F, 0x9C, 0x2F,
            0x9B, 0x43, 0xEF, 0xD9, 0x79, 0xB6, 0x53, 0x7F, 0xC1, 0xF0, 0x23, 0xE7, 0x25, 0x5E, 0xB5, 0x1E,
            0xA2, 0xDF, 0xA6, 0xFE, 0xAC, 0x22, 0xF9, 0xE2, 0x4A, 0xBC, 0x35, 0xCA, 0xEE, 0x78, 0x05, 0x6B,
            0x51, 0xE1, 0x59, 0xA3, 0xF2, 0x71, 0x56, 0x11, 0x6A, 0x89, 0x94, 0x65, 0x8C, 0xBB, 0x77, 0x3C,
            0x7B, 0x28, 0xAB, 0xD2, 0x31, 0xDE, 0xC4, 0x5F, 0xCC, 0xCF, 0x76, 0x2C, 0xB8, 0xD8, 0x2E, 0x36,
            0xDB, 0x69, 0xB3, 0x14, 0x95, 0xBE, 0x62, 0xA1, 0x3B, 0x16, 0x66, 0xE9, 0x5C, 0x6C, 0x6D, 0xAD,
            0x37, 0x61, 0x4B, 0xB9, 0xE3, 0xBA, 0xF1, 0xA0, 0x85, 0x83, 0xDA, 0x47, 0xC5, 0xB0, 0x33, 0xFA,
            0x96, 0x6F, 0x6E, 0xC2, 0xF6, 0x50, 0xFF, 0x5D, 0xA9, 0x8E, 0x17, 0x1B, 0x97, 0x7D, 0xEC, 0x58,
            0xF7, 0x1F, 0xFB, 0x7C, 0x09, 0x0D, 0x7A, 0x67, 0x45, 0x87, 0xDC, 0xE8, 0x4F, 0x1D, 0x4E, 0x04,
            0xEB, 0xF8, 0xF3, 0x3E, 0x3D, 0xBD, 0x8A, 0x88, 0xDD, 0xCD, 0x0B, 0x13, 0x98, 0x02, 0x93, 0x80,
            0x90, 0xD0, 0x24, 0x34, 0xCB, 0xED, 0xF4, 0xCE, 0x99, 0x10, 0x44, 0x40, 0x92, 0x3A, 0x01, 0x26,
            0x12, 0x1A, 0x48, 0x68, 0xF5, 0x81, 0x8B, 0xC7, 0xD6, 0x20, 0x0A, 0x08, 0x00, 0x4C, 0xD7, 0x74
        };

        #region X Преобразование (Сложение по модулю 2)

        static byte[] XOR(byte[] input1, byte[] input2)             // X Түрлендіру векторларды екілік модуль бойынша қосу (сложение 2х векторов по модулю 2)
        {
            byte[] output = new byte[blockSize];
            for (int i = 0; i < blockSize; i++)
            {
                output[i] = Convert.ToByte(input1[i] ^ input2[i]);  // ^ XOR - болдырмайтын НЕМЕСЕ (исключающий ИЛИ)
            }
            return output;
        }

        #endregion

        #region Генерация_раундовых_ключей

        private void GostF(byte[] input1, byte[] input2, ref byte[] output1, ref byte[] output2, byte[] round_C)
        {
            byte[] state = new byte[blockSize];
            state = XOR(input1, round_C);
            state = GostS(state);
            state = GostL(state);
            output1 = XOR(state, input2);
            output2 = input1;
        }

        private void GostKeyGen(byte[] mas_key)
        {
            #region Генерация раундовых констант

            byte[][] iterNum = new byte[32][];
            for (int i = 0; i < 32; i++)
            {
                iterNum[i] = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Convert.ToByte(i + 1) };
                iterC[i] = GostL(iterNum[i]);
            }

            #endregion

            #region Генерация_первых_2-х_ключей

            byte[] A = new byte[blockSize];
            for (int i = 0; i < blockSize; i++) A[i] = mas_key[i];
            byte[] B = new byte[blockSize];
            int j = 0;
            for (int i = blockSize; i < 32; i++)
            {
                B[j] = mas_key[i];
                j++;
            }
            j = 0;
            iterK[0] = B;
            iterK[1] = A;

            byte[] C = new byte[blockSize];
            byte[] D = new byte[blockSize];
            #endregion

            #region Генерация_остальных_ключей

            for (int i = 0; i < 4; i++)
            {
                GostF(A, B, ref C, ref D, iterC[0 + 8 * i]);
                GostF(C, D, ref A, ref B, iterC[1 + 8 * i]);
                GostF(A, B, ref C, ref D, iterC[2 + 8 * i]);
                GostF(C, D, ref A, ref B, iterC[3 + 8 * i]);
                GostF(A, B, ref C, ref D, iterC[4 + 8 * i]);
                GostF(C, D, ref A, ref B, iterC[5 + 8 * i]);
                GostF(A, B, ref C, ref D, iterC[6 + 8 * i]);
                GostF(C, D, ref A, ref B, iterC[7 + 8 * i]);
                iterK[2 * i + 2] = A;
                iterK[2 * i + 3] = B;
            }

            #endregion

        }

        #endregion

        #region Зашифрование и расшифрование

        public byte[] GostEncript(byte[] text, byte[] masterKey)
        {
            masterKey = Encoding.Default.GetBytes(LengthTo32Bytes(Encoding.Default.GetString(masterKey)));
            
            keyByte = BitConverter.ToString(masterKey);
            keyText = LengthTo32Bytes(Encoding.Default.GetString(masterKey));

            GostKeyGen(masterKey);          // Кілт өрістету
            int NumOfBlocks;                // 16 байтқа тең блок санын есептеуші
            int NumberOfNull;               // жетіспейтін байт санын есептеуші
            byte[] OriginText = text;       // Ашық мәтін
            byte[] encrText = new byte[0];  // Шифрленген байт сақтаушы массив
            if ((text.Length % blockSize) == 0)
            {
                NumOfBlocks = text.Length / blockSize;
                numbersOfBlock = NumOfBlocks;
                plainTextLengthened = BitConverter.ToString(OriginText);
                Array.Resize(ref encrText, text.Length);
            }
            else
            {
                NumOfBlocks = (text.Length / blockSize) + 1;
                NumberOfNull = NumOfBlocks * blockSize - text.Length;
                
                Array.Resize(ref OriginText, OriginText.Length + NumberOfNull);
                Array.Resize(ref encrText, OriginText.Length);
                numbersOfBlock = NumOfBlocks;
                if (NumberOfNull == 1) OriginText[OriginText.Length - 1] = 0x80;
                else
                {
                    for (int i = OriginText.Length - 1; i >= 0; i--)
                    {
                        Console.WriteLine(i + " i");
                        if (i == OriginText.Length - 1)
                        {
                            OriginText[OriginText.Length - 1] = 0x81;
                        }
                        else if (OriginText[i] != 0)
                        {
                            OriginText[i + 1] = 0x01;
                            break;
                        }
                    }
                    Console.WriteLine(BitConverter.ToString(OriginText) + " OriginText");
                    plainTextLengthened = BitConverter.ToString(OriginText);
                }
            }
            for (int i = 0; i < NumOfBlocks; i++) // Шифрлеу операциясы
            {
                byte[] block = new byte[blockSize];
                for (int j = 0; j < blockSize; j++)
                {
                    block[j] = OriginText[i * blockSize + j];
                }
                for (int j = 0; j < 9; j++)
                {
                    block = XOR(block, iterK[j]);   // 2 модуль бойынша қосу
                    block = GostS(block);           // Сызықты емес түрлендіру S
                    block = GostL(block);           // Сызықты түрлендіру
                }
                block = XOR(block, iterK[9]);
                for (int j = 0; j < blockSize; j++)
                {
                    encrText[i * blockSize + j] = block[j];
                }
            }
            getRoundKeys();
            return encrText;
        } // Шифрлеу функциясы

        public byte[] GostDecript(byte[] text, byte[] masterKey)
        {
            GostKeyGen(masterKey);
            keyText = Encoding.Default.GetString(masterKey);
            int NumOfBlocks = text.Length / blockSize;  // 16 байтқа тең блок санын есептеу
            byte[] OriginText = text;                   // Шифр мәтінді сақтайтын массив
            byte[] decrText = new byte[text.Length];    // Ашық мәтінді сақтайтын массив
            for (int i = 0; i < NumOfBlocks; i++)
            {
                byte[] block = new byte[blockSize];
                for (int j = 0; j < blockSize; j++)
                {
                    block[j] = OriginText[i * blockSize + j];
                }
                block = XOR(block, iterK[9]);
                for (int j = 8; j >= 0; j--)
                { 
                    block = GostLReverse(block);
                    block = GostSReverse(block);
                    block = XOR(block, iterK[j]);
                }
                for (int j = 0; j < blockSize; j++)
                {
                    decrText[i * blockSize + j] = block[j];
                }
                if (i == NumOfBlocks - 1 && (decrText[decrText.Length - 1] == 0x81 || decrText[decrText.Length - 1] == 0x80))
                {
                    if (decrText[decrText.Length - 1] == 0x81)
                    {
                        int Zeros = 0;
                        for (int j = decrText.Length - 1; j > 0; j--)
                        {
                            if (decrText[j] == 0x81 || decrText[j] == 0x01 || decrText[j] == 0) Zeros++;
                            else break;
                        }
                        Array.Resize(ref decrText, decrText.Length - Zeros);
                    }
                    if (decrText[decrText.Length - 1] == 0x80) Array.Resize(ref decrText, decrText.Length - 1);
                }
            }
            return decrText;
        }

        #endregion

        #region Нелинейное_преобразование_(Операция_S)

        static byte[] GostS(byte[] input) // Сызықты емес түрлендіру S
        {
            byte[] output = new byte[blockSize];
            for (int i = 0; i < blockSize; i++)
            {
                output[i] = Pi[input[i]];
            }
            return output;
        }

        static byte[] GostSReverse(byte[] input) // Кері сызықты емес түрлендіруы S
        {
            byte[] output = new byte[blockSize];
            for (int i = 0; i < blockSize; i++)
            {
                output[i] = Pi_Reverse[input[i]];
            }
            return output;
        }
        #endregion

        #region Линейное_преобразование_(Операция_L)

        static byte GostMulInGF(byte a, byte b)
        {
            byte p = 0;
            byte counter;
            byte hi_bit_set;
            for (counter = 0; counter < 8 && a != 0 && b != 0; counter++)
            {
                if ((b & 1) != 0)
                    p ^= a;
                hi_bit_set = (byte)(a & 0x80);
                a <<= 1;
                if (hi_bit_set != 0)
                    a ^= 0xc3; /* x^8 + x^7 + x^6 + x + 1 */
                b >>= 1;
            }
            return p;
        }

        static byte[] LVec = new byte[]
        {148, 32, 133, 16, 194, 192, 1, 251, 1, 192, 194, 16, 133, 32, 148, 1};

        static byte[] GostR(byte[] input)
        {
            byte a_15 = 0;
            byte[] state = new byte[blockSize];
            for (int i = 0; i <= 15; i++)
            {
                a_15 ^= GostMulInGF(input[i], LVec[i]);
            }
            for (int i = 15; i > 0; i--)
            {
                state[i] = input[i - 1];
            }
            state[0] = a_15;
            return state;
        }

        static byte[] GostL(byte[] input)
        {
            byte[] state = input;
            for (int i = 0; i < blockSize; i++)
            {
                state = GostR(state);
            }
            return state;
        }

        static byte[] GostRReverse(byte[] input)
        {
            byte a_15 = input[0];
            byte[] state = new byte[blockSize];
            for (int i = 0; i < 15; i++)
            {
                state[i] = input[i + 1];
            }
            for (int i = 15; i >= 0; i--)
            {
                a_15 ^= GostMulInGF(state[i], LVec[i]);
            }
            state[15] = a_15;
            return state;
        }

        static byte[] GostLReverse(byte[] input) // Кері сызықты түрлендіру
        {
            byte[] state = input;
            for (int i = 0; i < blockSize; i++)
            {
                state = GostRReverse(state);
            }
            return state;
        }

        #endregion

        private string LengthTo32Bytes(string str)
        {
            if (str.Length < 32)
            {
                int diff = 32 - str.Length;
                int j = 0;
                for (int i = str.Length; i < 32; i++)
                {
                    str += str.Substring(j, 1);
                    if (j == str.Length - 1) j = 0;
                    else j++;
                }
                return str;
            }
            else if (str.Length > 32) return str = str.Substring(0, 32);
            else return str;
        }

        public string getRoundKeys()
        {
            String keys = "";
            for (int i = 0; i < 10; i++)
            {
                keys += i+1 + ") " + BitConverter.ToString(iterK[i]) + Environment.NewLine;
            }
            return keys;
        }

    }
}
