using System;
using System.IO;
using System.Text;
using GameFramework;

namespace GT.Hotfix
{
    public class BufferReader : IReference
    {
        private MemoryStream stream;
        private BinaryReader reader;

        public BufferReader(params byte[] data)
        {
            stream = new MemoryStream(data);
            reader = new BinaryReader(stream);
        }

        public BufferReader()
        {
            stream = new MemoryStream();
            reader = new BinaryReader(stream);
        }

        public void Close()
        {
            if (reader != null) reader.Close();

            stream.Close();
            reader = null;
            stream = null;
        }

        public byte ReadByte()
        {
            return reader.ReadByte();
        }

        public int ReadInt()
        {
            byte[] temp = BitConverter.GetBytes(reader.ReadUInt32());
            Array.Reverse(temp);
            return BitConverter.ToInt32(temp, 0);
        }

        public ushort ReadShort()
        {
            byte[] temp = BitConverter.GetBytes(reader.ReadUInt16());
            Array.Reverse(temp);
            return BitConverter.ToUInt16(temp, 0);
        }

        public long ReadLong()
        {
            return (long)reader.ReadInt64();
        }

        public float ReadFloat()
        {
            byte[] temp = BitConverter.GetBytes(reader.ReadSingle());
            Array.Reverse(temp);
            return BitConverter.ToSingle(temp, 0);
        }

        public double ReadDouble()
        {
            byte[] temp = BitConverter.GetBytes(reader.ReadDouble());
            Array.Reverse(temp);
            return BitConverter.ToDouble(temp, 0);
        }

        public string ReadString()
        {
            ushort len = ReadShort();
            byte[] buffer = new byte[len];
            buffer = reader.ReadBytes(len);
            return Encoding.UTF8.GetString(buffer);
        }

        public byte[] ReadBytes()
        {
            int len = ReadInt();
            return reader.ReadBytes(len);
        }

        public byte[] ReadRemainBytes()
        {
            long length = reader.BaseStream.Length - reader.BaseStream.Position;
            return reader.ReadBytes((int)length);
        }

        public MemoryStream GetMemoryStream()
        {
            return stream;
        }

        public void WriteTo(Stream destination)
        {
            stream.WriteTo(destination);
        }

        public void WriteFrom(Stream destination)
        {
            destination.CopyTo(stream);
            stream.Position = 0;
        }

        public void Flush()
        {
            stream.Position = 0;
            stream.SetLength(0);
            stream.Flush();
        }

        public void Clear()
        {
            Flush();
        }
    }
}