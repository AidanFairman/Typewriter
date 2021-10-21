using System;
using System.Text;
using System.Buffers;
using System.Collections.Generic;

namespace Typewriter
{
    class Typewriter
    {
        private LinkedList<byte> buffer = new LinkedList<byte>();
        private Encoding encoding;
        private int dim1, dim2, dim3, val2, loc1, loc2, loc3;
        private Dictionary<char, Action> actions;

        public Typewriter(int len, int wid, int depth, Encoding encoding)
        {
            dim1 = len;
            dim2 = wid;
            dim3 = depth;
            val2 = dim1 * dim2;
            loc1 = loc2 = loc3 = 0;
            this.encoding = encoding;
            actions = new Dictionary<char, Action>();
            actions.Add('<', left);
            actions.Add('>', right);
            actions.Add('^', up);
            actions.Add('v', down);
            actions.Add('V', down);
            actions.Add('+', forward);
            actions.Add('-', back);
            actions.Add('#', print);
            actions.Add('o', toOrigin);
            actions.Add('O', toOrigin);
        }

        public String executeCommands(char[] commands)
        {
            Action action;
            foreach(char command in commands)
            {
                actions.TryGetValue(command, out action);
                action();
            }
            byte[] byteBuffer = new byte[buffer.Count];
            buffer.CopyTo(byteBuffer, 0);
            return encoding.GetString(byteBuffer);
        }

        private void left()
        {
            loc1 -= 1;
            if(loc1 < 0)
            {
                loc1 = dim1 - 1;
            }
        }
        private void right()
        {
            loc1 = (loc1 + 1) % dim1;
        }
        private void up()
        {
            loc2 -= 1;
            if(loc2 < 0)
            {
                loc2 = dim2 - 1;
            }
        }
        private void down()
        {
            loc2 = (loc2 + 1) % dim2;
        }
        private void back()
        {
            loc3 -= 1;
            if(loc3 < 0)
            {
                loc3 = dim3 - 1;
            }
        }
        private void forward()
        {
            loc3 = (loc3 + 1) % dim3;
        }

        private void print()
        {
            int value = loc1 + (loc2 * dim1) + (loc3 * val2);
            while(value > 0)
            {
                buffer.AddLast((byte)(value % byte.MaxValue));
                value /= byte.MaxValue;
            }
        }
        private void toOrigin()
        {
            loc1 = loc2 = loc3 = 0;
        }

        private static void EncodingInfo()
        {
            Encoding encoding = Encoding.Default;

            Console.WriteLine("Default Name: " + Encoding.Default.EncodingName);
            Console.WriteLine("Default Web Name: " + Encoding.Default.WebName);
            Console.WriteLine("UTF7 Name: " + Encoding.UTF7.EncodingName);
            Console.WriteLine("UTF7 Web Name: " + Encoding.UTF7.WebName);
            Console.WriteLine("UTF8 Name: " + Encoding.UTF8.EncodingName);
            Console.WriteLine("UTF8 Web Name: " + Encoding.UTF8.WebName);
            Console.WriteLine("UTF16 Name: " + Encoding.Unicode.EncodingName);
            Console.WriteLine("UTF16 Web Name: " + Encoding.Unicode.WebName);
            Console.WriteLine("UTF32 Name: " + Encoding.UTF32.EncodingName);
            Console.WriteLine("UTF32 Web Name: " + Encoding.UTF32.WebName);
            Console.WriteLine("ASCII Name: " + Encoding.ASCII.EncodingName);
            Console.WriteLine("ASCII Web Name: " + Encoding.ASCII.WebName);
            Console.WriteLine("Latin1 Name: " + Encoding.Latin1.EncodingName);
            Console.WriteLine("Latin1 Web Name: " + Encoding.Latin1.WebName);
        }
        private static void ASCIIKey()
        {
            for(int p = 0 ; p < 4 ; ++p)
            {
                Console.WriteLine("_________________________________");
                for(int h = 0 ; h < 8 ; ++h)
                {
                    Console.Write("|");
                    for(int w = 0 ; w < 8 ; ++w)
                    {
                        Console.Write($"{(char)(w + (h * 8) + (p * 64)),3}|");
                    }
                    Console.WriteLine();
                    Console.WriteLine("_________________________________");
                }
            }
        }
        static void Main(string[] args)
        {
            Typewriter typewriter = new Typewriter(8, 8, 4, Encoding.ASCII);
            string commands = "v+#vvv<<<#v<##>>>#>^-#<^^+#vvv#v>>>#^>>#^#<<<-#";
            //ASCIIKey();
            string output = typewriter.executeCommands(commands.ToCharArray());
            Console.WriteLine(output);
        }
    }
}
