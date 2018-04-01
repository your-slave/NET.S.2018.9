using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace StreamsDemo
{
    // C# 6.0 in a Nutshell. Joseph Albahari, Ben Albahari. O'Reilly Media. 2015
    // Chapter 15: Streams and I/O
    // Chapter 6: Framework Fundamentals - Text Encodings and Unicode
    // https://msdn.microsoft.com/ru-ru/library/system.text.encoding(v=vs.110).aspx

    public static class StreamsExtension
    {

        #region Public members

        #region TODO: Implement by byte copy logic using class FileStream as a backing store stream .

        public static int ByByteCopy(string sourcePath, string destinationPath)
        {
            long result;

            using (FileStream inStream = File.Open(sourcePath, FileMode.Open))
            {
                using (FileStream outStream = File.Create(destinationPath))
                {
                    while (inStream.Position < inStream.Length)
                    {
                        outStream.WriteByte((byte)inStream.ReadByte());
                    }

                    result = inStream.Position;
                }
            }

            return (int)result;
        }

        #endregion

        #region TODO: Implement by byte copy logic using class MemoryStream as a backing store stream.

        public static int InMemoryByByteCopy(string sourcePath, string destinationPath)
        {
            // TODO: step 1. Use StreamReader to read entire file in string

            // TODO: step 2. Create byte array on base string content - use  System.Text.Encoding class

            // TODO: step 3. Use MemoryStream instance to read from byte array (from step 2)

            // TODO: step 4. Use MemoryStream instance (from step 3) to write it content in new byte array

            // TODO: step 5. Use Encoding class instance (from step 2) to create char array on byte array content

            // TODO: step 6. Use StreamWriter here to write char array content in new file

            int result;

            string fileString;

            //step 1

            using (StreamReader streamReader = new StreamReader(sourcePath))
            {
                fileString = streamReader.ReadToEnd();
            }

            //step 2

            byte[] streamReaderByteArr = new byte[fileString.Length];

            Encoding.Convert(Encoding.Unicode, Encoding.Unicode, streamReaderByteArr);

            byte[] memoryStreamByteArr = new byte[streamReaderByteArr.Length];

            using (MemoryStream memoryStream = new MemoryStream(streamReaderByteArr.Length))
            {
                //step 3

                memoryStream.Write(streamReaderByteArr, 0, streamReaderByteArr.Length);

                //step 4

                int count;

                do
                {
                    count = memoryStream.Read(memoryStreamByteArr, 0, streamReaderByteArr.Length);
                }
                while (count > 0);

            }

            //step 5

            char[] charArr = new char[Encoding.Unicode.GetChars(memoryStreamByteArr).Length];

            charArr = Encoding.Unicode.GetChars(memoryStreamByteArr);

            //step 6

            using (StreamWriter streamWriter = new StreamWriter(destinationPath))
            {
                streamWriter.Write(charArr);
            }

            result = Encoding.Unicode.GetBytes(charArr).Length;

            return result;
        }

        #endregion

        #region TODO: Implement by block copy logic using FileStream buffer.

        public static int ByBlockCopy(string sourcePath, string destinationPath)
        {
            int result = 0;

            using (FileStream inStream = new FileStream(sourcePath, FileMode.Open, FileAccess.Read))
            {
                byte[] byteArr = new byte[inStream.Length];

                while (inStream.Position < inStream.Length)
                {
                   inStream.Read(byteArr, (int)inStream.Position, (int)(inStream.Length - inStream.Position));

                }

                using (FileStream outStream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write))
                {
                    outStream.Write(byteArr, 0, byteArr.Length);
                }

                result = byteArr.Length;
            }

            return result;
        }

        #endregion

        #region TODO: Implement by block copy logic using MemoryStream.

        public static int InMemoryByBlockCopy(string sourcePath, string destinationPath)
        {
            int result;

            string fileString;

            //step 1

            using (StreamReader streamReader = new StreamReader(sourcePath))
            {
                fileString = streamReader.ReadToEnd();
            }

            //step 2

            byte[] streamReaderByteArr = new byte[fileString.Length];

            Encoding.Convert(Encoding.Unicode, Encoding.Unicode, streamReaderByteArr);

            byte[] memoryStreamByteArr = new byte[streamReaderByteArr.Length];

            using (MemoryStream memoryStream = new MemoryStream(streamReaderByteArr.Length))
            {
                //step 3

                memoryStream.Write(streamReaderByteArr, 0, streamReaderByteArr.Length);

                //step 4

                int count;

                do
                {
                    count = memoryStream.Read(memoryStreamByteArr, 0, streamReaderByteArr.Length);
                }
                while (count > 0);

            }

            //step 5

            char[] charArr = new char[Encoding.Unicode.GetChars(memoryStreamByteArr).Length];

            charArr = Encoding.Unicode.GetChars(memoryStreamByteArr);

            //step 6

            using (StreamWriter streamWriter = new StreamWriter(destinationPath))
            {
                streamWriter.Write(charArr);
            }

            result = Encoding.Unicode.GetBytes(charArr).Length;

            return result;
        }

        #endregion

        #region TODO: Implement by block copy logic using class-decorator BufferedStream.

        /*
        Microsoft improved the performance of all streams in the .NET Framework by including a built-in buffer. 
        The performance noticeably improved by applying a BufferedStream to existing streams, such as a FileStream or MemoryStream. 
        Applying a BufferedStream to an existing .NET Framework stream results in a double buffer.
        */

        public static int BufferedCopy(string sourcePath, string destinationPath)
        {
            using (FileStream fileStream = File.OpenRead(sourcePath))
            using (BufferedStream bufferedStream = new BufferedStream(fileStream, 20000))
            {
                bufferedStream.ReadByte();
                Console.WriteLine(fileStream.Position);
                // 20000
            }
        }

        #endregion

        #region TODO: Implement by line copy logic using FileStream and classes text-adapters StreamReader/StreamWriter

        public static int ByLineCopy(string sourcePath, string destinationPath)
        {
            List<string> data = new List<string>();
            string line;

            using (FileStream fileStream = File.OpenRead(sourcePath))
            using (TextReader textReader = new StreamReader(fileStream))
            {
                while ((line = textReader.ReadLine()) != null)
                {
                    data.Add(line);
                }
            }

            using (FileStream fileStrean = File.Create("test.txt"))
            using (TextWriter textWriter = new StreamWriter(fileStrean))
            {
                foreach(string element in data)

                textWriter.WriteLine(element);
            }

            int result = Encoding.Unicode.GetBytes(String.Join(String.Empty, data)).Length;

            return result;
        }

        #endregion

        #region TODO: Implement content comparison logic of two files 

        public static bool IsContentEquals(string sourcePath, string destinationPath)
        {
            int file1byte;
            int file2byte;
            FileStream fileStream1;
            FileStream fileStream2;

            if (sourcePath == destinationPath)
            {
                return true;
            }

            fileStream1 = new FileStream(sourcePath, FileMode.Open, FileAccess.Read);
            fileStream2 = new FileStream(destinationPath, FileMode.Open, FileAccess.Read);

            if (fileStream1.Length != fileStream2.Length)
            {
                fileStream1.Close();
                fileStream2.Close();

                return false;
            }

            do
            {
                file1byte = fileStream1.ReadByte();
                file2byte = fileStream2.ReadByte();
            }
            while ((file1byte == file2byte) && (file1byte != -1));

            fileStream1.Close();
            fileStream2.Close();

            return ((file1byte - file2byte) == 0);
        }

        #endregion

        #endregion

        #region Private members

        #region TODO: Implement validation logic

        private static void InputValidation(string sourcePath, string destinationPath)
        {
            if (!Directory.Exists(sourcePath))
            {
                throw new ArgumentException("Invalid source path is entered");
            }
            if (!Directory.Exists(destinationPath))
            {
                throw new ArgumentException("Invalid destinaion path is entered");
            }
        }

        #endregion

        #endregion

    }
}
