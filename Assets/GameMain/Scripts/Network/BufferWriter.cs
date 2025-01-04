using System;
using System.IO;
using System.Text;
using GameFramework;

namespace GT.Hotfix
{
    public class BufferWriter : IReference
    {
        private MemoryStream stream;
        private BinaryWriter writer;

        public BufferWriter()
        {
            stream = new MemoryStream();
            writer = new BinaryWriter(stream);
        }

        public void Close()
        {
            if (writer != null) writer.Close();

            stream.Close();
            writer = null;
            stream = null;
        }

        public void WriteByte(byte v)
        {
            writer.Write(v);
        }

        public void WriteInt(int v)
        {
            byte[] bData = BitConverter.GetBytes(v);
            Array.Reverse(bData);
            writer.Write(bData);
        }

        public void WriteShort(ushort v)
        {
            byte[] bData = BitConverter.GetBytes(v);
            Array.Reverse(bData);
            writer.Write(bData);
        }

        public void WriteLong(long v)
        {
            writer.Write((long)v);
        }

        public void WriteFloat(float v)
        {
            byte[] temp = BitConverter.GetBytes(v);
            Array.Reverse(temp);
            writer.Write(BitConverter.ToSingle(temp, 0));
        }

        public void WriteDouble(double v)
        {
            byte[] temp = BitConverter.GetBytes(v);
            Array.Reverse(temp);
            writer.Write(BitConverter.ToDouble(temp, 0));
        }

        public void WriteString(string v)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(v);
            writer.Write((ushort)bytes.Length);
            writer.Write(bytes);
        }

        public void WriteBytes(byte[] v)
        {
            //writer.Write((int)v.Length);
            writer.Write(v);
        }

        public byte[] ToBytes()
        {
            writer.Flush();
            return stream.ToArray();
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
        }

        public void Flush()
        {
            writer.Flush();
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