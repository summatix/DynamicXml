using System;
using DynamicXml;

namespace ExampleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            string data = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<xml>
    <errors>
        <error>This is error 1</error>
        <data>
            <str>username</str>
            <str>password</str>
        </data>
    </errors>
    <errors>
        <error>This is error 2</error>
        <error>This is error 3</error>
    </errors>
	<message>This is message</message>
</xml>";

            dynamic xml = new DynamicXmlReader(data);
            if (xml.errors != null)
            {
                Console.WriteLine("xml.errors is set");
                foreach (var errors in xml.errors)
                {
                    if (errors is string)
                    {
                        Console.WriteLine(errors);
                    }
                    else if (errors.error != null)
                    {
                        Console.WriteLine("errors.error is set");

                        if (errors.error is string)
                        {
                            Console.WriteLine(errors.error);
                        }
                        else
                        {
                            foreach (var error in errors.error)
                            {
                                Console.WriteLine(error);
                            }
                        }

                        if (errors.data != null)
                        {
                            Console.WriteLine("errors.error.data is set");
                        }
                    }
                }
            }

            foreach (var errors in xml.errors)
            {
                if (errors.error is string)
                {
                    Console.WriteLine(errors.error);
                }
                else
                {
                    foreach (var error in errors.error)
                    {
                        Console.WriteLine(error);
                    }
                }

                if (errors.data is IXElement)
                {
                    foreach (var str in errors.data.str)
                    {
                        Console.WriteLine(str);
                    }
                }
            }

            if (xml.message != null)
            {
                Console.WriteLine(xml.message);
            }

            foreach (var el in xml)
            {
                Console.WriteLine(el);
            }
        }
    }
}
