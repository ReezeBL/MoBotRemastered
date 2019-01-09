using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MoBot.Core.Net
{
    public class StreamWrapper : IDisposable
    {
        private readonly Stream stream = new MemoryStream();
        private readonly BinaryReader reader;
        private readonly BinaryWriter writer;

        public StreamWrapper()
        {
            reader = new BinaryReader(stream);
            writer = new BinaryWriter(stream);
        }
        public StreamWrapper(byte[] data)
        {
            stream = new MemoryStream(data);
            reader = new BinaryReader(stream);
            writer = new BinaryWriter(stream);
        }
        public StreamWrapper(Stream s)
        {
            stream = s;
            reader = new BinaryReader(s);
            writer = new BinaryWriter(s);
        }

        public int ReadVarInt() // Reads variable length int to stream
        {
            int result = 0, length = 0;
            byte nextByte;

            do
            {
                nextByte = ReadByte();
                result |= (nextByte & 127) << length++ * 7;

                if (length > 5)
                    throw new IOException("VarInt too big");
            }
            while ((nextByte & 128) == 128);

            return result;
        }
        public void WriteVarInt(int val) // Writes variable length int to stream
        {
            while ((val & -128) != 0)
            {
                WriteByte((byte)(val & 127 | 128));
                val >>= 7;
            }
            WriteByte((byte)val);
        }
        public byte ReadByte()
        {
            return reader.ReadByte();
        }
        public void WriteByte(byte b)
        {
            writer.Write(b);
        }
        public void WriteInt(int val)
        {
            writer.Write(IPAddress.HostToNetworkOrder(val));
        }
        public int ReadInt()
        {
            return IPAddress.NetworkToHostOrder(reader.ReadInt32());
        }
        public void WriteShort(short val)
        {
            writer.Write(IPAddress.HostToNetworkOrder(val));
        }
        public short ReadShort()
        {
            return IPAddress.NetworkToHostOrder(reader.ReadInt16());
        }
        public void WriteBytes(byte[] val)
        {
            writer.Write(val);
        }

        public byte[] ReadBytes(int len)
        {
            return reader.ReadBytes(len);
        }

        public int ReadBytes(byte[] buff)
        {
            return reader.Read(buff, 0, buff.Length);
        }

        public void WriteString(string val) //Writes an UTF8 string to stream
        {
            var bytes = Encoding.UTF8.GetBytes(val);
            WriteVarInt(bytes.Length);
            writer.Write(bytes);
        }     
        public string ReadString() //Reads an UTF8 string from stream
        {
            var length = ReadVarInt();
            if (length < 0)
            {
                throw new IOException("The received encoded string Bytes length is less than zero! Weird string!");
            }
            var buffer = reader.ReadBytes(length);
            return Encoding.UTF8.GetString(buffer);
        }

        public string ReadStringT()
        {
            var length = ReadShort();
            if(length < 0)
                throw new IOException("The received encoded string Bytes length is less than zero! Weird string!");
            var buffer = reader.ReadBytes(length);
            return Encoding.UTF8.GetString(buffer);
        }

        public char ReadChar()
        {
            return reader.ReadChar();
        }

        public float ReadSingle()
        {
            return BitConverter.ToSingle(BitConverter.GetBytes(IPAddress.NetworkToHostOrder(reader.ReadInt32())), 0);
        }
        public void WriteSingle(float val)
        {
            writer.Write(IPAddress.HostToNetworkOrder(BitConverter.ToInt32(BitConverter.GetBytes(val), 0)));
        }
        public double ReadDouble()
        {
            return BitConverter.Int64BitsToDouble(IPAddress.NetworkToHostOrder(reader.ReadInt64()));
        }
        public void WriteDouble(double val)
        {
            writer.Write(IPAddress.HostToNetworkOrder(BitConverter.DoubleToInt64Bits(val)));
        }
        public bool ReadBool()
        {
            return reader.ReadBoolean();
        }
        public void WriteBool(bool val)
        {
            writer.Write(val);
        }
        public void WriteLong(long val)
        {
            writer.Write(IPAddress.HostToNetworkOrder(val));
        }
        public long ReadLong()
        {
            return IPAddress.NetworkToHostOrder(reader.ReadInt64());
        }
        public byte[] GetBlob()//Return contains of memory stream
        {
            if(!(stream is MemoryStream memoryStream))
                throw new InvalidOperationException("Cant recieve blob from non MEmory stream");
            return memoryStream.ToArray();
        }

        public void Dispose()
        {
            stream?.Dispose();
        }

        public void Clear()
        {
            stream.Flush();
        }
        public Stream GetStream()
        {
            return stream;
        }

        public bool Eof => reader.PeekChar() == -1;
    }
}
